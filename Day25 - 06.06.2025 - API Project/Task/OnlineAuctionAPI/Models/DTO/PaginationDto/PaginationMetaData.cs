namespace OnlineAuctionAPI.Models.DTO;

public class PaginationMetaData
{
    public int TotalRecords { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}