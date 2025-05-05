namespace BlazorAPI.Data.Fluxor.Auth
{
    public class LoginAction { public string Email; public string Password; }
    public class LoginSuccessAction { public string Token; public string Role; }
    public class LoginFailedAction { public string Error; }

    public class LogoutAction;
}
