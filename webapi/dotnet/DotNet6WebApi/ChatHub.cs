using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        this.Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);
        this.Clients.Group(Context.ConnectionId).SendAsync("Connected", Context.ConnectionId);
        return base.OnConnectedAsync();
    }
    public Task SendMessage(string user, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}