using Gym2.Model;
using Gym2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username; // el usuario
        private string _password; // la contraseña
        private readonly IUserRepository userRepository; // repositorio para manejar los usuarios

        // Campo para el comando evita recrearlo constantemente
        private ICommand _loginCommand;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new ViewModelCommand(LoginExecute, LoginCanExecute);
                }
                return _loginCommand;
            }
        }

        public LoginViewModel()
        {
            userRepository = new UserRepository();
        }

        private void LoginExecute(object parameter)
        {
            try
            {
                var credential = new NetworkCredential(Username ?? string.Empty, Password ?? string.Empty);
                // 1. Autenticamos las credenciales básicas
                bool authenticated = userRepository.AuthenticateUser(credential);

                if (authenticated)
                {
                    // 2. Obtenemos los datos del usuario para tener su ID
                    var user = userRepository.GetByUsername(Username);

                    // 3. Verificamos si ese ID existe en la tabla Admin
                    // (Nota: Debes agregar este método IsUserAdmin en tu UserRepository)
                    bool esAdmin = userRepository.IsUserAdmin(int.Parse(user.Id));

                    if (esAdmin)
                    {
                        // Si es Admin, entra al menú principal de gestión
                        var adminView = new Gym2.Views.MenuAdmin();
                        adminView.Show();
                    }
                    else
                    {
                        // Si no es Admin, es un Cliente
                        // Aquí abrirías tu ventana de Menú Cliente
                        // var clienteView = new MenuClienteWindow();
                        // clienteView.Show();
                        MessageBox.Show("Bienvenido, Cliente.");
                    }

                    // Cerramos la ventana de Login actual
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item is Views.LoginView) item.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool LoginCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }



    }

    
}
