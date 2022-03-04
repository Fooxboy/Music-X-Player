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
using Flurl.Http;
using Fooxboy.MusicX.Core;
using VkNet.AudioBypassService.Models;


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
            _container = (IContainer)e.Parameter;
           var navigationService = _container.Resolve<NavigationService>();


            PlayerViewModel = new PlayerViewModel(_container);
            PlayerViewModel.CloseBigPlayer = new Action(CloseBigPlayer);
            NavigationViewModel = new NavigationRootViewModel(_container);
            navigationService.RootFrame = this.Root;
            navigationService.Go(typeof(HomeView), _container);

            UserInfoViewModel = new UserInfoViewModel(_container);
            LoadingViewModel = new LoadingViewModel(_container);
            NotificationViewModel = new NotificationViewModel(_container);

            Window.Current.SetTitleBar(this.TitleBar);


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

            var theme = Application.Current.RequestedTheme;

            if(theme == ApplicationTheme.Light)
            {
                audioblack.Visibility = Visibility.Visible;
                audiowhite.Visibility = Visibility.Collapsed;


                nextblack.Visibility = Visibility.Visible;
                nextwhite.Visibility = Visibility.Collapsed;

                prevblack.Visibility = Visibility.Visible;
                prevwhite.Visibility = Visibility.Collapsed;

                playblack.Visibility = Visibility.Visible;
                playwhite.Visibility = Visibility.Collapsed;

                pauseblack.Visibility = Visibility.Visible;
                pausewhite.Visibility = Visibility.Collapsed;

            }
            else
            {
                audioblack.Visibility = Visibility.Collapsed;
                audiowhite.Visibility = Visibility.Visible;

                nextblack.Visibility = Visibility.Collapsed;
                nextwhite.Visibility = Visibility.Visible;

                prevblack.Visibility = Visibility.Collapsed;
                prevwhite.Visibility = Visibility.Visible;

                playblack.Visibility = Visibility.Collapsed;
                playwhite.Visibility = Visibility.Visible;

                pauseblack.Visibility = Visibility.Collapsed;
                pausewhite.Visibility = Visibility.Visible;
            } 

            var configService = _container.Resolve<ConfigService>();
            var config = await configService.GetConfig().ConfigureAwait(false);

            PlayerViewModel.Volume = config.Volume;

            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                d.Value = config.Volume;

            });

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
                var logger = _container.Resolve<LoggerService>();
                var notification = _container.Resolve<NotificationService>();
                n.Go(typeof(SearchView), new object[]{SearchBox.Text, api, notification, logger}, 1);
            }
        }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.PlaceholderText = "Нажмите enter для поиска";
        }

        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.PlaceholderText = "Найдите что-нибудь...";

        }

        private async void ArtistText_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var api = _container.Resolve<Api>();
                var logger = _container.Resolve<LoggerService>();
                var notificationService = _container.Resolve<NotificationService>();
                var navigation = _container.Resolve<NavigationService>();
                var player = _container.Resolve<PlayerService>();
                if (PlayerViewModel.PlayerSerivce.CurrentTrack.Artists != null)
                {
                    if (PlayerViewModel.PlayerSerivce.CurrentTrack.Artists.Count > 0)
                    {
                        try
                        {
                            var artist = PlayerViewModel.PlayerSerivce.CurrentTrack.Artists[0];
                            navigation.Go(typeof(ArtistView), new object[] { api, notificationService, artist.Id, player, logger }, 1);
                            return;
                        }
                        catch (Exception ee)
                        {
                            notificationService.CreateNotification("Ошибка при открытии карточки музыканта", $"Ошибка: {ee.Message}");
                            return;
                        }
                    }
                }

                navigation.Go(typeof(SearchView), new object[] { PlayerViewModel.Artist, api, notificationService, logger }, 1);
            }catch(Exception ex)
            {

                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var dialog = new ContentDialog();
                    dialog.Title = "Произошла неизвестная ошибка";
                    dialog.Content = ex;
                    await dialog.ShowAsync();
                });

            }

        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void d_PointerReleased(object sender, PointerRoutedEventArgs e)
        {


        }

        private async void Flyout_Closed(object sender, object e)
        {
            var configService = _container.Resolve<ConfigService>();
            var config = await configService.GetConfig().ConfigureAwait(false);

            if (config.Volume == PlayerViewModel.Volume) return;
            config.Volume = PlayerViewModel.Volume;

            await configService.SetConfig(config);

        }
    }
}
