namespace Notify.Hub;
using Microsoft.AspNetCore.SignalR;

public class FileUploadNotification : Hub
{
    public async Task SendMessage(string user, string message, string time)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message, time);
    }
        
}