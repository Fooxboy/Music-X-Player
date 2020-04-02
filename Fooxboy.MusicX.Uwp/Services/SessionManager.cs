using Fooxboy.MusicX.Core.New.Models;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class SessionManager : SettingsManagerBase<Session>
    {
        public override string File => "session.json";
    }
}