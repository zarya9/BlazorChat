using BlazorAPI.ApiRequest.Model;
using Fluxor;

namespace BlazorAPI.Data.Fluxor.Auth
{
    [Serializable]
    public record AuthState
    {
        public string JwtToken { get; init; }
        public bool IsAuthenticated { get; }
        public string Role { get; init; } 

        public static AuthState Empty => new AuthState();
        public AuthState(string jwtToken, bool isAuthenticated, string role = null)
        {
            JwtToken = jwtToken;
            IsAuthenticated = isAuthenticated;
            Role = role;
        }

        public AuthState() : this(null, false) { }
    }
}