using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Interfaces;

public interface IBidItemService
{
    Task<BidItemResponseDto> PlaceBidAsync(BidItemAddDto bidDto);
    Task<IEnumerable<BidItemResponseDto>> GetAllBidAsync();
    Task<IEnumerable<BidItemResponseDto>> GetBidsByAuctionIdAsync(Guid auctionId);
    Task<IEnumerable<BidItemResponseDto>> GetBidsByUseridAsync(Guid userId);
    Task<BidItemResponseDto?> GetHighestBidAsync(Guid auctionItemId);
    Task<bool> DeleteBidAsync(Guid bidId);

}