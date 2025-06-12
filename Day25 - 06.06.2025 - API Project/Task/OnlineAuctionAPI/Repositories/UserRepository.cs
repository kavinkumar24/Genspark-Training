
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Exceptions;
namespace OnlineAuctionAPI.Repositories;

public class UserRepository : Repository<Guid, User>, IUserRepository
{
    public UserRepository(AuctionContext context) : base(context)
    {

    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        InputValidator.ValidateString(username, nameof(username));

        try
        {
            var users = await _auctionContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
            return users;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get users by userName", ex);
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        InputValidator.ValidateString(email, nameof(email));
        try
        {
            var usersByEmail = await _auctionContext.Users
                                .FirstOrDefaultAsync(e => e.Email == email);
            return usersByEmail;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get users by Email", ex);
        }
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _auctionContext.Users
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
        return user;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
{
    return await _auctionContext.RefreshTokens
        .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
}
}