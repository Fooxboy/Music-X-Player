using ReactiveUI;
using System.Reactive.Disposables;
using Fooxboy.MusicX.Uwp.ViewModels;

namespace Fooxboy.MusicX.Uwp.Views
{
    public class BootsrapperViewBase : ReactivePage<AppBootstrapper>
    {
    }
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public partial class BootsrapperView
    {
        public BootsrapperView()
        {
            InitializeComponent();

            ViewModel = new AppBootstrapper();

            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, model => model.Router, window => window.RoutedViewHost.Router).DisposeWith(disposable);
            });
        }
    }
}
