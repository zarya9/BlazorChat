using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;

namespace BlazorAPI
{
    public static class FluxorExtensions
    {
        public static IServiceCollection AddFluxorConfig(this IServiceCollection services)
        {
            services.AddFluxor(options =>
            {
                options.ScanAssemblies(typeof(Program).Assembly);
                options.UseReduxDevTools();
            });
            return services;
        }
    }
}
