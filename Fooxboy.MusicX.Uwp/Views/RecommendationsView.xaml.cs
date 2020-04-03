using Fooxboy.MusicX.Uwp.ViewModels;
using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.Views
{
	public class RecommendationsViewBase : ReactiveUserControl<RecommendationsViewModel>
	{
	}

	public sealed partial class RecommendationsView
	{
		public RecommendationsView()
		{
			InitializeComponent();
		}
	}
}