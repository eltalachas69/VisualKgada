using System;
using System.Windows;
using System.Windows.Input;
using Gym2.Commands;
using Gym2.Views;

namespace Gym2.ViewModel
{
    public class MenuAdminViewModel : ViewModelBase
    {
        // Definición de comandos para los cuadros
        public ICommand AbrirClientesCommand { get; }
        public ICommand AbrirUsuariosCommand { get; }
        public ICommand AbrirMembresiasCommand { get; }
        public ICommand AbrirEjerciciosCommand { get; }
        public ICommand AbrirAdminsCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public MenuAdminViewModel()
        {
            // Inicialización de los comandos
            AbrirClientesCommand = new ViewModelCommand(EjecutarAbrirClientes);
            AbrirUsuariosCommand = new ViewModelCommand(EjecutarAbrirUsuarios);
            AbrirMembresiasCommand = new ViewModelCommand(EjecutarAbrirMembresias);
            AbrirEjerciciosCommand = new ViewModelCommand(EjecutarAbrirEjercicios);
            AbrirAdminsCommand = new ViewModelCommand(EjecutarAbrirAdmins);
            CerrarSesionCommand = new ViewModelCommand(EjecutarCerrarSesion);
        }

        private void EjecutarAbrirAdmins(object obj)
        {
            ManejoUsuariosAdmin win = new ManejoUsuariosAdmin();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarAbrirClientes(object obj)
        {
            ManejoCliente win = new ManejoCliente();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarAbrirUsuarios(object obj)
        {
            ManejoUsuarios win = new ManejoUsuarios();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarAbrirMembresias(object obj)
        {
            ManejoMembresia win = new ManejoMembresia();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarAbrirEjercicios(object obj)
        {
            ManejoEjercicios win = new ManejoEjercicios();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarCerrarSesion(object obj)
        {
            LoginView login = new LoginView();
            login.Show();
            CerrarVentanaActual(obj);
        }

        // Método auxiliar para cerrar el menú al abrir otra cosa
        private void CerrarVentanaActual(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}