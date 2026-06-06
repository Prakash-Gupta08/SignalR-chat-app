using Microsoft.AspNetCore.SignalR;
using SignalR.Data;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {

        public async Task SendMessage(string userName,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",userName,message,DateTime.Now);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    Console.WriteLine($"Connected : {Context.ConnectionId}");
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.Caller.SendAsync("ReceiveConnectionId",Context.ConnectionId);
        //    Console.WriteLine($"Connected : {Context.ConnectionId}");
        //    await base.OnConnectedAsync();
        //}

        public override async Task OnConnectedAsync()
        {
            UserTracker.Connections.Add(Context.ConnectionId);

            await Clients.Caller.SendAsync(
                "ReceiveConnectionId",
                Context.ConnectionId);

            await Clients.All.SendAsync(
                "OnlineUsersCount",
                UserTracker.Connections.Count);

            Console.WriteLine(
                $"Connected : {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    Console.WriteLine($"Disconnected : {Context.ConnectionId}");
        //    await base.OnDisconnectedAsync(exception);
        //}

        public override async Task OnDisconnectedAsync(
    Exception? exception)
        {
            UserTracker.Connections
                .Remove(Context.ConnectionId);

            await Clients.All.SendAsync(
                "OnlineUsersCount",
                UserTracker.Connections.Count);

            Console.WriteLine(
                $"Disconnected : {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
