using System;
using System.Threading.Tasks;
using Windows.Storage;
using Fooxboy.MusicX.Core.New.Services;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Uwp.Services
{
    public abstract class SettingsManagerBase<T> : ISettingsManager<T>
    {
        private readonly StorageFolder _storageFolder;

        protected SettingsManagerBase()
        {
            _storageFolder = ApplicationData.Current.LocalFolder;
        }

        public abstract string File { get; }

        public async Task<T> LoadAsync()
        {
            var file = await _storageFolder.TryGetItemAsync(File);

            if (file == null)
            {
                return default;
            }

            var sessionString = await FileIO.ReadTextAsync((StorageFile) file);

            return JsonConvert.DeserializeObject<T>(sessionString);
        }

        public async Task SaveAsync(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var file = await _storageFolder.TryGetItemAsync(File) ??
                       await _storageFolder.CreateFileAsync(File);

            var sessionString = JsonConvert.SerializeObject(obj);

            await FileIO.WriteTextAsync((StorageFile) file, sessionString);
        }
    }
}