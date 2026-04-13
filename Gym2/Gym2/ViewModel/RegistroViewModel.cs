using Gym2.Model;
using ProyectoEjemploEsco.Model;
using ProyectoEjemploEsco.Repositories;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProyectoEjemploEsco.ViewModel
{
    public class RegistroViewModel : ViewModelBase
    {
        private readonly RepositoryBase repositoryBase;

        private ObservableCollection<UserModel> _users;
        private UserModel _user;
        private IUserRepository userRepository;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

        public RegistroViewModel()
        {
            userRepository = new UserRepository();
            _user = new UserModel();
        }

        public ICommand AddCommand
        {
            get
            {
                return new ViewModelCommand(AddExecute, AddCanExecute);
            }
        }

        private void AddExecute(object user)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User?.Username) ||
                string.IsNullOrWhiteSpace(User.Name) || string.IsNullOrWhiteSpace(User.LastName) ||
                string.IsNullOrWhiteSpace(User.Email))
            {
                MessageBox.Show("Por favor, completa todos los campos antes de guardar.",
                    "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validar que las contraseñas coincidan
            if (User.Password != User.ConfirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, verifica.", "Error de contraseña",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar si el username ya existe usando GetByUsername()
            var existingUser = userRepository.GetByUsername(User.Username);
            if (existingUser != null)
            {
                MessageBox.Show("El nombre de usuario ya existe. Por favor, elige otro.", "Usuario duplicado",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si pasa las validaciones, se añade el usuario
            User.Id = Guid.NewGuid().ToString();
            userRepository.Add(User);
            MessageBox.Show("Usuario añadido correctamente.", "Éxito", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private bool AddCanExecute(object user)
        {
            // Deshabilita el botón si los campos están vacíos
            return !string.IsNullOrWhiteSpace(User?.Username) && !string.IsNullOrWhiteSpace(User?.Password) &&
                   !string.IsNullOrWhiteSpace(User?.Name) && !string.IsNullOrWhiteSpace(User?.LastName) &&
                   !string.IsNullOrWhiteSpace(User?.Email);
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new ViewModelCommand(DeleteExecute, DeleteCanExecute);
            }
        }

        private void DeleteExecute(Object user)
        {
            userRepository.Delete(User); // Borra el usuario usando el Id
                                         // Actualizar la lista de usuarios si es necesario
                                         // Users = userRepository.Get();
        }

        private bool DeleteCanExecute(Object user)
        {
            // Verifica que el objeto user no sea nulo y tenga un Id válido
            return true;
        }

        public ICommand EditCommand
        {
            get
            {
                return new ViewModelCommand(EditExecute, EditCanExecute);
            }
        }

        private void EditExecute(Object user)
        {
            userRepository.Update(User); // Borra el usuario usando el Id
                                         // Actualizar la lista de usuarios si es necesario
                                         // Users = userRepository.Get();
        }

        private bool EditCanExecute(Object user)
        {
            // Verifica que el objeto user no sea nulo y tenga un Id válido
            return true;
        }
    }
}