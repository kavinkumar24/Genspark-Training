using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Services;
using OnlineAuctionAPI.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

namespace OnlineAuctionAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BidItemController : ControllerBase
{
    private readonly IBidItemService _bidItemService;
    private readonly ILogger<BidItemController> _logger;
    private readonly IHubContext<AuctionHub> _hubContext;

    public BidItemController(IBidItemService bidItemService, ILogger<BidItemController> logger, IHubContext<AuctionHub> hubContext)
    {
        _bidItemService = bidItemService;
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpPost]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult> AddBidItem([FromBody] BidItemAddDto bidDto)
    {
        var result = await _bidItemService.PlaceBidAsync(bidDto);
        _logger.LogInformation("Bid placed successfully for auction item {AuctionItemId} by user {UserId}", bidDto.AuctionItemId, bidDto.BidderId);
        await _hubContext.Clients.All.SendAsync("BidPlaced", result);
        return Ok(new ApiResponse<BidItemResponseDto>
        {
            Success = true,
            Message = "Bid placed successfully",
            Data = result
        });
    }

    [HttpGet("{auctionId}")]
    [Authorize(Roles = "Seller,Bidder, Admin")]
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

    [HttpGet("ByBidder")]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult<IEnumerable<BidItemResponseDto>>> GetBidsByBidderId()
    {
        try
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("Invalid or missing userId in token");
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Please provide a valid userId.",
                    Data = null
                });
            }

            var bidderItems = await _bidItemService.GetBidsByUserIdAsync(userId);
            if (bidderItems == null)
            {
                _logger.LogWarning("No bidding data found");
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No bidding Items found",
                    Data = null
                });
            }
            return Ok(new ApiResponse<IEnumerable<BidItemResponseDto>>
            {
                Success = true,
                Message = "Auctions for seller id",
                Data = bidderItems
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bids for user");
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = "An unexpected error occurred.",
                Data = null
            });
        }
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
    [Authorize(Roles = "Admin")]
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

    [HttpGet("ByBiddingId/{biddingId}")]
    [Authorize(Roles = "Seller,Bidder, Admin")]
    public async Task<ActionResult<IEnumerable<BidItemResponseDto>>> GetBidsByBiddingId(Guid biddingId)
    {
        try
        {
            if (biddingId == Guid.Empty)
            {
                _logger.LogWarning("Bidding ID cannot be empty");
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Bidding ID cannot be empty",
                    Data = null
                });
            }
            var bids = await _bidItemService.GetBidsByBiddingIdAsync(biddingId);
            if (bids == null)
            {
                _logger.LogWarning("No bids found for bidding ID {BiddingId}", biddingId);
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"No bids found for bidding ID {biddingId}",
                    Data = null
                });
            }
            _logger.LogInformation("Bids retrieved successfully for bidding ID {BiddingId}", biddingId);
            return Ok(new ApiResponse<BidItemResponseDto>
            {
                Success = true,
                Message = "Bids retrieved successfully",
                Data = bids
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bids for bidding ID {BiddingId}", biddingId);
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = "An unexpected error occurred.",
                Data = null
            });
        }
    }
}