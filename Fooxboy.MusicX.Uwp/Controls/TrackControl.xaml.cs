using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using VkNet.Model.Attachments;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class TrackControl : UserControl
    {
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track",
            typeof(Audio), typeof(TrackControl), new PropertyMetadata(null));

        public TrackControl()
        {
            this.InitializeComponent();

            PlayCommand = new RelayCommand( async () =>
            {
               
            });

            

            DeleteCommand = new RelayCommand(async () =>
            {
                
            });

           
            
            AddOnLibraryCommand = new RelayCommand(async () =>
            {
                
            });

            
            GoToArtistCommand = new RelayCommand(() =>
            {
               
            });

        }


        public string Artists { get; set; }

        public Audio Track
        {
            get { return (Audio)GetValue(TrackProperty); }
            set
            {
                //foreach (var artist in value.Artists) Artists += artist.Name + ", ";
                SetValue(TrackProperty, value);
            }
        }

      
        private RelayCommand PlayCommand { get; set; }
        private RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddOnLibraryCommand { get; set; }
        public RelayCommand GoToArtistCommand { get; set; }


        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            RectanglePlay.Visibility = Visibility.Visible;
            IconPlay.Visibility = Visibility.Visible;
        }



        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RectanglePlay.Visibility = Visibility.Collapsed;
            IconPlay.Visibility = Visibility.Collapsed;
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            GoToArtist.Visibility = Visibility.Collapsed;
            AddOnLibrary.Visibility = Visibility.Visible;
           
            if (Track.IsLicensed.GetValueOrDefault())
            {
                if (Track.Artist != null)
                {
                    if (Track.Artist.Length != 0)
                    {
                        GoToArtist.Visibility = Visibility.Visible;
                    }
                    
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Track.AccessKey == "loading")
            {
                LoadingGrid.Visibility = Visibility.Visible;
                TrackGrid.Visibility = Visibility.Collapsed;
            }else if(Track.AccessKey == "space")
            {
                LoadingGrid.Visibility = Visibility.Visible;
                TrackGrid.Visibility = Visibility.Collapsed;
                progressRing.Visibility = Visibility.Collapsed;
            }

            if(Track.ContentRestricted.HasValue)
            {
                TitleText.Foreground = new SolidColorBrush(Colors.Gray);
                ArtistText.Foreground = new SolidColorBrush(Colors.Gray);

            }
        }
    }
}
