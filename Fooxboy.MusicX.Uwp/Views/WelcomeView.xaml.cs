using ReactiveUI;
using Fooxboy.MusicX.Uwp.ViewModels;
using System.Reactive.Disposables;

namespace Fooxboy.MusicX.Uwp.Views
{
    public class WelcomeViewBase : ReactiveUserControl<WelcomeViewModel>
    {
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class WelcomeView
    {
        public WelcomeView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, model => model.Loading, view => view.ProgressRing.IsActive).DisposeWith(disposable);
                this.BindCommand(ViewModel, model => model.StartCommand, view => view.StartButton).DisposeWith(disposable);
            });
        }
    }
}
