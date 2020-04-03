using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
	public class FavoriteArtistsViewModel : ReactiveObject, IRoutableViewModel
	{
		public string UrlPathSegment { get; }
		public IScreen HostScreen { get; }
	}
}