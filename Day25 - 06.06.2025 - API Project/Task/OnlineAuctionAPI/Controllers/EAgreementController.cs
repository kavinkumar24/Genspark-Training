using Microsoft.AspNetCore.Mvc;
using OnlineAuctionAPI.Interfaces;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using OnlineAuctionAPI.Models;
using Microsoft.AspNetCore.Authorization;
using OnlineAuctionAPI.Services;

namespace OnlineAuctionAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EAgreementController : ControllerBase
    {
        private readonly IEAgreementService _eAgreementService;

        public EAgreementController(IEAgreementService eAgreementService)
        {
            _eAgreementService = eAgreementService;
        }

        [HttpGet("myAgreements")]
        [Authorize(Roles = "Bidder")]
        public async Task<IActionResult> GetMyAgreements()
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("UserId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Please provide a valid userId.",
                    Data = null
                });
            }

            var result = await _eAgreementService.GetByUserIdAsync(userId.ToString());
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Agreements retrieved successfully.",
                Data = result
            });
        }

        [HttpGet("{id}/download")]
        [Authorize(Roles = "Bidder")]
        public async Task<IActionResult> DownloadAgreementFile(Guid id)
        {
            var agreement = await _eAgreementService.GetByIdAsync(id);
            if (agreement == null || agreement.File == null)
            {
                return NotFound("Agreement or file not found.");
            }

            var fileName = $"agreement_{agreement.Id}.pdf"; 
            var contentType = "application/pdf"; 

            return File(agreement.File, contentType, fileName);
        }
    }
}