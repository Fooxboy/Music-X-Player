using Fooxboy.MusicX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class ImageCacheService
    {
        private ConfigService _config;
        private ILoggerService _logger;
        public ImageCacheService(ConfigService config, LoggerService logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool IsActive { get; set; }

        public async Task InitService()
        {
            var info = await _config.GetConfig();
            IsActive = info.SaveImageToCache;
        }

        public async Task<string> GetImage(string url)
        {
            try
            {
                return url;

                var pathCache = ApplicationData.Current.LocalCacheFolder;
                var hash = url.GetHashCode();

                var fileName = hash + ".jpg";

                var item = await pathCache.TryGetItemAsync(fileName);

                if (item == null)
                {
                    return url;
                    //TODO: скачиваем файл
                }
                else return item.Path;
            }catch(Exception e)
            {
                _logger.Error("Ошибка при кэшировании картинки", e);
                return url;
            }
            
        }

    }
}
