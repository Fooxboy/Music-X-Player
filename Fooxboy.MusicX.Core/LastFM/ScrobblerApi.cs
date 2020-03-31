using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Lpfm.LastFmScrobbler;

namespace Fooxboy.MusicX.Core.LastFM
{
    public class ScrobblerApi
    {
        private Scrobbler _scrobbler;
        public ScrobblerApi(Scrobbler scrobbler)
        {
            this._scrobbler = scrobbler;
        }

        public void SetNowPlaying(ITrack track)
        {
            _scrobbler.NowPlaying(track.ToTrack());
        }

        public async Task SetNowPlayingAsync(ITrack track)
        {
            await Task.Run(() => SetNowPlaying(track));
        }

        public void Track(ITrack track)
        {
            _scrobbler.Scrobble(track.ToTrack());
        }

        public async Task TrackAsync(ITrack track)
        {
            await Task.Run(() => Track(track));
        }

        public void Like(ITrack track)
        {
            _scrobbler.Love(track.ToTrack());

        }

        public async Task LikeAsync(ITrack track)
        {
            await Task.Run(() => Like(track));
        }

        public void Dislike(ITrack track)
        {
            _scrobbler.UnLove(track.ToTrack());
        }

        public async Task DislikeAsync(ITrack track)
        {
            await Task.Run(() => Dislike(track));
        }



    }
}
