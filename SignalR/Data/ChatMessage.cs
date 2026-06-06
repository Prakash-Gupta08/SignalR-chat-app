namespace SignalR.Data
{
    public class ChatMessage
    {
        public string UserName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }
    }
}
