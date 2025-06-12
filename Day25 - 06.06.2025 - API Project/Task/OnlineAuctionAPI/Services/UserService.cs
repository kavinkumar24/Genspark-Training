
using AutoMapper;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OnlineAuctionAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _mapper = mapper;
    }

    public async Task<User> CreateUserAsync(UserRegisterRequestDto userDto)
    {
        var existingUserData = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUserData != null)
        {
            throw new AlreadyExistsException("Some user already exists with this email.");
        }
        var user = _mapper.Map<User>(userDto);
        user.Password = _passwordService.HashPassword(user.Password);
        await _userRepository.Add(user);
        return user;
    }

    public async Task<User> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.Get(id);
        if (user.StatusId == 2)
        {
            throw new AlreadyDeletedException("User is already deleted");
        }

        user.StatusId = 2;
        await _userRepository.Update(user.Id, user);
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new NullValueException("Email can't be null or empty please ensure it");
        }
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"The given email - {email} is not found, please verify that.");
        }
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.Get(id);
        return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new NullValueException("User name can't be null or empty, please ensure it");
        }
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new NotFoundException($"The user with username {username} is not found");
        }
        return user;
    }

    public async Task<User> UpdateUserInfoAsync(Guid id, UserUpdateRequestDto updateuserdto)
    {
        var user = await _userRepository.Get(id);
        if (user == null)
        {
            throw new NotFoundException($"The user with Id {id} is not found, please ensure the UserId");
        }
        _mapper.Map(updateuserdto, user);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.Update(id, user);
        return user;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.Get(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (!_passwordService.VerifyPassword(user.Password, currentPassword))
            throw new Exception("Current password is incorrect");

        user.Password = _passwordService.HashPassword(newPassword);
        await _userRepository.Update(userId, user);
        return true;
    }
    
}