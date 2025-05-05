using Fluxor;

namespace BlazorAPI.Data.Fluxor.Auth
{
    [Serializable]
    public record AuthState
    {
        public bool IsLoading { get; init; }
        public bool IsAuthenticated { get; init; }
        public string Token { get; init; }
        public string Role { get; init; }
        public string Error { get; init; }
        public string LastVisitedUrl { get; init; }
        public DateTime LastUpdated { get; init; }

        public static AuthState Empty => new()
        {
            IsLoading = false,
            IsAuthenticated = false,
            Token = null,
            Role = null,
            Error = null,
            LastVisitedUrl = "/",
            LastUpdated = DateTime.MinValue
        };
    }
}