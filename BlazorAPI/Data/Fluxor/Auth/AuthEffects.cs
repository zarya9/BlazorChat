using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static BlazorAPI.Pages.LoginPage;

namespace BlazorAPI.Data.Fluxor.Auth
{
    public class AuthEffects
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;
        private readonly AuthenticationStateProvider _authProvider;

        public AuthEffects(
            HttpClient http,
            ILocalStorageService localStorage,
            NavigationManager navigation,
            AuthenticationStateProvider authProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _navigation = navigation;
            _authProvider = authProvider;
        }

        [EffectMethod]
        public async Task HandleLogin(LoginAction action, IDispatcher dispatcher)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/UserLogin/Login", new
                {
                    action.Email,
                    action.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    await (_authProvider as CustomAuthenticationStateProvider).NotifyUserAuthentication(result.Token);

                    dispatcher.Dispatch(new LoginSuccessAction
                    {
                        Token = result.Token,
                        Role = GetRoleFromToken(result.Token)
                    });

                    _navigation.NavigateTo(GetRedirectUrl(result.Token));
                }
                else
                {
                    dispatcher.Dispatch(new LoginFailedAction
                    {
                        Error = await response.Content.ReadAsStringAsync()
                    });
                }
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoginFailedAction
                {
                    Error = ex.Message
                });
            }
        }

        private string GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        }

        private string GetRedirectUrl(string token)
        {
            return GetRoleFromToken(token) == "Admin" ? "/users" : "/profile";
        }
        [EffectMethod]
        public async Task HandleSaveNavigation(SaveNavigationAction action, IDispatcher dispatcher)
        {
            await _localStorage.SetItemAsync("last_url", action.Url);
            dispatcher.Dispatch(new UpdateLastUrlAction { 
                Url = action.Url,
                Timestamp = DateTime.Now
            });
            Console.WriteLine($"Сохранен URL: {action.Url} в {DateTime.Now}");
        }

        [EffectMethod]
        public async Task HandleUpdateLastUrl(UpdateLastUrlAction action, IDispatcher dispatcher)
        {
            Console.WriteLine($"Обновлен URL в состоянии: {action.Url}");
        }

        [EffectMethod]
        public async Task HandleLoadAuthState(LoadAuthStateAction action, IDispatcher dispatcher)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                dispatcher.Dispatch(new LoginSuccessAction
                {
                    Token = token,
                    Role = role
                });
            }
        }
        [EffectMethod]
        public async Task HandleNavigation(NavigationAction action, IDispatcher dispatcher)
        {
            if (!action.Url.Contains("/login") && !action.Url.Contains("/auth"))
            {
                await _localStorage.SetItemAsync("last_url", action.Url);
                Console.WriteLine($"Сохранен URL в LocalStorage: {action.Url}");
            }
        }
    }
}
