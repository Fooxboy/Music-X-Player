using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class HomeViewModel
    {
        private static HomeViewModel instanse;

        public static HomeViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new HomeViewModel();

                return instanse;
            }
        }
    }
}
