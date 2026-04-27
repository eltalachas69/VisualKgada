using Gym2.Model;
using Gym2.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class ManejoUsuariosAdminViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private ObservableCollection<AdminModel> _admins;
        private ObservableCollection<UserModel> _allUsers;
        private UserModel _selectedUser;
        private AdminModel _selectedAdmin;

        // Propiedades para la Vista
        public ObservableCollection<AdminModel> Admins
        {
            get => _admins;
            set { _admins = value; OnPropertyChanged(nameof(Admins)); }
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

        public AdminModel SelectedAdmin
        {
            get => _selectedAdmin;
            set { _selectedAdmin = value; OnPropertyChanged(nameof(SelectedAdmin)); }
        }

        // Comandos
        public ICommand MakeAdminCommand { get; }
        public ICommand RemoveAdminCommand { get; }
        public ICommand RefreshCommand { get; }

        public ManejoUsuariosAdminViewModel()
        {
            _userRepository = new UserRepository();

            // Inicializar Comandos
            MakeAdminCommand = new ViewModelCommand(ExecuteMakeAdminCommand, CanExecuteMakeAdmin);
            RemoveAdminCommand = new ViewModelCommand(ExecuteRemoveAdminCommand, CanExecuteRemoveAdmin);
            RefreshCommand = new ViewModelCommand(p => LoadData());

            // Cargar datos iniciales
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Obtenemos todos los usuarios y los que son admins específicamente
                var users = _userRepository.GetAllUsers();
                var admins = _userRepository.GetAllAdmins();

                AllUsers = new ObservableCollection<UserModel>(users);
                Admins = new ObservableCollection<AdminModel>(admins);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        // Lógica para Agregar a la tabla Admin
        private bool CanExecuteMakeAdmin(object obj) => SelectedUser != null;

        private void ExecuteMakeAdminCommand(object obj)
        {
            try
            {
                // Verificamos si ya es admin para no duplicar
                if (Admins.Any(a => a.UserId.ToString() == SelectedUser.Id))
                {
                    MessageBox.Show("Este usuario ya es administrador.");
                    return;
                }

                _userRepository.AddAdmin(int.Parse(SelectedUser.Id));
                LoadData(); // Refrescar listas
                MessageBox.Show("Usuario promovido a Administrador con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Lógica para Quitar de la tabla Admin
        private bool CanExecuteRemoveAdmin(object obj) => SelectedAdmin != null;

        private void ExecuteRemoveAdminCommand(object obj)
        {
            try
            {
                var result = MessageBox.Show($"¿Quitar privilegios de admin a {SelectedAdmin.Username}?",
                    "Confirmar", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _userRepository.RemoveAdmin(SelectedAdmin.UserId);
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
