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

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Resources.ContentDialogs
{
    public sealed partial class ExceptionDialog : ContentDialog
    {
        public ExceptionDialog(string title, string info, Exception e)
        {
            this.InitializeComponent();
            TitleText = title;
            Info = info;
            ExceptionString = e.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public string TitleText { get; set; }
        public string Info { get; set; }
        public string ExceptionString { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(stackText.Visibility == Visibility.Collapsed)
            {
                showButton.Content = "Скрыть подробности";
                stackText.Visibility = Visibility.Visible;
            }else
            {
                showButton.Content = "Показать подробности";
                stackText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
