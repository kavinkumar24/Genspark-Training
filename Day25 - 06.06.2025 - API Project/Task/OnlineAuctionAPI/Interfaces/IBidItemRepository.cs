using OnlineAuctionAPI.Models;
namespace OnlineAuctionAPI.Interfaces;

public interface IBidItemRepository : IRepository<Guid, BidItem>
{
    Task<IEnumerable<BidItem>> GetBidsByAuctionAsync(Guid auctionItemId);
    Task<BidItem?> GetHighestBidAsync(Guid auctionItemId);

    Task<IEnumerable<BidItem>> GetBidsByUserIdAsync(Guid userId);
    Task<BidItem?> GetByIdAsync(Guid bidId);

}