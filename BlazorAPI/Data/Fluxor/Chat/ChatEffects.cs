using BlazorAPI.ApiRequest.Model;
using Blazored.LocalStorage;
using Fluxor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAPI.Data.Fluxor.Chat
{
    public class ChatEffects
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ChatEffects(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        [EffectMethod]
        public async Task HandleLoadMessages(LoadMessagesAction action, IDispatcher dispatcher)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                var userId = GetUserIdFromToken(token);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User ID not found in token");
                }

                var messages = await _http.GetFromJsonAsync<List<ChatMessage>>($"/api/Chat/dialogs/{userId}");
                dispatcher.Dispatch(new LoadMessagesSuccessAction { Messages = messages });
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoadMessagesFailedAction { Error = ex.Message });
                dispatcher.Dispatch(new LoadMessagesFailedAction { Error = ex.Message });
            }
        }

        private string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}