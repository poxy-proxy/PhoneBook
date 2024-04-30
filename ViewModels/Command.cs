using System;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    public class Command : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecutePredicate;

        public event EventHandler CanExecuteChanged;

        public Command(Action executeAction, Func<bool> canExecutePredicate = null)
        {
            _executeAction = executeAction;
            _canExecutePredicate = canExecutePredicate ?? (() => true);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate();
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
