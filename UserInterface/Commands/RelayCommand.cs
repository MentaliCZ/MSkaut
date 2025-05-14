using System;
using System.Windows.Input;

namespace UserInterface.Commands
{
    public class RelayCommand(Action<object?> executeAction, Predicate<object?> canExecutePredicate)
        : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object?> executeAction = executeAction;
        private Predicate<object?> canExecutePredicate = canExecutePredicate;

        public bool CanExecute(object? parameter) => canExecutePredicate(parameter);

        public void Execute(object? parameter) => executeAction(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
