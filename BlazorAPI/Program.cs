using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorAPI.ApiRequest;
using Microsoft.AspNetCore.Authentication.Cookies; 
using BlazorAPI.Hubs;
using Fluxor;
using BlazorAPI;
using Fluxor.Blazor.Web.ReduxDevTools;


var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => options.DetailedErrors = true);

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    options.UseReduxDevTools();
});

// Другие сервисы...
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<MovieService>();

// Настройка HttpClient
builder.Services.AddHttpClient("UnauthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5256");
});

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5256");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddTransient<AuthorizationMessageHandler>();

// Настройка CORS
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

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<ChatHub>("/chatHub");
app.Run();