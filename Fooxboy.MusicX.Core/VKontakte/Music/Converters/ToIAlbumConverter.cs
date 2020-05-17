using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class ToIAlbumConverter
    {
        public static IAlbum ToIAlbum(this AudioPlaylist  playlist)
        {
            IAlbum album = new Models.Album();
            try
            {
                //Api.Logger.Trace("[CORE] Конвертация ToIAlbum...");
                album.Title = playlist.Title;
                album.Id = playlist.Id.Value;
                album.Description = playlist.Description;
                album.Followers = playlist.Followers;
                album.Plays = playlist.Plays;
                album.Type = playlist.Type;
                album.Year = playlist.Year ?? 0;
                album.TimeUpdate = playlist.UpdateTime;
                album.TimeCreate = playlist.CreateTime;
                album.AccessKey = playlist.AccessKey;
                album.OwnerId = playlist.OwnerId ?? 0;
                album.IsFollowing = playlist.IsFollowing;
                album.Cover = playlist.Cover?.Photo600;
                album.Artists = new List<IArtist>();
                if (playlist.MainArtists != null)
                {
                    foreach (var artist in playlist.MainArtists)
                    {
                        try
                        {
                            IArtist art = new Artist();
                            art.Domain = artist.Domain;
                            art.Id = artist.Id;
                            art.Name = artist.Name;
                            album.Artists.Add(art);
                        }
                        catch
                        {
                        }
                    }
                }

                album.Genres = new List<string>();
                if (playlist.Genres != null)
                {
                    foreach (var genre in playlist.Genres)
                    {
                        album.Genres.Add(genre.Name);
                    }
                }
                album.Tracks = new List<ITrack>();
                album.IsAvailable = true;
            }
            catch(Exception exception)
            {
                Api.Logger.Error("[CORE] Невозможно сконвертировать альбом", exception);
                album.Title = playlist.Title?? "Альбом недоступен";
                album.IsAvailable = false;
            }
            return album;
        }
    }
}
