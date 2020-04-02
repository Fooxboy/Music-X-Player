using System;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.New.Models;
using VkNet.Abstractions;
using VkNet.Model;

namespace Fooxboy.MusicX.Core.New.Services
{
    public class AuthorizationService
    {
        private readonly IVkApi _api;
        private readonly ISettingsManager<Session> _sessionManager;

        public AuthorizationService(IVkApi api, ISettingsManager<Session> sessionManager)
        {
            _api = api;
            _sessionManager = sessionManager;
        }

        public async Task AuthorizeAsync(string login, string password, string code, Func<Task<string>> twoFactor)
        {
            try
            {
                await _api.AuthorizeAsync(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    Code = code
                }).ConfigureAwait(false);
            }
            catch (InvalidOperationException) // handle missing two factor callback exception
            {
                code = await twoFactor().ConfigureAwait(false);

                await AuthorizeAsync(login, password, code, twoFactor).ConfigureAwait(false);
            }

            await _sessionManager.SaveAsync(new Session
            {
                AccessToken = _api.Token,
                UserId = _api.UserId.Value
            }).ConfigureAwait(false);
        }

        public async Task<bool> AuthorizeFromSession()
        {
            var session = await _sessionManager.LoadAsync();

            if (session == null)
            {
                return false;
            }

            // TODO: validate properties

            await _api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = session.AccessToken
            });

            _api.UserId = session.UserId;

            return true;
        }
    }
}