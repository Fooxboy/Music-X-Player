using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class RecommendationsViewModel : ReactiveObject, IRoutableViewModel
    {
	    public string UrlPathSegment => "recommendations";

        public IScreen HostScreen { get; }
    }
}
