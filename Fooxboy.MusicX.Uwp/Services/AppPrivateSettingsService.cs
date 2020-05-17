using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class AppPrivateSettingsService
    {

        private  ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        public object Get(string value)
        {
            try
            {
                return _settings.Values[value];
            }
            catch
            {
                return null;
            }
        }

        public void Set(string property, object value)
        {
            _settings.Values[property] = value;
        }

        public ApplicationTheme GetTheme()
        {

            var value = this.Get("ThemeApp");
            if (value != null)
            {
                var theme = (int) value;
                return theme == 0 ? ApplicationTheme.Light : ApplicationTheme.Dark;
            }
            else
            {
                this.Set("ThemeApp", 0);
                return ApplicationTheme.Light;
            }
        }
    }
}
