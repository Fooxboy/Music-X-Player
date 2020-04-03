using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
	public class DownloadsViewModel : ReactiveObject, IRoutableViewModel
	{
		public string UrlPathSegment { get; }
		public IScreen HostScreen { get; }
	}
}