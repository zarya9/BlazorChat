using Fluxor;

namespace BlazorAPI.Data
{
    [FeatureState]
    public class AppState
    {
        public string AuthToken { get; set; }
        public string LastVisitedUrl { get; set; }
    }
}
