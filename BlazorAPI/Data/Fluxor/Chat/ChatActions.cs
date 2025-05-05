using BlazorAPI.ApiRequest.Model;

namespace BlazorAPI.Data.Fluxor.Chat
{
    public class LoadMessagesAction;
    public class LoadMessagesSuccessAction { public List<ChatMessage> Messages; }
    public class LoadMessagesFailedAction { public string Error { get; set; } }

    public class SendMessageAction { public string Text; }
    public class UpdateCurrentMessageAction { public string Text; }
}
