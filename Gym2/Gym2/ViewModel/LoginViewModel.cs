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

                bool authenticated = userRepository.AuthenticateUser(credential);

                if (authenticated)
                {
                    MessageBox.Show("Inicio de sesión exitoso.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Limpiar contraseña después del login
                    Password = string.Empty;
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar iniciar sesión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool LoginCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }



    }

    
}
