using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class SettingsManager : SettingsManagerBase<AppSettings>
    {
        public override string File => "settings.json";
    }
}
