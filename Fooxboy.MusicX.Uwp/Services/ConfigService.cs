using Fooxboy.MusicX.Uwp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class ConfigService
    {
        public async Task<ConfigApp> GetConfig()
        {
            var localpath = ApplicationData.Current.LocalFolder;

            var item = await localpath.TryGetItemAsync("config.app");
            if (item == null)
            {
                var file = await localpath.CreateFileAsync("config.app");

                var config = new ConfigApp();
                config.IsRateMe = false;
                config.SaveImageToCache = true;
                config.SaveTracksToCache = false;
                config.StreamMusic = false;
                config.ThemeApp = 0;
                var json = JsonConvert.SerializeObject(config);

                await FileIO.WriteTextAsync(file, json);

                return config;

            }

            var storageFile = await localpath.GetFileAsync("config.app");

            var stringConfig = await FileIO.ReadTextAsync(storageFile);
            return JsonConvert.DeserializeObject<ConfigApp>(stringConfig);
        }

        public async Task SetConfig(ConfigApp config)
        {
            var localpath = ApplicationData.Current.LocalFolder;

            var item = await localpath.TryGetItemAsync("config.app");
            if (item == null)
            {
                await localpath.CreateFileAsync("config.app");
            }

            var file = await localpath.GetFileAsync("config.app");
            var stringConfig = JsonConvert.SerializeObject(config);
            await FileIO.WriteTextAsync(file, stringConfig);
        }
    }
}
