using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Repositories;

public class AuctionItemService : IAuctionItemService
{
    private readonly IAuctionItemRepository _auctionRespository;
    private readonly IUserRepository _userRepository;
    private readonly IBidItemRepository _bidItemRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuctionContext _auctionContext;

    private readonly IMapper _mapper;

    public AuctionItemService(IAuctionItemRepository auctionRepository, IUserRepository userRepository, IBidItemRepository bidItemRepository, IMapper mapper,
    IHttpContextAccessor httpContextAccessor, AuctionContext auctionContext)
    {
        _auctionRespository = auctionRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _bidItemRepository = bidItemRepository;
        _auctionContext = auctionContext;

    }

    public async Task<AuctionItemResponseDto> AddAuctionItemAsync(AuctionItemAddDto auctionDto)
    {

    var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId");
    if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var sellerId))
    {
        throw new InvalidDataException("You are not authorized to add an auction.");
    }

        auctionDto.SellerId = sellerId;
        var seller = await _userRepository.Get(auctionDto.SellerId);

        if (seller == null)
        {
            throw new NotFoundException($"Seller Id with {auctionDto.SellerId} is not found.");
        }

        var auctionItem = _mapper.Map<AuctionItem>(auctionDto);
        auctionItem.CreatedAt = DateTime.UtcNow;
        auctionItem.UpdatedAt = DateTime.UtcNow;

        if (auctionItem.FileAttachments == null)
        {
            auctionItem.FileAttachments = new List<FileData>();
        }

        if (auctionDto.FileAttachments != null && auctionDto.FileAttachments.Any())
        {
            foreach (var fileattachment in auctionDto.FileAttachments)
            {
                using var memoryStream = new MemoryStream();
                await fileattachment.CopyToAsync(memoryStream);
                var fileData = new FileData
                {
                    Name = fileattachment.FileName,
                    ContentType = fileattachment.ContentType,
                    Data = memoryStream.ToArray()
                };
                auctionItem.FileAttachments.Add(fileData);
            }
        }

        await _auctionRespository.Add(auctionItem);

        var notification = new Notification
        {
            Message = $"New auction created: {auctionItem.Name}",
            CreatedAt = DateTime.UtcNow
        };

        _auctionContext.Notifications.Add(notification);
        await _auctionContext.SaveChangesAsync();

        var responseDto = _mapper.Map<AuctionItemResponseDto>(auctionItem, opts =>
        {
            opts.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
        return responseDto;

    }

    public async Task<bool> DeleteAuctionItemAsync(Guid id)
    {
        var auctionItem = await _auctionRespository.GetAuctionsWithFile(id);
        if (auctionItem == null)
        {
            throw new NotFoundException("There is no data present in the db with this particular id. Please verify it");
        }

        var bids = await _bidItemRepository.GetBidsByAuctionAsync(id);
        if (bids != null)
        {
            foreach (var bid in bids)
            {
                await _bidItemRepository.Delete(bid.Id);
            }
        }

        await _auctionRespository.Delete(id);
        return true;
    }

    public async Task<IEnumerable<AuctionItemResponseDto>> GetAllAuctionItemAsync()
    {

        var auctionItems = await _auctionRespository.GetAllAuctionsWithFile();
        if (auctionItems == null || !auctionItems.Any())
        {
            throw new NotFoundException("There is no data present in the auctions, please try after a while");
        }
        return _mapper.Map<IEnumerable<AuctionItemResponseDto>>(auctionItems, opt =>
        {
            opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
    }

    public async Task<AuctionItemResponseDto?> GetAuctionItemByIdAsync(Guid id)
    {
        var auctionItem = await _auctionRespository.GetAuctionsWithFile(id);
        if (auctionItem == null)
        {
            throw new NotFoundException($"Auction item with ID {id} is not found");
        }
        return _mapper.Map<AuctionItemResponseDto>(auctionItem, opt =>
        {
            opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
    }


    public async Task<AuctionItemResponseDto?> UpdateAuctionItemAsync(Guid id, AuctionItemAddDto auctionUpdateDto)
    {
        var auctionItem = await _auctionRespository.GetAuctionsWithFile(id);

        if (auctionItem == null)
            throw new NotFoundException($"Auction item with ID {id} not found");

        if (!string.IsNullOrWhiteSpace(auctionUpdateDto.Name))
            auctionItem.Name = auctionUpdateDto.Name;

        if (!string.IsNullOrWhiteSpace(auctionUpdateDto.Description))
            auctionItem.Description = auctionUpdateDto.Description;

        if (auctionUpdateDto.StartingPrice != default(decimal))
            auctionItem.StartingPrice = auctionUpdateDto.StartingPrice;

        if (auctionUpdateDto.ReservePrice != default(decimal))
            auctionItem.ReservePrice = auctionUpdateDto.ReservePrice;

        if (auctionUpdateDto.StartTime != default(DateTime))
            auctionItem.StartTime = auctionUpdateDto.StartTime;

        if (auctionUpdateDto.EndTime != default(DateTime))
            auctionItem.EndTime = auctionUpdateDto.EndTime;

        if (!string.IsNullOrWhiteSpace(auctionUpdateDto.Status))
        {
            if (Enum.TryParse<AuctionStatus>(auctionUpdateDto.Status, out var statusEnum))
            {
                auctionItem.Status = statusEnum;
            }
            else
            {
                throw new ArgumentException($"Invalid status value: {auctionUpdateDto.Status}");
            }
        }

        if (auctionUpdateDto.SellerId != Guid.Empty)
        {
            var validSeller = await _userRepository.Get(auctionUpdateDto.SellerId);
            if (validSeller != null && validSeller.Role == UserRole.Seller)
                auctionItem.SellerId = auctionUpdateDto.SellerId;
            else
                throw new InvalidException("Invalid Seller Id");

        }


        if (auctionUpdateDto.FileAttachments != null && auctionUpdateDto.FileAttachments.Any())
        {
            foreach (var fileattachment in auctionUpdateDto.FileAttachments)
            {
                using var memoryStream = new MemoryStream();
                await fileattachment.CopyToAsync(memoryStream);

                auctionItem.FileAttachments.Add(new FileData
                {
                    Name = fileattachment.FileName,
                    ContentType = fileattachment.ContentType,
                    Data = memoryStream.ToArray()
                });
            }
        }

        auctionItem.UpdatedAt = DateTime.UtcNow;

        await _auctionRespository.Update(id, auctionItem);

        return _mapper.Map<AuctionItemResponseDto>(auctionItem, opt =>
        {
            opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
    }





    public async Task<PaginatedResponseDto<AuctionItemResponseDto>> GetPagedAuctionItemsAsync(PaginationDto pagination)
    {
        var query = _auctionRespository.GetAllQueryable();

        if (pagination.StartTime.HasValue)
        {
            var startDate = pagination.StartTime.Value.Date;
            query = query.Where(a => a.StartTime >= startDate);
        }

        if (pagination.EndTime.HasValue)
        {
            var endDate = pagination.EndTime.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(a => a.EndTime <= endDate);
        }

        int totalRecords = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalRecords / (double)pagination.PageSize);

        var pagedItems = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        var resultDto = _mapper.Map<IEnumerable<AuctionItemResponseDto>>(
            pagedItems,
            opt => opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!
        );

        return new PaginatedResponseDto<AuctionItemResponseDto>
        {
            Data = resultDto,
            Pagination = new PaginationMetaData
            {
                TotalRecords = totalRecords,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = totalPages
            }
        };
    }

    public async Task<WinnerIdResponseDto> UpdateWinningId(WinningIdUpdateDto winningIdUpdateDto)
    {
        if (winningIdUpdateDto == null)
            throw new ArgumentException("Invalid winning ID update data");

        var bid = await _bidItemRepository.GetByIdAsync(winningIdUpdateDto.WinningId);

        if (bid == null)
            throw new NotFoundException("Bid not found for the provided WinningId.");

        var user = await _userRepository.Get(bid.BidderId);
        if (user == null || user.Role != UserRole.Bidder)
            throw new InvalidException("Invalid bidder for the winning bid.");

        return await _auctionRespository.UpdateWinningId(winningIdUpdateDto);
    }

    public async Task<AuctionItemResponseDto> UpdateAuctionStatusAsync(Guid auctionItemId, AuctionStatus newStatus)
    {
        var auctionItem = await _auctionRespository.GetAuctionsWithFile(auctionItemId);
        if (auctionItem == null)
            throw new NotFoundException($"Auction item with ID {auctionItemId} not found");

        if (!Enum.IsDefined(typeof(AuctionStatus), newStatus))
            throw new ArgumentException($"Invalid status value: {newStatus}");

        if (newStatus != AuctionStatus.Completed &&
            newStatus != AuctionStatus.Cancelled &&
            newStatus != AuctionStatus.Live)
        {
            throw new InvalidException("Cannot change status of a completed or cancelled auction.");
        }

        if ((auctionItem.Status == AuctionStatus.Completed && newStatus != AuctionStatus.Completed) ||
        (auctionItem.Status == AuctionStatus.Cancelled && newStatus != AuctionStatus.Cancelled))            throw new InvalidException("Cannot change status of a completed auction.");

        auctionItem.Status = newStatus;
        auctionItem.UpdatedAt = DateTime.UtcNow;
        await _auctionRespository.Update(auctionItemId, auctionItem);

        return _mapper.Map<AuctionItemResponseDto>(auctionItem, opt =>
        {
            opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
    }

    public async Task<IEnumerable<AuctionItemResponseDto>> GetActiveAuctionsAsync()
    {
        var activeAuctions = await _auctionRespository.GetAllQueryable()
            .Where(a => a.Status == AuctionStatus.Live)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AuctionItemResponseDto>>(activeAuctions, opt =>
        {
            opt.Items["HttpContext"] = _httpContextAccessor.HttpContext!;
        });
    }

    public async Task<WinnerIdResponseDto?> GetAuctionWinnerAsync(Guid auctionItemId)
    {
        var auctionItem = await _auctionRespository.GetAuctionsWithFile(auctionItemId);
        if (auctionItem == null)
            throw new NotFoundException($"Auction item with ID {auctionItemId} not found");

        if (auctionItem.WinnerId == null)
            throw new NullValueException("This auction has not declared a winner yet.");

        var bid = await _bidItemRepository.GetByIdAsync(auctionItem.WinnerId.Value);
        if (bid == null)
            throw new NotFoundException("No winning bid found for this auction item.");

        var user = await _userRepository.Get(bid.BidderId);
        if (user == null)
            throw new NotFoundException("No user found for the winning bid.");

        return new WinnerIdResponseDto
        {
            WinnerId = user.Id,
            WinnerName = user.Username,
            WinningPrice = bid.Amount,
            AuctionItemId = auctionItemId
        };
    }

    

}