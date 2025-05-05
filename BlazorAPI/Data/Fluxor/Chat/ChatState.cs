using BlazorAPI.ApiRequest.Model;

namespace BlazorAPI.Data.Fluxor.Chat
{
    public record ChatState
    {
        public List<ChatMessage> Messages { get; init; } = new();
        public bool IsLoading { get; init; }
        public string CurrentMessage { get; init; }

        public static ChatState Empty => new()
        {
            Messages = new(),
            IsLoading = false,
            CurrentMessage = string.Empty
        };
    }
}
