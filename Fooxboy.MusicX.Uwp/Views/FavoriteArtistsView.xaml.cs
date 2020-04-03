using Fooxboy.MusicX.Uwp.ViewModels;
using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.Views
{
	public class FavoriteArtistsViewBase : ReactiveUserControl<FavoriteArtistsViewModel>
	{
	}

	public sealed partial class FavoriteArtistsView
	{
		public FavoriteArtistsView()
		{
			InitializeComponent();
		}
	}
}