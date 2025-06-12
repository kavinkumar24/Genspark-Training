using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Services;

namespace OnlineAuctionAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BidItemController : ControllerBase
{
    private readonly IBidItemService _bidItemService;
    private readonly ILogger<BidItemController> _logger;

    public BidItemController(IBidItemService bidItemService, ILogger<BidItemController> logger)
    {
        _bidItemService = bidItemService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult> AddBidItem([FromBody] BidItemAddDto bidDto)
    {
        var result = await _bidItemService.PlaceBidAsync(bidDto);
        _logger.LogInformation("Bid placed successfully for auction item {AuctionItemId} by user {UserId}", bidDto.AuctionItemId, bidDto.BidderId);
        return Ok(new ApiResponse<BidItemResponseDto>
        {
            Success = true,
            Message = "Bid placed successfully",
            Data = result
        });
    }

    [HttpGet("{auctionId}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> GetBidsByAuctionId(Guid auctionId)
    {
        var result = await _bidItemService.GetBidsByAuctionIdAsync(auctionId);
        _logger.LogInformation("Bids retrieved successfully for auction item {AuctionItemId}", auctionId);
        return Ok(new ApiResponse<IEnumerable<BidItemResponseDto>>
        {
            Success = true,
            Message = "Bids retrieved successfully",
            Data = result
        });
    }

    [HttpGet("HighestBid/{auctionItemId}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> GetHighestBids(Guid auctionItemId)
    {
        var result = await _bidItemService.GetHighestBidAsync(auctionItemId);
        _logger.LogInformation("Highest bid retrieved successfully for auction item {AuctionItemId}", auctionItemId);
        return Ok(new ApiResponse<BidItemResponseDto>
        {
            Success = true,
            Message = "Highest bid retrieved successfully",
            Data = result
        });
    }

    [HttpDelete("{bidId}")]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult> DeleteBid(Guid bidId)
    {
        var result = await _bidItemService.DeleteBidAsync(bidId);
        _logger.LogInformation("Bid with ID {BidId} deleted successfully", bidId);
        return Ok(new ApiResponse<bool>
        {
            Success = true,
            Message = "Bid deleted successfully",
            Data = result
        });
    }
}