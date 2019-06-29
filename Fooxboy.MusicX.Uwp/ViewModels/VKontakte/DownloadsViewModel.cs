using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
