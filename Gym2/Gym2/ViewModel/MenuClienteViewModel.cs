using System;
using System.Windows;
using System.Windows.Input;
using Gym2.Commands;
using Gym2.Views;

namespace Gym2.ViewModel
{
    public class MenuClienteViewModel : ViewModelBase
    {
        public ICommand AbrirSeleccionMembresiaCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public MenuClienteViewModel()
        {
            AbrirSeleccionMembresiaCommand = new ViewModelCommand(EjecutarAbrirSeleccion);
            CerrarSesionCommand = new ViewModelCommand(EjecutarCerrarSesion);
        }

        private void EjecutarAbrirSeleccion(object obj)
        {
            // Aquí abrirías la ventana donde el cliente elige específicamente su plan
            ManejoMembresia win = new ManejoMembresia();
            win.Show();
            CerrarVentanaActual(obj);
        }

        private void EjecutarCerrarSesion(object obj)
        {
            LoginView login = new LoginView();
            login.Show();
            CerrarVentanaActual(obj);
        }

        private void CerrarVentanaActual(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}