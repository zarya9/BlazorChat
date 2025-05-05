using BlazorAPI.Data.Utilities;
using Blazored.LocalStorage;
using Fluxor;

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

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    if (!JwtParser.IsTokenValid(token))
                    {
                        dispatcher.Dispatch(new LoginFailedAction { Error = "Токен невалиден" });
                        return;
                    }

                    var role = JwtParser.GetRoleFromToken(token);
                    if (string.IsNullOrEmpty(role))
                    {
                        dispatcher.Dispatch(new LoginFailedAction { Error = "Роль не найдена в токене" });
                        return;
                    }

                    await _localStorage.SetItemAsync("authToken", token);

                    dispatcher.Dispatch(new LoginSuccessAction
                    {
                        Token = token,
                        Role = role
                    });
                }
                else
                {
                    dispatcher.Dispatch(new LoginFailedAction
                    {
                        Error = await response.Content.ReadAsStringAsync() ?? "Неверный email или пароль"
                    });
                }
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoginFailedAction
                {
                    Error = $"Ошибка сервера: {ex.Message}"
                });
            }
        }
    }
}
