using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Interfaces;

public interface IBidItemService
{
    Task<BidItemResponseDto> PlaceBidAsync(BidItemAddDto bidDto);
    Task<IEnumerable<BidItemResponseDto>> GetAllBidAsync();
    Task<IEnumerable<BidItemResponseDto>> GetBidsByAuctionIdAsync(Guid auctionId);
    Task<BidItemResponseDto?> GetHighestBidAsync(Guid auctionItemId);
    Task<bool> DeleteBidAsync(Guid bidId);
    Task<IEnumerable<BidItemResponseDto>> GetBidsByUserIdAsync(Guid userId);
    Task<BidItemResponseDto> GetBidsByBiddingIdAsync(Guid biddingId);
}
