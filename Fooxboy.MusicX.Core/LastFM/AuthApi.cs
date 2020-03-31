using System;
using System.Collections.Generic;
using System.Text;
using Lpfm.LastFmScrobbler;

namespace Fooxboy.MusicX.Core.LastFM
{
    public class AuthApi
    {
        private LastFmScrobblerApi _api;
        private Scrobbler _scrobbler;

        public AuthApi(LastFmScrobblerApi api)
        {
            this._api = api;
        }

        public void Auth(string apiKey, string apiSecret, string session)
        {
            _scrobbler = new Scrobbler(apiKey, apiSecret, session);
            _api.SetScrobbler(_scrobbler);
        }

        public void Auth(string session)
        {
            Auth("2a649e9fc2e168da07c69fa32a51ed5b", "396367228492079ec1bf9194ebe8e480", session);
        }

        public void Auth()
        {
            Auth(null);
        }

        public string GetUrlAuth()
        {
            if(!_api.IsAuth) Auth();

            return _scrobbler.GetAuthorisationUri();
        }

        public string GetSession()
        {
            return _scrobbler?.GetSession();
        }
    }
}
