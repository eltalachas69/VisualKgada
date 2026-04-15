using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class ViewModelCommand : ICommand
    {
        //Representa el metodo que se ejecuta cuando se llama execute
        private readonly Action<object> _executeAction;
        //Representa una función que devuelve bool y nos dice si el comando se puede ejecutar
        private readonly Predicate<object> _canExecuteAction;
        //Constructores
        //Este constructor permite ejecutar una acción sin revisión previa
        public ViewModelCommand(Action<object> execureAction)
        {
            _executeAction = execureAction;
            _canExecuteAction = null;
        }
        //Este constructor realiza una cción ejecutando una revisión previa
        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }
        //Este método me permite ejecutar el evento
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /* Indica si se puede ejecutar el comando o no
         * -Si _canExecute es null --> devuelve True
         * (El comando siempre se puede ejecutar)
         * si no es null --> se llama al predicador para que decida */
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null ? true :
                _canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
