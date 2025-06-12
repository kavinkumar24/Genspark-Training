
using AutoMapper;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Mapping;

public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<BidItemAddDto, BidItem>()
            .ForMember(dest => dest.Bidder, opt => opt.Ignore())
            .ForMember(dest => dest.AuctionItem, opt => opt.Ignore());

        CreateMap<BidItem, BidItemAddDto>();

        CreateMap<BidItem, BidItemResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BidderId));

        CreateMap<BidItemAddDto, BidItem>();
    }
}