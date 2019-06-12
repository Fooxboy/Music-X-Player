using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Newtonsoft.Json;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class ConfigService
    {

        public static async Task SaveConfig(ConfigApp config)
        {
            try
            {
                var configFile = await StaticContent.LocalFolder.GetFileAsync("ConfigApp.json");
                var configString = JsonConvert.SerializeObject(config);
                await FileIO.WriteTextAsync(configFile, configString);
            }catch(Exception e)
            {
                await new ExceptionDialog("Не удалось сохранить настройки приложения", "Возможно, файл занят другим приложением, если ошибка будет повторятся, переустановите приложение", e).ShowAsync();
            }
            
        }
    }
}
