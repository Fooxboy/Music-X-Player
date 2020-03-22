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
        public RootWindow()
        {
            _hasOpenPlayer = false;
            this.InitializeComponent();
            var navigationService = new NavigationService();
            Container.Get.RegisterInstance<NavigationService>(navigationService);

            PlayerViewModel = new PlayerViewModel();
            PlayerViewModel.CloseBigPlayer = new Action(CloseBigPlayer);
            NavigationViewModel = new NavigationRootViewModel();
            navigationService.RootFrame = this.Root;
            navigationService.Go(typeof(HomeView));

            UserInfoViewModel = new UserInfoViewModel();
            LoadingViewModel = new LoadingViewModel();
            NotificationViewModel = new NotificationViewModel();
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await UserInfoViewModel.StartLoadingUserInfo();
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
    }
}
