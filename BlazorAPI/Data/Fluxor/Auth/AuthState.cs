namespace BlazorAPI.Data.Fluxor.Auth
{
    public record AuthState
    {
        public bool IsLoading { get; init; }
        public bool IsAuthenticated { get; init; }
        public string Token { get; init; }
        public string Error { get; init; }
        public string Role { get; init; }

        public static AuthState Empty => new()
        {
            IsLoading = false,
            IsAuthenticated = false,
            Token = null,
            Error = null,
            Role = null
        };
    }
}
