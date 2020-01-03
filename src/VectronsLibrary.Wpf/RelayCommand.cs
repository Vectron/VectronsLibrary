using System;
using System.Windows.Input;

namespace VectronsLibrary.Wpf
{
    public class RelayCommand : ICommand
    {
        private Predicate<object> canExecute;
        private Action<object> execute;

        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        public event EventHandler CanExecuteChanged
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

        private event EventHandler CanExecuteChangedInternal;

        public static ICommand Empty
            => new RelayCommand(_ => { });

        public bool CanExecute(object parameter)
            => canExecute != null && canExecute(parameter);

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ =>
            {
                return;
            };
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(object parameter)
            => true;
    }

    public class RelayCommand<T> : ICommand<T>
    {
        private Predicate<T> canExecute;
        private Action<T> execute;

        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        public event EventHandler CanExecuteChanged
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

        private event EventHandler CanExecuteChangedInternal;

        public static ICommand<T> Empty
            => new RelayCommand<T>(_ => { });

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return CanExecute((T)parameter);
            }

            if (parameter.GetType() != typeof(T))
            {
                throw new ArgumentException("Parameter if of wrong type", nameof(parameter));
            }

            return CanExecute((T)parameter);
        }

        public bool CanExecute(T parameter)
            => canExecute != null && this.canExecute(parameter);

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ =>
            {
                return;
            };
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                Execute((T)parameter);
            }

            if (parameter.GetType() != typeof(T))
            {
                throw new ArgumentException("Parameter if of wrong type", nameof(parameter));
            }

            Execute((T)parameter);
        }

        public void Execute(T parameter)
        {
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(T parameter) => true;
    }
}