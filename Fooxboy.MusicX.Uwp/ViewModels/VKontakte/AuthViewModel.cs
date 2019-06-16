using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class AuthViewModel:BaseViewModel
    {

        private static AuthViewModel instanse;

        public static AuthViewModel Instanse
        {
            get
            {
                if (instanse is null) instanse = new AuthViewModel();

                return instanse;
            }
        }

        private AuthViewModel()
        {

        }
    }
}
