using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.ViewModels.VKontakte;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.ApplicationModel;
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

namespace Fooxboy.MusicX.Uwp.Views.VKontakte
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
            ViewModel = HomeViewModel.Instanse;
            this.Loaded += HomeView_Loaded;
            Application.Current.Resuming += AppResuming;
            Application.Current.Suspending += AppSuspending;
        }


        public HomeViewModel ViewModel { get; set; }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetDescendants(MusicListView).OfType<ScrollViewer>().FirstOrDefault();

            timer.Tick += async (ss, eee) =>
            {
                if (scrollViewer.VerticalOffset < 20)
                {
                    if (PlaylistsGrid.Visibility != Visibility.Visible)
                    {
                        PlaylistsGrid.Visibility = Visibility.Visible;
                        await PlaylistsGrid.Fade(value: 1f, duration: 200, delay: 0).StartAsync();
                    }
                }
                else
                {
                    if (PlaylistsGrid.Visibility != Visibility.Collapsed)
                    {
                        await PlaylistsGrid.Fade(value: 0.0f, duration: 200, delay: 0).StartAsync();
                        PlaylistsGrid.Visibility = Visibility.Collapsed;
                    }
                }
            };

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();
        }



        public static IEnumerable<DependencyObject> GetDescendants(DependencyObject start)
        {
            var queue = new Queue<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(start);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(start, i);
                yield return child;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }

        //protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        //{
        //    timer.Stop();
        //}

        DispatcherTimer timer = new DispatcherTimer();


        private void AppResuming(object sender, object e)
        {
            timer.Start();
        }

        private void AppSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            timer.Stop();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StaticContent.NavigationContentService.Go(typeof(Views.AllPlaylistsView));
        }

    }
}
