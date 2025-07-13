using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
namespace Restaurant_Management_System.Hubs;

    public class NotificationHub: Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var user = httpContext?.User;

        if (user != null)
        {
            if (user.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            else if (user.IsInRole("Customer"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");
            }
        }

        await base.OnConnectedAsync();
    }
}

