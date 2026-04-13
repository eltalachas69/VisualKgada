using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Gym2.Model; // Asegúrate de referenciar tu carpeta Model

namespace Gym2.ViewModel
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        // Propiedades privadas para almacenar los datos del formulario
        private string _username;
        private string _password;
        private string _email;

        // Propiedades públicas que se enlazarán (Binding) en el XAML
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        // Comando para la acción de registrar
        public ICommand RegistrarCommand { get; }

        public RegistroViewModel()
        {
            // Aquí inicializarías el comando, por ejemplo, conectándolo a un método GuardarUsuario
        }

        // Evento necesario para que la vista se entere cuando cambian los datos
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}