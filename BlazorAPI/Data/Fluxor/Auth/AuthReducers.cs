using Fluxor;

namespace BlazorAPI.Data.Fluxor.Auth
{
    public class AuthReducers
    {
        [ReducerMethod]
        public static AuthState OnLogin(AuthState state, LoginAction action)
    => state with { IsLoading = true };

        [ReducerMethod]
        public static AuthState OnLoginSuccess(AuthState state, LoginSuccessAction action)
            => state with
            {
                IsLoading = false,
                IsAuthenticated = true,
                Token = action.Token,
                Role = action.Role
            };

        [ReducerMethod]
        public static AuthState OnLoginFailed(AuthState state, LoginFailedAction action)
            => state with
            {
                IsLoading = false,
                Error = action.Error
            };
    }
}
