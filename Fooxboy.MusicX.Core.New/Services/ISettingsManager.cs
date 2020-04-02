using System.Threading.Tasks;

namespace Fooxboy.MusicX.Core.New.Services
{
    public interface ISettingsManager<T>
    {
        Task<T> LoadAsync();

        Task SaveAsync(T obj);
    }
}