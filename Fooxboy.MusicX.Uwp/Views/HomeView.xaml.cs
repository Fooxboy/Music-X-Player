using Fooxboy.MusicX.Uwp.ViewModels;
using ReactiveUI;
using Windows.UI.Xaml;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    public class HomeViewBase : ReactiveUserControl<HomeViewModel>
    {
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void BlockPlaylists_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderShadow.Width = e.NewSize.Width;
        }

        //private async void scroll_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        //{
        //    var current = scroll.VerticalOffset;
        //    var max = scroll.ScrollableHeight;

        //    //Долистали до конца, загружаем еще треков.
        //    if(max - current < 80)
        //    {
        //        await ViewModel.StartLoadingTracks();
        //    }

        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    _container = (IContainer) e.Parameter;
        //    ViewModel = new HomeViewModel(_container);


        //    base.OnNavigatedTo(e);
        //}

        //private async void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    await ViewModel.GetMaxTracks();
        //    await ViewModel.StartLoadingAlbums();
        //    await ViewModel.StartLoadingTracks();
        //}

        //private async void TracksListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var track = (Track)e.ClickedItem;
        //     ViewModel.PlayTrack(track);
        //}
    }
}