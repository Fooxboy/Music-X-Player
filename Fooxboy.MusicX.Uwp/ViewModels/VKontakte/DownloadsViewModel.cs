using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class DownloadsViewModel:BaseViewModel
    {

        private static DownloadsViewModel instanse;

        public static DownloadsViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new DownloadsViewModel();

                return instanse;
            }
        }


        private DownloadsViewModel()
        {

        }

        public Visibility VisibilityNoNowDownload
        {
            get => Visibility.Visible;
            set
            {
                //supprort x:bind
            }
        }

        public Visibility VisibilityNowDownload
        {
            get => Visibility.Collapsed;
            set
            {
                //supprort x:bind
            }
        }
    }
}
