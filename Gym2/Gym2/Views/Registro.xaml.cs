using System.Windows;

namespace Gym2.Views // 
{
    public partial class Registro : Window
    {
        public Registro()
        {
            InitializeComponent();
            this.DataContext = new Gym2.ViewModel.RegistroViewModel();
        }

        // Agrega estos métodos para quitar los errores CS1061 de la imagen
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}