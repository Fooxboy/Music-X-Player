using System;
using DryIoc;
using ReactiveUI;

namespace RxMvvm.Navigation
{
    public class RxNavigationService : IRxNavigationService
    {
        public IObservable<IRoutableViewModel> Navigate<TViewModel>(RoutingState router)
            where TViewModel : IRoutableViewModel
        {
            var viewModel = Rx.IoCProvier.Resolve(typeof(TViewModel));

            return Navigate((IRoutableViewModel) viewModel, router);
        }

        public IObservable<IRoutableViewModel> Navigate(IRoutableViewModel viewModel, RoutingState router)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (router == null)
            {
                throw new ArgumentNullException(nameof(router));
            }

            return router.Navigate.Execute(viewModel);
        }
    }
}