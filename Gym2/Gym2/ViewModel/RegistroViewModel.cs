using Gym2.Model;
using Gym2.Repositories;
using Gym2.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class RegistroViewModel : ViewModelBase
    {
        private UserModel _user;
        private readonly IUserRepository userRepository;

        // Propiedades para los comandos (definirlas como campos evita que se creen infinitamente)
        private ICommand _addCommand;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public RegistroViewModel()
        {
            userRepository = new UserRepository();
         
            User = new UserModel();
        }


        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new ViewModelCommand(AddExecute, AddCanExecute);
                }
                return _addCommand;
            }
        }

        private void AddExecute(object parameter)
        {
            try
            {
       
                if (User.Password != User.ConfirmPassword)
                {
                    MessageBox.Show("Las contraseñas no coinciden. Por favor, verifica.", "Error de contraseña",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

              
                var existingUser = userRepository.GetByUsername(User.Username);
                if (existingUser != null)
                {
                    MessageBox.Show("El nombre de usuario ya existe. Por favor, elige otro.", "Usuario duplicado",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

             
                User.Id = Guid.NewGuid().ToString();

                userRepository.Add(User);

                MessageBox.Show("Usuario añadido correctamente.", "Éxito", MessageBoxButton.OK,
                    MessageBoxImage.Information);

              
                User = new UserModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar: {ex.Message}", "Error de Base de Datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AddCanExecute(object parameter)
        {
      
            return User != null &&
                   !string.IsNullOrWhiteSpace(User.Username) &&
                   !string.IsNullOrWhiteSpace(User.Password) &&
                   !string.IsNullOrWhiteSpace(User.ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(User.Name);
        }

    
    }
}