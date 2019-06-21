using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Services;
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
    public sealed partial class MainFrameView : Page
    {
        public MainFrameView()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(TitleBar);
            StaticContent.PlayerMenuFrame = PlayerMenuFrame;
            StaticContent.NavigationContentService = new Services.NavigationService() { RootFrame = ContentFrame };
            PlayerMenuFrame.Navigate(typeof(PlayerMenuView));

            if (StaticContent.IsAuth)
            {
                if (InternetService.CheckConnection())
                {
                    StaticContent.NavigationContentService.Go(typeof(VKontakte.HomeView));
                }else
                {
                    StaticContent.NavigationContentService.Go(typeof(HomeLocalView));
                }
            }else
            {
                StaticContent.NavigationContentService.Go(typeof(HomeLocalView));
            }
        }
    }
}
