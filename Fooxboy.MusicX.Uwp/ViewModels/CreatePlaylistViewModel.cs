using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Storage;
using Windows.UI.Popups;
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
                if(StaticContent.Playlists.Count >= 15 && StaticContent.IsPro == false) 
                {
                    await new MessageDialog("У Вас уже есть 15 плейлистов. Для того, чтобы создать больше 15 плейлистов, необходимо купить MusicX Pro", "Купите MusicX Pro").ShowAsync();
                }else
                {
                    try
                    {
                        var playlist = new PlaylistFile()
                        {
                            Artist = "Music X",
                            Cover = ImagePlaylist,
                            Id = new Random().Next(10, 1234),
                            Name = NamePlaylist,
                            TracksFiles = new List<AudioFile>(),
                            IsLocal = true
                        };

                        await PlaylistsService.SavePlaylist(playlist);
                        StaticContent.Playlists.Add(playlist);
                        VisibilityGridCreate = Visibility.Collapsed;
                        VisibilityGridDone = Visibility.Visible;
                        NamePlaylist = "";
                        Changed("NamePlaylist");
                    }
                    catch (Exception e)
                    {
                        await ContentDialogService.Show(new ExceptionDialog("Невозможно создать плейлист", "Возможно, такой плейлист уже существует. Попробуйте ещё раз.", e));

                    }
                }
                

            });

            SelectCover = new RelayCommand(async () =>
            {
                try
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
                            cover = await file.CopyAsync(StaticContent.CoversFolder);

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
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Ошибка при выборе файла", "Неизвестная ошибка", e));
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
                    return "ms-appx:///Assets/Images/playlist-placeholder.png";
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
