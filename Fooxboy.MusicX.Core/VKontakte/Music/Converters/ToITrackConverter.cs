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
    public static class ToIAudioFileConverter
    {
        public static ITrack ToITrack(this Audio audio)
        {
            //Api.Logger.Trace("[CORE] Конвертация ToITrack...");

            ITrack track = new Track();
            try
            {
                track.AccessKey = audio.AccessKey;
                track.Duration = TimeSpan.FromSeconds(audio.Duration);
                track.GenreId = audio.Genre == null? 0: (int)audio.Genre;
                track.Id = audio.Id ?? 0;
                if (audio.Album != null)
                {
                    try
                    {
                        IAlbum alb = new Models.Album();
                        alb.AccessKey = audio.Album?.AccessKey;
                        alb.Id = audio.Album.Id;
                        alb.OwnerId = audio.Album.OwnerId;
                        alb.Title = audio.Album?.Title ?? "";
                        alb.Cover = audio.Album.Cover?.Photo300;
                        track.Album = alb;
                    }
                    catch
                    {
                        Api.Logger.Trace("[CORE] Трек без альбома.");

                    }
                }

                track.Artists = new List<IArtist>();
                if (audio.IsLicensed != null)
                {
                    if (audio.IsLicensed.Value)
                    {
                        //Api.Logger.Trace("[CORE] Трек лицензируется.");

                        if (audio.MainArtists != null && audio.MainArtists?.Count() != 0)
                        {
                            //Api.Logger.Trace("[CORE] Трек имеет исполнителя.");

                            foreach (var artist in audio.MainArtists)
                            {
                                try
                                {
                                   // Api.Logger.Trace($"[CORE] Исполнитель: {artist.Name}");

                                    IArtist art = new Artist();
                                    art.Domain = artist.Domain;
                                    art.Id = artist.Id;
                                    art.Name = artist.Name;
                                    track.Artists.Add(art);
                                }
                                catch
                                {
                                    Api.Logger.Trace("[CORE] Ошибка декодирования исполнителя.");

                                }

                            }
                        }

                        if (audio.FeaturedArtists != null && audio.FeaturedArtists?.Count() != 0)
                        {
                            //Api.Logger.Trace("[CORE] Трек имеет FeaturedArtists.");

                            foreach (var artist in audio.FeaturedArtists)
                            {
                                if (!(track.Artists.Any(t => t.Id == artist.Id)))
                                {
                                    try
                                    {
                                        //Api.Logger.Trace($"[CORE] Исполнитель: {artist.Name}");

                                        IArtist art = new Artist();
                                        art.Domain = artist.Domain;
                                        art.Id = artist.Id;
                                        art.Name = artist.Name;
                                        track.Artists.Add(art);
                                    }
                                    catch
                                    {
                                        Api.Logger.Trace("[CORE] Ошибка декодирования исполнителя.");

                                    }

                                }

                            }
                        }

                    }
                }

                track.Artist = audio.Artist;
                track.Subtitle = audio.Subtitle;
                track.IsAvailable = true;
                track.Title = audio.Title;
                track.Url = audio.Url;
                track.UrlMp3 = audio.Url.DecodeAudioUrl();
                track.IsLicensed = audio.IsLicensed != null && audio.IsLicensed.Value;
                track.OwnerId = audio.OwnerId;
            }
            catch(Exception e)
            {
                Api.Logger.Error("[CORE] Ошибка конвертации трека", e);

                track.Title = audio.Title;
                track.Artist = audio.Artist;
                track.IsAvailable = false;
            }

            return track;
        }

        public static List<ITrack> ToITrackList(this IList<Audio> list)
        {
            var l = new List<ITrack>();
            foreach (var track in list)
            {
                l.Add(track.ToITrack());
            }

            return l;
        }
    }
}
