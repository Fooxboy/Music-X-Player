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
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SearchView : Page
    {
        public SearchViewModel ViewModel { get; set; }

        public SearchView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = (object[])e.Parameter;
            var query = (string) data[0];
            var api = (Api) data[1];
            var notification = (NotificationService) data[2];
            var logger = (LoggerService) data[3];
            ViewModel = new SearchViewModel(api, notification, logger);

            await ViewModel.StartLoading(query);
            base.OnNavigatedTo(e);
        }
    }
}
