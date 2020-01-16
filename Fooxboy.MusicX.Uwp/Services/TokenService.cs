using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Newtonsoft.Json;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class TokenService
    {
        private readonly ConfigService _config;
        public TokenService(ConfigService configService) 
        {
            _config = configService;
        }

        public async Task Save(string token)
        {
            var config = await _config.GetConfig();
            config.AccessTokenVkontakte = token;
            await _config.SetConfig(config);
        }

        public async Task<string> Get()
        {
            var config = await _config.GetConfig();
            return config.AccessTokenVkontakte;
        }


        public async Task Delete()
        {
            var config = await _config.GetConfig();
            config.AccessTokenVkontakte = null;
            await _config.SetConfig(config);
        }

        
    }
}
