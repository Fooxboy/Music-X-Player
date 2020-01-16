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
            var file = await localpath.GetFileAsync("config.app");
            var stringConfig = await FileIO.ReadTextAsync(file);
            return JsonConvert.DeserializeObject<ConfigApp>(stringConfig);
        }

        public async Task SetConfig(ConfigApp config)
        {
            var localpath = ApplicationData.Current.LocalFolder;
            var file = await localpath.GetFileAsync("config.app");
            var stringConfig = JsonConvert.SerializeObject(config);
            await FileIO.WriteTextAsync(file, stringConfig);
        }
    }
}
