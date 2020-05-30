using Fooxboy.MusicX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
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
            try
            {
                var info = await _config.GetConfig();
                IsActive = info.SaveImageToCache;
            }
            catch (Exception e)
            {
                IsActive = false;
                _logger.Error("Ошибка инициализации Cacher Service", e);
            }
           
        }

        public async Task<string> GetImage(string url)
        {
            try
            {
                if (!IsActive) return url;

                var pathCache = ApplicationData.Current.LocalCacheFolder;
                var hash = url.GetHashCode();

                var fileName = hash + ".jpg";

                var item = await pathCache.TryGetItemAsync(fileName);

                if (item == null)
                {
                    var cover = await pathCache.CreateFileAsync(hash + ".jpg");
                    BackgroundDownloader downloader = new BackgroundDownloader();
                    DownloadOperation download = downloader.CreateDownload(new Uri(url), cover);
                    await download.StartAsync();

                    return url;
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
