using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class LoadingViewModel:BaseViewModel
    {
        public bool IsLoading { get; set; }

        private IContainer _container;
        public LoadingViewModel(IContainer container)
        {
            _container = container;
            var service = _container.Resolve<LoadingService>();
            service.LoadingChangedEvent += Service_LoadingChangedEvent;
        }

        private void Service_LoadingChangedEvent(bool result)
        {
            IsLoading = result;
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Changed("IsLoading");
            });
        }
    }
}
