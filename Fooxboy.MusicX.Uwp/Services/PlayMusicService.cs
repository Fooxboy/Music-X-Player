using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class PlayMusicService
    {
        public async static void PlayMusicForLibrary(AudioFile audioFile)
        {
            var lastPlayPlaylist = await PlaylistsService.GetById(1);
            if (!(lastPlayPlaylist.Tracks.Any(t => t.Source == audioFile.Source))) lastPlayPlaylist.Tracks.Add(audioFile);
        }
    }
}
