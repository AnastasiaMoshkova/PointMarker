using System;
using System.Windows.Input;

namespace PointMarker.ViewModel
{
    public class Command : ICommand
    {
        public Command(Action action)
        {
            this.action = action;
        }

        Action action;

        EventHandler canExecuteChanged;
        event EventHandler ICommand.CanExecuteChanged
        {
            add { canExecuteChanged += value; }
            remove { canExecuteChanged -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
