using Microsoft.AspNetCore.SignalR;
namespace OnlineAuctionAPI.Hubs;

public class AuctionHub : Hub
{
    public async Task SendBidUpdate(Guid auctionItemId, decimal bidAmount, string bidderId)
    {
        await Clients.All.SendAsync("ReceiveBidUpdate", auctionItemId, bidAmount, bidderId);
    }
}
