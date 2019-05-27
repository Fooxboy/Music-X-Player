using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Utils.Extensions
{
    public static class PlaylistFileExtensions
    {
        public static AudioPlaylist ToAudioPlaylist(this PlaylistFile source)
        {
            var audioPlaylist = new AudioPlaylist()
            {
                Shuffle = false,
                Cover = source.Cover,
                CurrentItem = source.Tracks.Count != 0? source.Tracks[0].ToIAudio(): null,
                Id = source.Id,
                Items = source.Tracks.Count != 0 ? source.Tracks.ToIAudioList(): new List<Interfaces.IAudio>(),
                Repeat = Enums.RepeatMode.None,
                Artist = source.Artist,
                Name = source.Name
            };

            return audioPlaylist;
        }

        public static AudioPlaylist ToAudioPlaylist(this PlaylistFile source, AudioFile fileNow)
        {
            var audioPlaylist = new AudioPlaylist()
            {
                Shuffle = false,
                Cover = source.Cover,
                CurrentItem = fileNow.ToIAudio(),
                Id = source.Id,
                Items = source.Tracks.Count != 0 ? source.Tracks.ToIAudioList() : new List<Interfaces.IAudio>(),
                Repeat = Enums.RepeatMode.None,
                Artist = source.Artist,
                Name = source.Name
            };

            return audioPlaylist;
        }
    }
}
