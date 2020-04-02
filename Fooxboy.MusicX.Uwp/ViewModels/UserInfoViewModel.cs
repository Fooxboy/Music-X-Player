using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class UserInfoViewModel : ReactiveObject
    {
        private readonly IVkApi _vkApi;
        private readonly ObservableAsPropertyHelper<string> _photo;
        private readonly ObservableAsPropertyHelper<string> _fullName;

        public UserInfoViewModel(IVkApi vkApi)
        {
            _vkApi = vkApi;

            var userObservable = LoadUserAsync().ToObservable().Where(user => user != null);
            _photo = userObservable.Select(user => user.Photo100.ToString()).ToProperty(this, nameof(Photo));
            _fullName = userObservable.Select(user => $"{user.FirstName} {user.LastName}").ToProperty(this, nameof(FullName));
        }

        public string Photo => _photo?.Value;

        public string FullName => _fullName?.Value;

        private async Task<User> LoadUserAsync()
        {
            var user = await _vkApi.Users.GetAsync(new long[] { }, ProfileFields.Photo100).ConfigureAwait(false);
            return user.FirstOrDefault();
        }
    }
}