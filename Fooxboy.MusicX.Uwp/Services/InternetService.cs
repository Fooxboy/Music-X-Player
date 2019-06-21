using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class InternetService
    {
        public static bool Connected { get; set; }

        public static bool CheckConnection()
        {
            Connected = NetworkInterface.GetIsNetworkAvailable();
            return Connected;
        }


        public static void CheckConnectionAuto()
        {

        }
    }
}
