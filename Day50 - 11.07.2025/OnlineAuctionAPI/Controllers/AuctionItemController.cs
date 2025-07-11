using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Helpers;
using OnlineAuctionAPI.Hubs;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuctionItemController : ControllerBase
{
    private readonly IAuctionItemService _auctionItemService;
    private readonly IHubContext<AuctionHub> _hubContext;

    private readonly AuctionContext _auctioncontext;
    private readonly ILogger<AuctionItemController> _logger;

    public AuctionItemController(
        IAuctionItemService auctionItemService,
        AuctionContext auctionContext,
        ILogger<AuctionItemController> logger,
        IHubContext<AuctionHub> hubContext
    )
    {
        _auctionItemService = auctionItemService;
        _auctioncontext = auctionContext;
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpPost]
    [Authorize(Roles = "Seller")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuctionItemResponseDto>> AddAuctionItem(
        [FromForm] AuctionItemAddDto dto
    )
    {
        var result = await _auctionItemService.AddAuctionItemAsync(dto);
        _logger.LogInformation("Auction item created successfully with ID {AuctionId}", result.Id);
        await _hubContext.Clients.All.SendAsync("AuctionItemAdded", result);
        return CreatedAtAction(
            nameof(GetAuctionById),
            new { auctionId = result.Id },
            ApiResponseHelper.CreateSuccess(result, "Auction item created successfully")
        );
    }

    [HttpGet("{auctionId}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult<AuctionItemResponseDto>> GetAuctionById(Guid auctionId)
    {
        var result = await _auctionItemService.GetAuctionItemByIdAsync(auctionId);
        if (result == null)
        {
            _logger.LogWarning("Auction item with ID {AuctionId} not found", auctionId);
            return NotFound(ApiResponseHelper.CreateNotFound<string>("Auction item not found"));
        }
        _logger.LogInformation(
            "Auction item with ID {AuctionId} retrieved successfully",
            auctionId
        );
        return Ok(ApiResponseHelper.CreateSuccess(result, "Auction item retrieved successfully"));
    }

    [HttpGet]
    [Authorize(Roles = "Seller,Bidder, Admin")]
    public async Task<ActionResult<IEnumerable<AuctionItemResponseDto>>> GetAllAuctions()
    {
        var results = await _auctionItemService.GetAllAuctionItemAsync();
        _logger.LogInformation("All auction items retrieved successfully");
        return Ok(
            ApiResponseHelper.CreateSuccess(results, "All auction items retrieved successfully")
        );
    }

    [HttpGet("BySeller")]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<IEnumerable<AuctionItemResponseDto>>> GetAuctionBySeller()
    {
        var userIdClaim = User.FindFirst("UserId");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning("Invalid or missing userId in token");
            return BadRequest(
                ApiResponseHelper.CreateBadRequest<string>("Please provide a valid userId.")
            );
        }

        var sellerAuctions = await _auctionItemService.GetAllAuctionItemBySellerAsync(userId);
        if (sellerAuctions == null)
        {
            _logger.LogWarning("No seller id found in auctions");
            return NotFound(
                ApiResponseHelper.CreateNotFound<string>("No seller found within auction items")
            );
        }
        return Ok(ApiResponseHelper.CreateSuccess(sellerAuctions, "Auctions for seller id"));
    }

    // [HttpGet("download/{auctionItemId}/{fileName}")]
    // [Authorize(Roles = "Seller,Bidder")]
    // public async Task<ActionResult> DownloadFile(Guid auctionItemId, string fileName)
    // {
    //     var fileData = await _auctioncontext.Files.FirstOrDefaultAsync(f =>
    //         f.AuctionItemId == auctionItemId && f.Name == fileName
    //     );
    //     if (fileData == null)
    //     {
    //         _logger.LogWarning(
    //             "File not found for auction item {AuctionItemId} and file {FileName}",
    //             auctionItemId,
    //             fileName
    //         );
    //         return NotFound(
    //             ApiResponseHelper.CreateNotFound<string>("File not found for this auction item.")
    //         );
    //     }
    //     _logger.LogInformation("File for auction item {AuctionItemId} retrieved", auctionItemId);
    //     return File(fileData.Data, fileData.ContentType, fileData.Name);
    // }
    [HttpGet("download/{auctionItemId}/{fileName}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> DownloadFile(Guid auctionItemId, string fileName)
    {
        var fileData = await _auctioncontext.Files.FirstOrDefaultAsync(f =>
            f.AuctionItemId == auctionItemId && f.Name == fileName
        );
        if (fileData == null || string.IsNullOrEmpty(fileData.BlobUrl))
        {
            _logger.LogWarning(
                "File or Blob URL not found for auction item {AuctionItemId} and file {FileName}",
                auctionItemId,
                fileName
            );
            return NotFound("File or Blob URL not found for this auction item.");
        }

        _logger.LogInformation(
            "Blob URL for auction item {AuctionItemId} retrieved",
            auctionItemId
        );
        return Ok(new { url = fileData.BlobUrl });
    }

    [Consumes("multipart/form-data")]
    [HttpPut("UpdateAuctionItem")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<ActionResult> UpdateAuctionItem(
        Guid id,
        [FromForm] AuctionItemAddDto auctionDto
    )
    {
        var result = await _auctionItemService.UpdateAuctionItemAsync(id, auctionDto);
        _logger.LogInformation("Auction item with ID {AuctionId} updated successfully", id);
        return Ok(ApiResponseHelper.CreateSuccess(result, "Auction item updated successfully"));
    }

    [HttpGet("PagedData")]
    [Authorize(Roles = "Seller,Bidder, Admin")]
    public async Task<
        ActionResult<PaginatedResponseDto<AuctionItemResponseDto>>
    > GetPagedAuctionItem([FromQuery] PaginationDto paginationDto)
    {
        var result = await _auctionItemService.GetPagedAuctionItemsAsync(paginationDto);
        _logger.LogInformation("Paged auction items retrieved successfully");
        return Ok(
            ApiResponseHelper.CreateSuccess(result, "Paged auction items retrieved successfully")
        );
    }

    [HttpDelete("{auctionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteAuctionItem(Guid auctionId)
    {
        var result = await _auctionItemService.DeleteAuctionItemAsync(auctionId);
        _logger.LogInformation(
            "Auction item with ID {AuctionId} deleted: {Result}",
            auctionId,
            result
        );
        return Ok(
            ApiResponseHelper.CreateSuccess(
                result,
                result ? "Auction item deleted successfully" : "Failed to delete auction item"
            )
        );
    }

    [HttpPatch("UpdateWinningId")]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<WinnerIdResponseDto>> UpdateWinningId(
        [FromBody] WinningIdUpdateDto winningIdUpdateDto
    )
    {
        var result = await _auctionItemService.UpdateWinningId(winningIdUpdateDto);
        _logger.LogInformation(
            "Winning ID updated for auction item {AuctionItemId}",
            winningIdUpdateDto.AuctionItemId
        );
        await _hubContext.Clients.All.SendAsync("WinningIdUpdated", result);
        return Ok(ApiResponseHelper.CreateSuccess(result, "Winning ID updated successfully"));
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<AuctionItemResponseDto>> UpdateAuctionStatus(
        Guid id,
        [FromQuery] AuctionStatus newStatus
    )
    {
        var result = await _auctionItemService.UpdateAuctionStatusAsync(id, newStatus);
        await _hubContext.Clients.All.SendAsync("AuctionStatusUpdated", result);
        _logger.LogInformation(
            "Auction item with ID {AuctionId} status updated to {Status}",
            id,
            newStatus
        );
        return Ok(
            ApiResponseHelper.CreateSuccess(result, $"Auction item status updated to {newStatus}")
        );
    }

    [HttpGet("GetActiveAuctions")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult<IEnumerable<AuctionItemResponseDto>>> GetActiveAuctions()
    {
        var results = await _auctionItemService.GetActiveAuctionsAsync();
        _logger.LogInformation("Active auctions retrieved successfully");
        return Ok(
            ApiResponseHelper.CreateSuccess(results, "Active auctions retrieved successfully")
        );
    }

    [HttpGet("GetAuctionWinner/{auctionId}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult<WinnerIdResponseDto>> GetAuctionWinner(Guid auctionId)
    {
        var result = await _auctionItemService.GetAuctionWinnerAsync(auctionId);
        if (result == null)
        {
            _logger.LogWarning("No winner found for auction item {AuctionId}", auctionId);
            return NotFound(
                ApiResponseHelper.CreateNotFound<string>("No winner found for this auction item.")
            );
        }
        _logger.LogInformation(
            "Winner retrieved successfully for auction item {AuctionId}",
            auctionId
        );
        return Ok(ApiResponseHelper.CreateSuccess(result, "Winner retrieved successfully"));
    }
}
