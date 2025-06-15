using OnlineAuctionAPI.Models;
using System;
using System.Threading.Tasks;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IVirtualWalletRepository : IRepository<Guid, VirtualWallet>
    {

        Task AddHistoryAsync(VirtualWalletHistory history);
        Task<VirtualWallet?> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(VirtualWallet wallet);

    }
}