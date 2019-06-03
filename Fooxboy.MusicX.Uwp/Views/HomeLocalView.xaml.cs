﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
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
    public sealed partial class HomeLocalView : Page
    {
        public HomeLocalView()
        {
            this.InitializeComponent();
            HomeViewModel = HomeLocalViewModel.Instanse;

            
        }

        public HomeLocalViewModel HomeViewModel { get; set; }


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

        protected async override void OnNavigatedTo(NavigationEventArgs ee)
        {
            if (HomeViewModel.Playlists.Count == 0)
            {
                await PlaylistsService.SetPlaylistLocal();
            }

            if (HomeViewModel.Music.Count == 0)
            {
                await MusicFilesService.GetMusicLocal();
                HomeViewModel.CountMusic();
            }

            var scrollViewer = GetDescendants(MusicListView).OfType<ScrollViewer>().FirstOrDefault();


            
            scrollViewer.ViewChanging += (o, e)  =>
            {
                //PlaylistsGrid.Visibility = Visibility.Collapsed;
                if (scrollViewer.VerticalOffset < 20)
                {
                    PlaylistsGrid.Visibility = Visibility.Visible;
                }else
                {
                    PlaylistsGrid.Visibility = Visibility.Collapsed;
                }

            };
        }
    }
}
