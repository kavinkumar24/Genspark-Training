namespace OnlineAuctionAPI.Models.DTO;

public class PaginationDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? SortBy { get; set; }   
    public string? SortDirection { get; set; } = "asc"; 
}