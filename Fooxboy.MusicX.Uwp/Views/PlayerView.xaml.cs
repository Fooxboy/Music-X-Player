using DryIoc;
using Fooxboy.MusicX.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class PlayerView : Page
    {
        public PlayerViewModel PlayerViewModel { get; set; }
        public PlayerView()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (PlayerViewModel)e.Parameter;
            PlayerViewModel = param;
            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerViewModel.CloseBigPlayer?.Invoke();
        }

        private void GridPlayerActions_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ClosePlayerGrid.Visibility = Visibility.Visible;
        }

        private void GridPlayerActions_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ClosePlayerGrid.Visibility = Visibility.Collapsed;

        }

        private void CoverGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayerViewModel.CloseBigPlayer?.Invoke();
        }
    }
}
