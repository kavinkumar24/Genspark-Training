
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Exceptions;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Repositories;

public class BidItemRepository : Repository<Guid, BidItem>, IBidItemRepository
{
    public BidItemRepository(AuctionContext context) : base(context)
    {

    }

    public async Task<IEnumerable<BidItem>> GetBidsByAuctionAsync(Guid auctionItemId)
    {
        try
        {
            var auction = await _auctionContext.BidItems
                                .Where(b => b.AuctionItemId == auctionItemId)
                                .Include(b => b.Bidder)
                                .OrderByDescending(b => b.Amount)
                                .ToListAsync();
            return auction;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Getting auction by auctionId", ex);
        }
    }

    public async Task<BidItem?> GetHighestBidAsync(Guid auctionItemId)
    {
        try
        {
            var highestBid = await _auctionContext.BidItems
                                    .Where(b => b.AuctionItemId == auctionItemId)
                                    .OrderByDescending(b => b.Amount)
                                    .ThenBy(b => b.BidTime)
                                    .FirstOrDefaultAsync();
            return highestBid;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Getting Highest bid in the auction", ex);
        }
    }

    public async Task<IEnumerable<BidItem>> GetBidsByUserIdAsync(Guid userId)
    {
        return await _auctionContext.BidItems.Where(b => b.Id == userId).ToListAsync();
    }

    public async Task<BidItem?> GetByIdAsync(Guid bidId)
    {
        return await _auctionContext.BidItems
            .FirstOrDefaultAsync(b => b.Id == bidId);
    }
}