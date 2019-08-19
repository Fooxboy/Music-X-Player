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
using Fooxboy.MusicX.Uwp.Models;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class BlockControl : UserControl
    {

        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Block), typeof(BlockControl), new PropertyMetadata(new Block()
        {
            BlockId = "", CountElements = 0, Description = "", PlaylistsFiles = null, TrackFiles =  null, Playlists = null, Tracks = null, Title = "", TypeBlock = "default"
        }));

        public Block Block
        {
            get => (Block)GetValue(BlockProperty);
            set
            {
                if (value.Tracks != null) IsTracks = Visibility.Visible;
                if (value.Playlists != null) IsPlaylists = Visibility.Visible;
                SetValue(BlockProperty, value);
            }
        }

        public BlockControl()
        {
            this.InitializeComponent();
        }

        public Visibility IsTracks { get; set; } = Visibility.Collapsed;
        public Visibility IsPlaylists { get; set; } = Visibility.Collapsed;
    }
}
