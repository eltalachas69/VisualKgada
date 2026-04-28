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
        private string _username; 
        private string _password; 
        private readonly IUserRepository userRepository;
        private ICommand _loginCommand;
        private ICommand _showRegisterCommand;

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

        public ICommand ShowRegisterCommand
        {
            get
            {
                if (_showRegisterCommand == null)
                    _showRegisterCommand = new ViewModelCommand(ShowRegisterExecute);
                return _showRegisterCommand;
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
                    var user = userRepository.GetByUsername(Username);

                    bool esAdmin = userRepository.IsUserAdmin(int.Parse(user.Id));

                    if (esAdmin)
                    {
                        var adminView = new Gym2.Views.MenuAdmin();
                        adminView.Show();
                    }
                    else
                    {
                        var clienteView = new Gym2.Views.MenuCliente();
                        clienteView.Show();
                    }

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

        private void ShowRegisterExecute(object parameter)
        {
            var registroView = new Gym2.Views.Registro();
            registroView.Show();
            CloseLoginWindow();
        }

        private void CloseLoginWindow()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item is Views.LoginView) item.Close();
            }
        }



    }

    
}
