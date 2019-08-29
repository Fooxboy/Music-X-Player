using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Blocks
{
    public class AllTracksViewModel:BaseViewModel
    {
        public AllTracksViewModel()
        {
            Tracks = new ObservableCollection<AudioFile>();
            SelectPlaylist = new PlaylistFile()
            {
                Artist = "",
                Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
                Id = 990,
                IsLocal = false,
                Name = "amm"
            };
        }
        public ObservableCollection<AudioFile> Tracks { get; set; }
        public PlaylistFile SelectPlaylist { get; set; }
        public AudioFile SelectTrack { get; set; }
        public bool IsLoading { get; set; }
        public string Title { get; set; }


        public async Task StartLoading(string blockId, string titleBlock)
        {
            try
            {
                Title = titleBlock;
                Changed("Title");
                IsLoading = true;
                Changed("IsLoading");
                var block = await MusicX.Core.VKontakte.Music.Block.GetById(blockId);
                Title = block.Title;
                Changed("Title");
                var tracksVk = block.Audios.ToIAudioFileList();
                var tracks = await MusicService.ConvertToAudioFile(tracksVk);
                Tracks = new ObservableCollection<AudioFile>(tracks);
                Changed("Tracks");
                IsLoading = false;
                Changed("IsLoading");
            }catch(Exception e)
            {
                IsLoading = false;
                Changed("IsLoading");
                await ContentDialogService.Show(new ExceptionDialog("Ошибка при загрузке треков", "Возможно, ВКонтакте не вернул необходимую информацию", e));
            }
            
        }

        public async void MusicListView_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectTrack == null) return;

            SelectPlaylist.TracksFiles = Tracks.ToList();

            await MusicService.PlayMusic(SelectTrack, 2, SelectPlaylist);
            //throw new NotImplementedException();
        }
    }
}
