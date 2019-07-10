using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
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
    public sealed partial class PropertiesTrackContentDialog : ContentDialog
    {
        public AudioFile track = null;
        public PropertiesTrackContentDialog(AudioFile file)
        {
            this.InitializeComponent();
            track = file;

            using (var mp3File = TagLib.File.Create(file.Source.Path))
            {
                if (mp3File.Tag.AlbumArtists.Count() != 0) ArtistName = mp3File.Tag.AlbumArtists[0];
                else
                {
                    if (mp3File.Tag.Artists.Count() != 0) ArtistName = mp3File.Tag.Artists[0];
                    else ArtistName = "Неизвестный исполнитель";
                }

                AlbumName = mp3File.Tag.Album;
                if (mp3File.Tag.Title != null) TitleTrack = mp3File.Tag.Title;
                else TitleTrack = file.Source.DisplayName;

                YearAlbum = mp3File.Tag.Year.ToString();
                Genre = mp3File.Tag.FirstGenre;
            }
        }

        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string TitleTrack { get; set; }
        public string YearAlbum { get; set; }
        public string Genre { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Task.Run(() =>
            {
                using (var mp3File = TagLib.File.Create(track.Source.Path))
                {
                    mp3File.Tag.AlbumArtists = new string[] { ArtistName };
                    mp3File.Tag.Album = AlbumName;
                    mp3File.Tag.Title = TitleTrack;
                    mp3File.Tag.Year = uint.Parse(YearAlbum);
                    mp3File.Tag.Genres = new string[] { Genre };
                    mp3File.Save();
                }
            });
        }
    }
}
