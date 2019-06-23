using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class MiniPlayerViewModel : BaseViewModel
    {

        public static MiniPlayerViewModel instanse;

        public static PlayerViewModel Instanse
        {
            get
            {
                return PlayerViewModel.Instanse;
            }
        }
    }
}
