using System;
using System.Windows;

namespace VectronsLibrary.Wpf.Extensions
{
    public static class IServiceProviderExtension
    {
        private const string ServiceNotFound = "No service for type '{0}' has been registered.";

        public static View GetView<View, ViewModel>(this IServiceProvider serviceProvider)
            where View : FrameworkElement
        {
            var viewType = typeof(View);
            if (!(serviceProvider.GetService(viewType) is View view))
            {
                throw new InvalidOperationException(string.Format(ServiceNotFound, viewType.FullName));
            }

            var viewModelType = typeof(ViewModel);
            var viewModel = serviceProvider.GetService(viewModelType);
            view.DataContext = viewModel ?? throw new InvalidOperationException(string.Format(ServiceNotFound, viewModelType.FullName));
            return view;
        }
    }
}