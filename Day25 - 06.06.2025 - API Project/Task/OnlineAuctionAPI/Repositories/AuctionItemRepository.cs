using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Exceptions;
using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Repositories;

public class AuctionRepository : Repository<Guid, AuctionItem>, IAuctionItemRepository
{
    public AuctionRepository(AuctionContext context) : base(context)
    {

    }
    public async Task<IEnumerable<AuctionItem>> GetActiveAuctionsAsync()
    {
        try
        {
            var CurrentTime = DateTime.UtcNow;
            var auctionItem = await _auctionContext.AuctionItems
                                .Where(a => a.Status == AuctionStatus.Live
                                && a.StartTime <= CurrentTime && a.EndTime >= CurrentTime)
                                .ToListAsync();
            return auctionItem;

        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get live autions", ex);
        }
    }

    public async Task<AuctionItem?> GetBidsAsync(Guid auctionItemId)
    {
        try
        {
            var bidsItem = await _auctionContext.AuctionItems
                            .Include(a => a.Bids)
                            .ThenInclude(b => b.Bidder)
                            .FirstOrDefaultAsync(a => a.Id == auctionItemId);
            return bidsItem;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get bids from autions", ex);
        }
    }

    public async Task<IEnumerable<AuctionItem>> GetAllAuctionsWithFile()
    {
        try
        {
            var auctionItems = await _auctionContext.AuctionItems
                                        .Include(a => a.FileAttachments)
                                        .Include(a => a.Seller)
                                        .ToListAsync();
            return auctionItems;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get all auctions with attachments", ex);
        }
    }

    public async Task<AuctionItem> GetAuctionsWithFile(Guid id)
    {
        try
        {
            var auctionItems = await _auctionContext.AuctionItems
                                        .Include(a => a.FileAttachments)
                                        .Include(a => a.Seller)
                                        .FirstOrDefaultAsync(a => a.Id == id);
            return auctionItems;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Get all auctions with attachments", ex);
        }
    }

    public async Task<(IEnumerable<AuctionItem>, int)> GetPagedItemsAsync(int page, int pageSize)
    {
        var query = _auctionContext.AuctionItems.AsQueryable();

        int totalRecords = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalRecords);
    }

    public IQueryable<AuctionItem> GetAllQueryable()
    {
        return _auctionContext.AuctionItems.AsQueryable();
    }

    public async Task<WinnerIdResponseDto> UpdateWinningId(WinningIdUpdateDto dto)
    {
        var bid = await _auctionContext.BidItems
            .FirstOrDefaultAsync(b => b.Id == dto.WinningId);

        if (bid == null)
            throw new NotFoundException("Bid not found for the provided WinningId.");

        if (bid.AuctionItemId != dto.AuctionItemId)
            throw new InvalidException("Bid does not belong to the specified auction item.");

        var auctionItem = await _auctionContext.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == dto.AuctionItemId);

        if (auctionItem == null)
            throw new NotFoundException($"Auction item with ID {dto.AuctionItemId} not found");

        var maxAmount = await _auctionContext.BidItems
            .Where(b => b.AuctionItemId == dto.AuctionItemId && b.BidTime <= auctionItem.EndTime)
            .MaxAsync(b => (decimal?)b.Amount);

        if (maxAmount == null)
            throw new InvalidException("No valid bids found for this auction item.");

        var highestBids = await _auctionContext.BidItems
            .Where(b => b.AuctionItemId == dto.AuctionItemId && b.BidTime <= auctionItem.EndTime && b.Amount == maxAmount)
            .OrderBy(b => b.BidTime)
            .ToListAsync();

        var isWinningBid = highestBids.Any(b => b.Id == bid.Id);

        if (!isWinningBid)
            throw new InvalidException("Only a bid with the highest amount can be declared as the winner.");

        var winningBid = highestBids.First();

        if (bid.Id != winningBid.Id)
            throw new InvalidException("Only the earliest bid with the highest amount can be declared as the winner.");

        auctionItem.WinnerId = bid.Id;
        _auctionContext.AuctionItems.Update(auctionItem);
        await _auctionContext.SaveChangesAsync();

        var winner = await _auctionContext.Users.FirstOrDefaultAsync(u => u.Id == bid.BidderId);

        return new WinnerIdResponseDto
        {
            AuctionItemId = dto.AuctionItemId,
            WinnerId = winner?.Id ?? Guid.Empty,
            WinnerName = winner?.Username ?? "Unknown",
            WinningPrice = bid.Amount
        };
    }


    public async Task<IEnumerable<AuctionItem>> GetAllEndedAndNotCompletedAsync(DateTime now)
    {
        try
        {
            var auctions = await _auctionContext.AuctionItems
                .Where(a => a.EndTime <= now && a.Status != AuctionStatus.Completed)
                .ToListAsync();
            return auctions;
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Error fetching ended and not completed auctions", ex);
        }
    }

    public async Task<IEnumerable<AuctionItem>> GetAllUpcomingAndShouldBeLiveAsync(DateTime now)
    {
        try
        {
            return await _auctionContext.AuctionItems
                .Where(a => a.Status == AuctionStatus.Upcoming && a.StartTime <= now)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryOperationException("Error fetching auctions that should go live", ex);
        }
    }
}