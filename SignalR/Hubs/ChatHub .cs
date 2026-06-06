using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {

        public async Task SendMessage(
            string userName,
            string message)
        {
            await Clients.All.SendAsync(
                "ReceiveMessage",
                userName,
                message,
                DateTime.Now);
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(
                $"Connected : {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            Console.WriteLine(
                $"Disconnected : {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
