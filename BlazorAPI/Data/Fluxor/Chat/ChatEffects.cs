using BlazorAPI.ApiRequest.Model;
using Fluxor;

namespace BlazorAPI.Data.Fluxor.Chat
{
    public class ChatEffects
    {
        private readonly HttpClient _http;

        public ChatEffects(HttpClient http) => _http = http;

        [EffectMethod]
        public async Task HandleLoadMessages(LoadMessagesAction action, IDispatcher dispatcher)
        {
            var messages = await _http.GetFromJsonAsync<List<ChatMessage>>("/api/Chat");
            dispatcher.Dispatch(new LoadMessagesSuccessAction { Messages = messages });
        }
    }
}
