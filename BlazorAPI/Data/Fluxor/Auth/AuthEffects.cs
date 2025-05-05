using Blazored.LocalStorage;
using Fluxor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAPI.Data.Fluxor.Auth
{
    public class AuthEffects
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public AuthEffects(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        [EffectMethod]
        public async Task HandleLogin(LoginAction action, IDispatcher dispatcher)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/UserLogin/Login", new
                {
                    action.Email,
                    action.Password
                });

                if (!response.IsSuccessStatusCode)
                {
                    dispatcher.Dispatch(new LoginFailedAction { Error = "Ошибка входа" });
                    return;
                }

                var token = await response.Content.ReadAsStringAsync();
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                await _localStorage.SetItemAsync("authToken", token);
                dispatcher.Dispatch(new LoginSuccessAction { Token = token, Role = role });
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoginFailedAction { Error = ex.Message });
            }
        }
    }
}
