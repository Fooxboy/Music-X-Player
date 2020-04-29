using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VkNet.Model.GroupUpdate;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Core;
using Windows.System;
using Fooxboy.MusicX.Core;


// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class RootWindow : Page
    {
        public PlayerViewModel PlayerViewModel { get; set; }
        public NavigationRootViewModel NavigationViewModel { get; set; }
        public UserInfoViewModel UserInfoViewModel { get; set; }
        public LoadingViewModel LoadingViewModel { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }


        private bool _isPlayerOpen;
        private bool _hasOpenPlayer;
        private IContainer _container;
        public RootWindow()
        {

            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _container = (IContainer) e.Parameter;
            Container.SetContainer(this._container);

            _container.Register<NavigationService>(Reuse.Singleton);
            var navigationService = _container.Resolve<NavigationService>();

            PlayerViewModel = new PlayerViewModel(_container);
            PlayerViewModel.CloseBigPlayer = new Action(CloseBigPlayer);
            NavigationViewModel = new NavigationRootViewModel(_container);
            navigationService.RootFrame = this.Root;
            navigationService.Go(typeof(HomeView), _container);

            UserInfoViewModel = new UserInfoViewModel(_container);
            LoadingViewModel = new LoadingViewModel(_container);
            NotificationViewModel = new NotificationViewModel(_container);

            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await UserInfoViewModel.StartLoadingUserInfo();
            TitleTrack.Text = "Сейчас ничего не воспроизводится";
            NavigationViewModel.VisibilitySelectorHome = true;
            NavigationViewModel.Changed();
            //AppWindow appWindow = await AppWindow.TryCreateAsync();
            //Frame appWindowContentFrame = new Frame();
            //appWindowContentFrame.Navigate(typeof(DeveloperView));
            //ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            //await appWindow.TryShowAsync();
        }

        private void PersonPicture_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutProfile.ShowAt((PersonPicture)sender);
        }

        private void TextBlock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (ArtistText.TextDecorations == Windows.UI.Text.TextDecorations.Underline) return;
            ArtistText.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
        }

        private void ArtistText_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (ArtistText.TextDecorations == Windows.UI.Text.TextDecorations.None) return;
            ArtistText.TextDecorations = Windows.UI.Text.TextDecorations.None;

        }

        private void GridButtom_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            IconBackground.Visibility = Visibility.Visible;
        }

        private void GridButtom_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            IconBackground.Visibility = Visibility.Collapsed;
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _isPlayerOpen = true;
            GridButtom.Height = 660;
            ShadowCover.Visibility = Visibility.Collapsed;
            GridImage.Visibility = Visibility.Collapsed;
            TextGrid.Visibility = Visibility.Collapsed;
            GridButtons.Visibility = Visibility.Collapsed;
            GridTimer.Visibility = Visibility.Collapsed;
            StackButtons.Visibility = Visibility.Collapsed;
            Shadoww.Visibility = Visibility.Collapsed;
            await Animations.BeginAsync();
            //RectangleBackground.Height = +500;
            BigPlayerFrame.Visibility = Visibility.Visible;
            BigPlayerFrame.Navigate(typeof(PlayerView), PlayerViewModel, new DrillInNavigationTransitionInfo());

        }

        private async void CloseBigPlayer()
        {

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                BigPlayerFrame.Visibility = Visibility.Collapsed;
                _isPlayerOpen = false;
                GridButtom.Height = 60;
                ShadowCover.Visibility = Visibility.Visible;
                GridImage.Visibility = Visibility.Visible;
                TextGrid.Visibility = Visibility.Visible;
                GridButtons.Visibility = Visibility.Visible;
                GridTimer.Visibility = Visibility.Visible;
                StackButtons.Visibility = Visibility.Visible;
                Shadoww.Visibility = Visibility.Visible;
                AnimationsClose.Begin();
            });
            
           
        }


        private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.OriginalKey == VirtualKey.Enter)
            {
                var n = _container.Resolve<NavigationService>();
                var api = _container.Resolve<Api>();
                var notification = _container.Resolve<NotificationService>();
                n.Go(typeof(SearchView), new object[]{SearchBox.Text, api, notification}, 1);
            }
        }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.PlaceholderText = "Нажмите enter для поиска";
        }

        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.PlaceholderText = "Найдите что нибудь...";

        }

       
    }
}
