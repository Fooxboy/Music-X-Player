using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Fooxboy.MusicX.Uwp.Annotations;
using Fooxboy.MusicX.Uwp.Services;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Resources.ContentDialogs
{
    public sealed partial class PropertiesTrackContentDialog : ContentDialog, INotifyPropertyChanged
    {
        public AudioFile track = null;
        public PropertiesTrackContentDialog(AudioFile file)
        {
            this.InitializeComponent();
            track = file;
        }

        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string TitleTrack { get; set; }
        public string YearAlbum { get; set; }
        public string Genre { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Task.Run(async() =>
            {
                try
                {
                    var file = track.Source;

                    var mp3FileAbs = new FileMp3Abstraction()
                    {
                        Name = file.Name,
                        ReadStream = await file.OpenStreamForReadAsync(),
                        WriteStream = await file.OpenStreamForWriteAsync(),
                    };
                    using (var mp3File = TagLib.File.Create(mp3FileAbs))
                    {
                        mp3File.Tag.AlbumArtists = new string[] {ArtistName};
                        mp3File.Tag.Album = AlbumName;
                        mp3File.Tag.Title = TitleTrack;
                        mp3File.Tag.Year = uint.Parse(YearAlbum);
                        mp3File.Tag.Genres = new string[] {Genre};
                        mp3File.Save();
                    }
                }
                catch (Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Ошибка при сохранении данных о треке", "Music X не смог сохранить информацию. Возможно, трек недоступен или Вы использовали запрещенные символы",e));
                }
                
            });
        }

        private async void PropertiesTrackContentDialog_OnLoading(FrameworkElement sender, object args)
        {

            try
            {
                var file = track.Source;

                var mp3FileAbs = new FileMp3Abstraction()
                {
                    Name = file.Name,
                    ReadStream = await file.OpenStreamForReadAsync(),
                    WriteStream = await file.OpenStreamForWriteAsync(),
                };

                using (var mp3File = TagLib.File.Create(mp3FileAbs))
                {
                    if (mp3File.Tag.AlbumArtists.Count() != 0) ArtistName = mp3File.Tag.AlbumArtists[0];
                    else
                    {
                        if (mp3File.Tag.Artists.Count() != 0) ArtistName = mp3File.Tag.Artists[0];
                        else ArtistName = "Неизвестный исполнитель";
                    }

                    AlbumName = mp3File.Tag.Album;
                    if (mp3File.Tag.Title != null) TitleTrack = mp3File.Tag.Title;
                    else TitleTrack = track.Source.DisplayName;

                    YearAlbum = mp3File.Tag.Year.ToString();
                    Genre = mp3File.Tag.FirstGenre;
                }

                OnPropertyChanged("ArtistName");
                OnPropertyChanged("AlbumName");
                OnPropertyChanged("TitleTrack");
                OnPropertyChanged("YearAlbum");
                OnPropertyChanged("Genre");
            }
            catch (Exception e)
            {
                ArtistName = "Ошибка при получении информации";
                AlbumName = "Ошибка при получении информации";
                TitleTrack = "Ошибка при получении информации";
                YearAlbum = "Ошибка при получении информации";
                Genre = "Ошибка при получении информации";
                OnPropertyChanged("ArtistName");
                OnPropertyChanged("AlbumName");
                OnPropertyChanged("TitleTrack");
                OnPropertyChanged("YearAlbum");
                OnPropertyChanged("Genre");
            }

            

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
