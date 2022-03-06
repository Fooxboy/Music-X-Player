using Fooxboy.MusicX.Core.Models;
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

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Controls
{
    public sealed partial class SectionBlockControl : UserControl
    {
        public SectionBlockControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Block),
          typeof(BlockControl), new PropertyMetadata(new Block()));

        public Block Block
        {
            get => (Block)GetValue(BlockProperty);
            set
            {
                SetValue(BlockProperty, value);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Block.DataType == "music_audios")
            {
                AudiosGrid.Visibility = Visibility.Visible;
                this.ListTracks.ItemsSource = Block.Audios;

            }
        }
    }
}
