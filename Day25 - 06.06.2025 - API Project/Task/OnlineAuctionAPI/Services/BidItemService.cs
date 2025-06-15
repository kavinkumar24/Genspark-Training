
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineAuctionAPI.Exceptions;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Repositories;
using OnlineAuctionAPI.Services;

namespace OnlineAuctionAPI.Service;

public class BidItemService : IBidItemService
{
    private readonly IBidItemRepository _bidItemRepository;
    private readonly IAuctionItemRepository _auctionItemRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public BidItemService(IBidItemRepository bidItemRepository, IAuctionItemRepository auctionItemRepository,IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _bidItemRepository = bidItemRepository;
        _auctionItemRepository = auctionItemRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BidItemResponseDto> PlaceBidAsync(BidItemAddDto bidDto)
    {
        var auctionItem = await _auctionItemRepository.Get(bidDto.AuctionItemId);
        if (auctionItem == null)
        {
            throw new NullValueException("Auction not found, please enter valid one.");
        }
        if (auctionItem.Status != AuctionStatus.Live)
        {
            throw new InvalidDataException("Auction is not live.");
        }
        if (auctionItem.EndTime < DateTime.UtcNow)
        {
            throw new InvalidDataException("Auction has ended.");
        }
        if (auctionItem.StartTime > DateTime.UtcNow)
        {
            throw new InvalidDataException("Auction has not started yet.");
        }
        if (auctionItem.WinnerId != null)
        {
            throw new InvalidDataException("This auction has decalred the winner.");
        }
        if (auctionItem.StartingPrice > bidDto.Amount)
        {
            throw new InvalidDataException("Please place the bid above the starting price of the auctions.");
        }
        if (bidDto.Amount <= 0)
        {
            throw new InvalidDataException("Bid amount must be greater than zero.");
        }
       
        var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var loggedInUserId))
        {
            throw new InvalidDataException("You are not authorized to place this bid.");
        }

        bidDto.BidderId = loggedInUserId;
        var user = await _userRepository.GetByIdWithVirtualWalletAsync(loggedInUserId);
        if (user?.VirtualWallet == null)
            throw new InvalidDataException("You do not have a virtual wallet. Please create one before bidding.");
        if (user.VirtualWallet.Balance < bidDto.Amount)
            throw new InvalidDataException("Insufficient wallet balance to place this bid.");

        var highestBid = await _bidItemRepository.GetHighestBidAsync(bidDto.AuctionItemId);

        if (highestBid != null)
        {
            if (bidDto.Amount <= highestBid.Amount)
                throw new InvalidDataException($"Your bid must be higher than the current highest bid ({highestBid.Amount}) - for the auction {bidDto.AuctionItemId}.");
        }
        else
        {
            if (bidDto.Amount < auctionItem.StartingPrice)
                throw new InvalidDataException("Please place the bid above the starting price of the auction.");
        }
        var existingBids = await _bidItemRepository.GetBidsByAuctionAsync(bidDto.AuctionItemId);
        var userPreviousBid = existingBids?.FirstOrDefault(b => b.BidderId == bidDto.BidderId);

        if (userPreviousBid != null && bidDto.Amount <= userPreviousBid.Amount)
        {
            throw new InvalidDataException("Your new bid must be greater than your previous bid for this auction.");
        }

        var bidItem = _mapper.Map<BidItem>(bidDto);
        await _bidItemRepository.Add(bidItem);
        return _mapper.Map<BidItemResponseDto>(bidItem);
    }

    public async Task<IEnumerable<BidItemResponseDto>> GetAllBidAsync()
    {
        var allBids = await _bidItemRepository.GetAll();
        if (allBids == null || !allBids.Any())
        {
            throw new RepositoryOperationException(
                "No bids available right now",
                new NotFoundException("No bids available right now")
            );
        }
        return _mapper.Map<IEnumerable<BidItemResponseDto>>(allBids);
    }

    public async Task<IEnumerable<BidItemResponseDto>> GetBidsByAuctionIdAsync(Guid auctionId)
    {
        var bids = await _bidItemRepository.GetBidsByAuctionAsync(auctionId);

        if (bids == null)
        {
            throw new NotFoundException($"No bids available for this auction{auctionId}");
        }

        return _mapper.Map<IEnumerable<BidItemResponseDto>>(bids);
    }

    public Task<IEnumerable<BidItemResponseDto>> GetBidsByUseridAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new NullValueException("User ID cannot be empty");
        }

        var bids = _bidItemRepository.GetBidsByUserIdAsync(userId);
        if (bids == null)
        {
            throw new NotFoundException($"No bids available for user {userId}");
        }

        return Task.FromResult(_mapper.Map<IEnumerable<BidItemResponseDto>>(bids));
    }

    public async Task<BidItemResponseDto?> GetHighestBidAsync(Guid auctionItemId)
    {
        var highestBid = await _bidItemRepository.GetHighestBidAsync(auctionItemId);
        return _mapper.Map<BidItemResponseDto>(highestBid);
    }

    public async Task<bool> DeleteBidAsync(Guid bidId)
    {
        var bid = await _bidItemRepository.Get(bidId);
        if (bid == null)
        {
            throw new NotFoundException($"Bid with ID {bidId} not found");
        }
        var deleteBidItem = await _bidItemRepository.Delete(bid.Id);
        if (deleteBidItem == null)
        {
            throw new NotFoundException($"Bid with ID {bidId} not found");
        }
        return true;
    }
}