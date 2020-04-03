using Fooxboy.MusicX.Uwp.ViewModels;
using ReactiveUI;

namespace Fooxboy.MusicX.Uwp.Views
{
	public class DownloadsViewBase : ReactiveUserControl<DownloadsViewModel>
	{
	}

    public sealed partial class DownloadsView
    {
        public DownloadsView()
        {
            InitializeComponent();
        }
    }
}
