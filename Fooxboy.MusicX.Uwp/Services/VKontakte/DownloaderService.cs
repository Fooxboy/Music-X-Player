using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public class DownloaderService
    {

        private static DownloaderService service;

        public static DownloaderService GetService
        {
            get
            {
                if (service == null) service = new DownloaderService();

                return service;
            }
        }


        private DownloaderService()
        {

        }
    }
}
