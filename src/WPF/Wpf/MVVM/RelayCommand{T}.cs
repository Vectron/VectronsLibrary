using System;
using System.Windows.Input;

namespace VectronsLibrary.Wpf.MVVM;

/// <summary>
/// A <see cref="ICommand"/> to bind to commands.
/// </summary>
/// <typeparam name="T">The type of passed parameter.</typeparam>
public class RelayCommand<T> : ICommand<T>
{
    private Predicate<T?> canExecute;
    private Action<T?> execute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The Action that needs to be executed when command is triggered.</param>
    public RelayCommand(Action<T?> execute)
        : this(execute, DefaultCanExecute)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The Action that needs to be executed when command is triggered.</param>
    /// <param name="canExecute">The action to check if the command can be executed.</param>
    public RelayCommand(Action<T?> execute, Predicate<T?> canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
            CanExecuteChangedInternal += value;
        }

        remove
        {
            CommandManager.RequerySuggested -= value;
            CanExecuteChangedInternal -= value;
        }
    }

    private event EventHandler? CanExecuteChangedInternal;

    /// <inheritdoc/>
    public bool CanExecute(object? parameter)
        => parameter == null
        ? CanExecute((T?)parameter)
        : parameter.GetType() != typeof(T)
            ? throw new ArgumentException("Parameter if of wrong type", nameof(parameter))
            : CanExecute((T)parameter);

    /// <inheritdoc/>
    public bool CanExecute(T? parameter)
        => canExecute != null && canExecute(parameter);

    /// <summary>
    /// Destroy this <see cref="ICommand"/> so it wont trigger anymore.
    /// </summary>
    public void Destroy()
    {
        canExecute = _ => false;
        execute = _ =>
        {
            return;
        };
    }

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        if (parameter == null)
        {
            Execute((T?)parameter);
            return;
        }

        if (parameter.GetType() != typeof(T))
        {
            throw new ArgumentException("Parameter if of wrong type", nameof(parameter));
        }

        Execute((T)parameter);
    }

    /// <inheritdoc/>
    public void Execute(T? parameter)
        => execute(parameter);

    /// <summary>
    /// Trigger event that on execute has changed.
    /// </summary>
    public void OnCanExecuteChanged()
        => CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);

    private static bool DefaultCanExecute(T? parameter)
        => true;
}