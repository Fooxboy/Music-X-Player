using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DryIoc;
using Fooxboy.MusicX.Uwp.Services;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Controls
{
    public sealed partial class NotificationControl : UserControl
    {
        public NotificationControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(NotificationControl), new PropertyMetadata("Title notification"));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(NotificationControl), new PropertyMetadata("Text notification"));

        public static readonly DependencyProperty ButtonOneTextProperty = DependencyProperty.Register("ButtonOneText", typeof(string), typeof(NotificationControl), new PropertyMetadata("Ok"));
        public static readonly DependencyProperty ButtonTwoTextProperty = DependencyProperty.Register("ButtonTwoText", typeof(string), typeof(NotificationControl), new PropertyMetadata("Cancel"));

        public static readonly DependencyProperty ButtonOneCommandProperty = DependencyProperty.Register("ButtonOneCommand", typeof(ICommand), typeof(NotificationControl), new PropertyMetadata(new RelayCommand(()=> { })));
        public static readonly DependencyProperty ButtonTwoCommandProperty = DependencyProperty.Register("ButtonTwoCommand", typeof(ICommand), typeof(NotificationControl), new PropertyMetadata(new RelayCommand(() => { })));

        public static readonly DependencyProperty HasButtonsProperty = DependencyProperty.Register("HasButtons", typeof(bool), typeof(NotificationControl), new PropertyMetadata(false));

        private IContainer _container;

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string ButtonOneText
        {
            get => (string)GetValue(ButtonOneTextProperty);
            set => SetValue(ButtonOneTextProperty, value);
        }

        public string ButtonTwoText
        {
            get => (string)GetValue(ButtonTwoTextProperty);
            set => SetValue(ButtonTwoTextProperty, value);
        }

        public ICommand ButtonOneCommand
        {
            get => (RelayCommand)GetValue(ButtonOneCommandProperty);
            set => SetValue(ButtonOneCommandProperty, value);
        }

        public ICommand ButtonTwoCommand
        {
            get => (RelayCommand)GetValue(ButtonTwoCommandProperty);
            set => SetValue(ButtonOneCommandProperty, value);
        }

        public bool HasButtons
        {
            get => (bool)GetValue(HasButtonsProperty);
            set => SetValue(HasButtonsProperty, value);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TitleBlock.Text = Title;
            DesktBlock.Text = Description;
            if (!HasButtons)
            {
                ButtonsPanel.Visibility = Visibility.Collapsed;
            }else
            {
                ButtonsPanel.Visibility = Visibility.Visible;
            }

            BorderShadow.Height = BackgroundNotification.Height;
            BorderShadow.Width = BackgroundNotification.Width;

        }

        private void BackgroundNotification_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderShadow.Height = e.NewSize.Height;
            BorderShadow.Width = e.NewSize.Width;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _container = Container.Get;

            var service = _container.Resolve<NotificationService>();
            service.ClosedNotification(new Models.Notification() {Title = Title, Description = Description, IsClickButton = true });
        }
    }
}
