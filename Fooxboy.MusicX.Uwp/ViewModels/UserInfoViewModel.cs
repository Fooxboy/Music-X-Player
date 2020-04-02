using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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

        public UserInfoViewModel(IVkApi vkApi)
        {
            _vkApi = vkApi;

            var userObservable = LoadUserAsync().ObserveOn(RxApp.MainThreadScheduler).Where(user => user != null);
            userObservable.Select(user => user.Photo100).ToPropertyEx(this, model => model.Photo);
            userObservable.Select(user => $"{user.FirstName} {user.LastName}").ToPropertyEx(this, model => model.FullName);
        }

        [ObservableAsProperty]
        public Uri Photo { get; }

        [ObservableAsProperty]
        public string FullName { get; }

        private IObservable<User> LoadUserAsync()
        {
            return _vkApi.Users.GetAsync(new long[] { }, ProfileFields.Photo100).ToObservable()
                .Select(users => users.FirstOrDefault());
        }
    }
}