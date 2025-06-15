using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Migrations;
using OnlineAuctionAPI.Exceptions;

namespace OnlineAuctionAPI.Repositories
{
    public class VirtualWalletRepository : Repository<Guid, VirtualWallet>, IVirtualWalletRepository
    {
        public VirtualWalletRepository(AuctionContext context) : base(context)
        {

        }

        public async Task AddHistoryAsync(VirtualWalletHistory history)
        {
            try
            {
                _auctionContext.VirtualWalletHistories.Add(history);
                await _auctionContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryOperationException("Adding virtual wallet history", ex);
            }
        }

        public async Task<VirtualWallet?> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _auctionContext.VirtualWallets.FirstOrDefaultAsync(w => w.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new RepositoryOperationException("Getting virtual wallet by userId", ex);
            }
        }

        public async Task UpdateAsync(VirtualWallet wallet)
        {
            try
            {
                _auctionContext.VirtualWallets.Update(wallet);
                await _auctionContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryOperationException("Updating virtual wallet", ex);
            }
        }
    }
}