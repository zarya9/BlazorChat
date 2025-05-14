namespace BlazorAPI.Data.Fluxor.Auth
{
    public record SetJwtTokenAction(string Token, string Role);
    public record ClearJwtTokenAction;
}