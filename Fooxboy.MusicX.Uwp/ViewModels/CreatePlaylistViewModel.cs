using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class CreatePlaylistViewModel : BaseViewModel
    {

        private static CreatePlaylistViewModel instanse;
        public static CreatePlaylistViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new CreatePlaylistViewModel();
                return instanse;
            }
        }
        private CreatePlaylistViewModel()
        {
            CreatePlaylist = new RelayCommand( async () =>
            {
                var playlist = new PlaylistFile()
                {
                    Artist = "Music X",
                    Cover = ImagePlaylist,
                    Id = new Random().Next(0, 500),
                    Name = NamePlaylist,
                    Tracks = new List<AudioFile>()
                };

                await PlaylistsService.SavePlaylist(playlist);
                StaticContent.Playlists.Add(playlist);
                VisibilityGridCreate = Visibility.Collapsed;
                VisibilityGridDone = Visibility.Visible;
            });

            SelectCover = new RelayCommand(async () =>
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".png");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    StorageFile cover;
                    try
                    {
                        cover =await file.CopyAsync(StaticContent.CoversFolder);

                    }
                    catch
                    {
                        cover = await StaticContent.CoversFolder.GetFileAsync(file.Name);
                        await file.CopyAndReplaceAsync(cover);
                    }

                    ImagePlaylist = cover.Path; 
                }
                else
                {
                    
                }
            });

            visibilityGridCreate = Visibility.Visible;
            visibilityGridDone = Visibility.Collapsed;
        }

        string imagePlaylist;
        public string ImagePlaylist
        {
            get
            {
                if(imagePlaylist == null)
                {
                    return "ms-appx:///Assets/Images/placeholder.png";
                }else
                {
                    return imagePlaylist;
                }
            }
            set
            {
                if (value != imagePlaylist) imagePlaylist = value;
            }
        }


        public string NamePlaylist { get; set; }

        public RelayCommand CreatePlaylist { get; set; }
        public RelayCommand SelectCover { get; set; }

        Visibility visibilityGridDone;
        public Visibility VisibilityGridDone
        {
            get
            {
                return visibilityGridDone;
            }set
            {
                if(visibilityGridDone != value)
                {
                    visibilityGridDone = value;
                    Changed("VisibilityGridDone");
                }
            }
        }

        Visibility visibilityGridCreate;
        public Visibility VisibilityGridCreate
        {
            get
            {
                return visibilityGridCreate;
            }
            set
            {
                if (visibilityGridCreate != value)
                {
                    visibilityGridCreate = value;
                    Changed("VisibilityGridCreate");
                }
            }
        }


    }
}
