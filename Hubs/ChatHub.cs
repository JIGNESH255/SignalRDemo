using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var userName = httpContext?.Session.GetString("UserName") ?? Context.ConnectionId;
        UserConnections[Context.ConnectionId] = userName;

        await Clients.All.SendAsync("UserConnected", userName);
        await UpdateUserList();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        UserConnections.TryRemove(Context.ConnectionId, out var userName);

        await Clients.All.SendAsync("UserDisconnected", userName);
        await UpdateUserList();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        bool isOnline = UserConnections.Values.Contains(user);
        await Clients.All.SendAsync("ReceiveMessage", user, message, isOnline);
    }

    private async Task UpdateUserList()
    {
        var users = UserConnections.Values.Distinct().ToList();
        await Clients.All.SendAsync("UpdateUserList", users);
    }
}
