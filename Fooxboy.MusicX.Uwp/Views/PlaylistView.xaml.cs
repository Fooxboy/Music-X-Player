﻿using System;
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
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PlaylistView : Page
    {
        public PlaylistView()
        {
            this.InitializeComponent();
            ViewModel = new PlaylistViewModel();
        }

        public PlaylistViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var album = (Album) e.Parameter;
            await ViewModel.StartLoading(album);
            base.OnNavigatedTo(e);
        }

        private void Rectangle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShadowRectangle.Width = e.NewSize.Width;
        }

        private void TracksListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var track = (Track) e.ClickedItem;
            ViewModel.PlayTrack(track);
        }
    }
}
