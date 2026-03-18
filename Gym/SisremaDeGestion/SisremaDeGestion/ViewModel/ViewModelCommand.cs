using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SisremaDeGestion.View_Model
{
    public class ViewModelCommand : ICommand
    {
        //Representa el metodo que se ejecuta 
        //cuando se llama Execute
        private readonly Action<object> _executeAction;

        //Representa una funcion que devuelve bool
        //y nos dice si el comando se puede ejecutar o no
        private readonly Predicate<object> _canExecuteAction;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
