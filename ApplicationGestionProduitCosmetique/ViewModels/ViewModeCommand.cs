using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApplicationGestionProduitCosmetique.ViewModels
{
    public class ViewModeCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Predicate<object> _pasAction;
        public ViewModeCommand(Action<object> action)
        {
            _action = action;
            _pasAction = null;
        }
        public ViewModeCommand(Action<object> action, Predicate<object> conAction)
        {
            _action = action;
            _pasAction = conAction;
        }


        event EventHandler? ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            return _pasAction == null ? true : _pasAction(parameter);
        }

        public void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
