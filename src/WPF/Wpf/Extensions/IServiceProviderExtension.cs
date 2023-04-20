using System;
using System.Globalization;
using System.Windows;

namespace VectronsLibrary.Wpf.Extensions;

/// <summary>
/// Extensions for <see cref="IServiceProvider"/>.
/// </summary>
public static class IServiceProviderExtension
{
    private const string ServiceNotFound = "No service for type '{0}' has been registered.";

    /// <summary>
    /// Get a <typeparamref name="TView"/> and bind the <typeparamref name="TViewModel"/> to the <see cref="FrameworkElement.DataContext"/>.
    /// </summary>
    /// <typeparam name="TView">The type of <see cref="FrameworkElement"/> that needs to be created.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to instantiate services.</param>
    /// <returns> A service object of type <typeparamref name="TView"/>. -or- null if there is no service object of type <typeparamref name="TView"/>.</returns>
    public static TView GetView<TView, TViewModel>(this IServiceProvider serviceProvider)
        where TView : FrameworkElement
    {
        var viewType = typeof(TView);
        if (serviceProvider.GetService(viewType) is not TView view)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ServiceNotFound, viewType.FullName));
        }

        var viewModelType = typeof(TViewModel);
        var viewModel = serviceProvider.GetService(viewModelType);
        view.DataContext = viewModel ?? throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ServiceNotFound, viewModelType.FullName));
        return view;
    }
}