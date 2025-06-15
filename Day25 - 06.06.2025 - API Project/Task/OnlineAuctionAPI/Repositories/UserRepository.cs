
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Exceptions;
namespace OnlineAuctionAPI.Repositories;

using OnlineAuctionAPI.Models.DTO;

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

    public async Task<User?> GetByIdWithVirtualWalletAsync(Guid id)
    {
        try
        {
            var user = await _auctionContext.Users
                .Include(u => u.VirtualWallet)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get user by Id with Virtual Wallet", ex);
        }
    }



    public async Task AddVirtualWalletAsync(Guid userId, VirtualWalletAddDto dto)
    {
        using var transaction = await _auctionContext.Database.BeginTransactionAsync();
        try
        {
            var virtualWallet = new VirtualWallet
            {
                UserId = userId,
                Balance = dto.Balance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _auctionContext.VirtualWallets.AddAsync(virtualWallet);

            var history = new VirtualWalletHistory
            {
                Id = Guid.NewGuid(),
                VirtualWalletId = virtualWallet.Id,
                Amount = dto.Balance,
                TransactionDate = DateTime.UtcNow
            };

            await _auctionContext.VirtualWalletHistories.AddAsync(history);

            await _auctionContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new RepositoryOperationException("Add virtual wallet and history", ex);
        }
    }

    public async Task AddFundsToWalletAndHistoryAsync(Guid userId, decimal amount)
    {
        using var transaction = await _auctionContext.Database.BeginTransactionAsync();
        try
        {
            var user = await _auctionContext.Users
                .Include(u => u.VirtualWallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.VirtualWallet == null)
                throw new Exception("User or wallet not found");

            user.VirtualWallet.Balance += amount;
            user.VirtualWallet.UpdatedAt = DateTime.UtcNow;

            var history = new VirtualWalletHistory
            {
                Id = Guid.NewGuid(),
                VirtualWalletId = user.VirtualWallet.Id,
                Amount = amount,
                TransactionDate = DateTime.UtcNow
            };

            _auctionContext.VirtualWalletHistories.Add(history);
            await _auctionContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new RepositoryOperationException("Add funds to wallet and history", ex);
        }
    }

}