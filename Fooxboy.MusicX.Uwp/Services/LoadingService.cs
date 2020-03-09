using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class LoadingService
    {
        public event LoadingChanged LoadingChangedEvent;

        public void Change(bool result)
        {
            LoadingChangedEvent?.Invoke(result);
        }
    }
}
