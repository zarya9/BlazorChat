using BlazorAPI.ApiRequest.Model;

namespace BlazorAPI.Data.Fluxor.Profile
{
    public record ProfileState
    {
        public Logins Profile { get; init; }
        public bool IsLoading { get; init; }

        public static ProfileState Empty => new()
        {
            Profile = null,
            IsLoading = false
        };
    }
}
