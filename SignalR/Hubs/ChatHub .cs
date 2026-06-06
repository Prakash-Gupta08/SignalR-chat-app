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

        public async Task RegisterUser(
        string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return;

            var existingUser =
                UserTracker.Users
                    .FirstOrDefault(x =>
                        x.ConnectionId ==
                        Context.ConnectionId);

            if (existingUser == null)
            {
                UserTracker.Users.Add(
                    new UserConnection
                    {
                        UserName = userName,
                        ConnectionId =
                            Context.ConnectionId
                    });
            }

            await Clients.All.SendAsync(
                "UsersUpdated",
                UserTracker.Users);
        }


        public async Task SendPrivateMessage(
        string targetConnectionId,
        string senderName,
        string message)
        {
            await Clients.Client(targetConnectionId)
                .SendAsync(
                    "ReceivePrivateMessage",
                    senderName,
                    message,
                    DateTime.Now);
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

        //public override async Task OnConnectedAsync()
        //{
        //    UserTracker.Connections.Add(Context.ConnectionId);

        //    await Clients.Caller.SendAsync(
        //        "ReceiveConnectionId",
        //        Context.ConnectionId);

        //    await Clients.All.SendAsync(
        //        "OnlineUsersCount",
        //        UserTracker.Connections.Count);

        //    Console.WriteLine(
        //        $"Connected : {Context.ConnectionId}");

        //    await base.OnConnectedAsync();
        //}
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync(
                "ReceiveConnectionId",
                Context.ConnectionId);

            Console.WriteLine(
                $"Connected : {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }
        //-----------------------------------------------------------------------------------------
        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    Console.WriteLine($"Disconnected : {Context.ConnectionId}");
        //    await base.OnDisconnectedAsync(exception);
        //}

        //    public override async Task OnDisconnectedAsync(
        //Exception? exception)
        //    {
        //        UserTracker.Connections
        //            .Remove(Context.ConnectionId);

        //        await Clients.All.SendAsync(
        //            "OnlineUsersCount",
        //            UserTracker.Connections.Count);

        //        Console.WriteLine(
        //            $"Disconnected : {Context.ConnectionId}");

        //        await base.OnDisconnectedAsync(exception);
        //    }

        public override async Task OnDisconnectedAsync(
    Exception? exception)
        {
            var user =
                UserTracker.Users
                    .FirstOrDefault(x =>
                        x.ConnectionId ==
                        Context.ConnectionId);

            if (user != null)
            {
                UserTracker.Users.Remove(user);
            }

            await Clients.All.SendAsync(
                "UsersUpdated",
                UserTracker.Users);

            Console.WriteLine(
                $"Disconnected : {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
