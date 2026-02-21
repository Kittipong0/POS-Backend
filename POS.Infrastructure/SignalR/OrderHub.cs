using Microsoft.AspNetCore.SignalR;

namespace POS.Infrastructure.SignalR;

public class OrderHub : Hub
{
    public async Task UpdateOrderStatus(int orderId, string status)
    {
        await Clients.All.SendAsync("ReceiveOrderStatusUpdate", orderId, status);
    }

    public async Task NotifyKitchen(int orderId)
    {
        await Clients.All.SendAsync("NewOrderAlert", orderId);
    }
}
