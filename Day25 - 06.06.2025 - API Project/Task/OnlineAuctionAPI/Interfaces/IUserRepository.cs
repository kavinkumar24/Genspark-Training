using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;

public interface IUserRepository : IRepository<Guid, User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
    Task<User?> GetByIdWithVirtualWalletAsync(Guid id);

    Task AddVirtualWalletAsync(Guid userId, VirtualWalletAddDto dto);
    Task AddFundsToWalletAndHistoryAsync(Guid userId, decimal amount);
}