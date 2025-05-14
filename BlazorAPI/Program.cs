using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorAPI.ApiRequest;
using BlazorAPI.Hubs;
using Fluxor;
using BlazorAPI;
using Fluxor.Blazor.Web.ReduxDevTools;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents(); builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    options.UseReduxDevTools();
});
builder.Services.AddFluxorConfig();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<MovieService>();

builder.Services.AddHttpClient("UnauthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5256");
});

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5256");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new Uri("http://localhost:5256")
//});

builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddRazorPages();
var app = builder.Build();

app.UseHttpsRedirection(); 
app.UseStaticFiles(); 
if (!app.Environment.IsDevelopment()) 
{ 
    app.UseExceptionHandler("/Error"); 
    app.UseHsts(); 
}
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapFallbackToPage("/_Host");
});
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();
app.Run();
