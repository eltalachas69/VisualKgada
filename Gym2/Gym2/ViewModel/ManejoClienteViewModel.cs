using Gym2.Model;
using Gym2.Repositories; // Asegúrate de tener los métodos en tu repositorio
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class ManejoClienteViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository; // O IClienteRepository si creaste uno separado
        private ObservableCollection<ClienteModel> _clientes;
        private ObservableCollection<UserModel> _allUsers;
        private UserModel _selectedUser;
        private ClienteModel _selectedCliente;

        // Propiedades para la Vista
        public ObservableCollection<ClienteModel> Clientes
        {
            get => _clientes;
            set { _clientes = value; OnPropertyChanged(nameof(Clientes)); }
        }

        public ObservableCollection<UserModel> AllUsers
        {
            get => _allUsers;
            set { _allUsers = value; OnPropertyChanged(nameof(AllUsers)); }
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        public ClienteModel SelectedCliente
        {
            get => _selectedCliente;
            set { _selectedCliente = value; OnPropertyChanged(nameof(SelectedCliente)); }
        }

        // Comandos
        public ICommand MakeClienteCommand { get; }
        public ICommand RemoveClienteCommand { get; }
        public ICommand RefreshCommand { get; }

        public ManejoClienteViewModel()
        {
            _userRepository = new UserRepository(); // Ajusta si usas un repositorio distinto

            // Inicializar Comandos
            MakeClienteCommand = new ViewModelCommand(ExecuteMakeClienteCommand, CanExecuteMakeCliente);
            RemoveClienteCommand = new ViewModelCommand(ExecuteRemoveClienteCommand, CanExecuteRemoveCliente);
            RefreshCommand = new ViewModelCommand(p => LoadData());

            // Cargar datos iniciales
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // NOTA: Debes crear GetAllClientes() en tu UserRepository o ClienteRepository
                var users = _userRepository.GetAllUsers();
                var clientes = _userRepository.GetAllClientes();

                AllUsers = new ObservableCollection<UserModel>(users);
                Clientes = new ObservableCollection<ClienteModel>(clientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        // Lógica para Agregar a la tabla Cliente
        private bool CanExecuteMakeCliente(object obj) => SelectedUser != null;

        private void ExecuteMakeClienteCommand(object obj)
        {
            try
            {
                // Verificamos si ya es cliente para no duplicar
                if (Clientes.Any(c => c.UserId.ToString() == SelectedUser.Id))
                {
                    MessageBox.Show("Este usuario ya está registrado como Cliente.");
                    return;
                }

                // NOTA: Debes crear AddCliente() en tu repositorio
                _userRepository.AddCliente(int.Parse(SelectedUser.Id));
                LoadData(); // Refrescar listas
                MessageBox.Show("Usuario registrado como Cliente con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Lógica para Quitar de la tabla Cliente
        private bool CanExecuteRemoveCliente(object obj) => SelectedCliente != null;

        private void ExecuteRemoveClienteCommand(object obj)
        {
            try
            {
                var result = MessageBox.Show($"¿Eliminar a {SelectedCliente.Username} de la lista de clientes?",
                    "Confirmar", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // NOTA: Debes crear RemoveCliente() en tu repositorio
                    _userRepository.RemoveCliente(SelectedCliente.UserId);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}