using Fluxor;

namespace BlazorAPI.Data.Fluxor.Chat
{
    public class ChatReducers
    {
        [ReducerMethod]
        public static ChatState OnLoadMessages(ChatState state, LoadMessagesAction action)
    => state with { IsLoading = true };

        [ReducerMethod]
        public static ChatState OnLoadMessagesSuccess(
            ChatState state,
            LoadMessagesSuccessAction action
        ) => state with
        {
            IsLoading = false,
            Messages = action.Messages
        };

        [ReducerMethod]
        public static ChatState OnUpdateCurrentMessage(
            ChatState state,
            UpdateCurrentMessageAction action
        ) => state with { CurrentMessage = action.Text };
    }
}
