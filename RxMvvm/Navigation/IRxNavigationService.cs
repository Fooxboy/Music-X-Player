using System;
using ReactiveUI;

namespace RxMvvm.Navigation
{
    public interface IRxNavigationService
    {
        IObservable<IRoutableViewModel> Navigate<TViewModel>(RoutingState router)
            where TViewModel : IRoutableViewModel;

        IObservable<IRoutableViewModel> Navigate(IRoutableViewModel viewModel, RoutingState router);
    }
}