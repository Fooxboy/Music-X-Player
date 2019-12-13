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

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class InAppNotification : UserControl
    {
        public InAppNotification()
        {
            this.InitializeComponent();
        }

        public event Action ClickButton;
        public event Action ClickNotification;

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(InAppNotification), new PropertyMetadata("", (d, e) =>
            {
                var control = (InAppNotification)d;
                var value = (string)e.NewValue;
                //TODO: обновление контрола
            }));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
           typeof(string),
           typeof(InAppNotification), new PropertyMetadata("", (d, e) =>
           {
               var control = (InAppNotification)d;
               var value = (string)e.NewValue;
                //TODO: обновление контрола
            }));

        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText",
           typeof(string),
           typeof(InAppNotification), new PropertyMetadata("", (d, e) =>
           {
               var control = (InAppNotification)d;
               var value = (string)e.NewValue;
               control.VisibileButton = true;
               //TODO: обновление контрола
           }));

        public static readonly DependencyProperty ButtonIconProperty = DependencyProperty.Register("ButtonIcon",
           typeof(string),
           typeof(InAppNotification), new PropertyMetadata("", (d, e) =>
           {
               var control = (InAppNotification)d;
               var value = (string)e.NewValue;
               control.VisibileButton = true;

               //TODO: обновление контрола
           }));




        public string Title { get; set; }
        public string Description { get; set; } 
        public string ButtonText { get; set; }
        public string ButtonIcon { get; set; }
        public bool VisibileButton { get; set; } = false;
    }
}
