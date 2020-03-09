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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        public RootWindow()
        {
            this.InitializeComponent();
            var navigationService = new NavigationService();
            Container.Get.RegisterInstance<NavigationService>(navigationService);

            PlayerViewModel = new PlayerViewModel();
            NavigationViewModel = new NavigationRootViewModel();
            navigationService.RootFrame = this.Root;
            navigationService.Go(typeof(HomeView));

            UserInfoViewModel = new UserInfoViewModel();
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await UserInfoViewModel.StartLoadingUserInfo();
        }

        //private void RectangleBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    BorderRectangleBackground.Width = e.NewSize.Width;
        //    BorderRectangleBackground.Height = e.NewSize.Height;
        //}


    }
}
