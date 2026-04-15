using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;

namespace Gym2.Views
{
    public partial class ManejoUsuarios : Window
    {
        public ManejoUsuarios()
        {
            // Ahora sí reconocerá este método porque el nombre de la clase coincide con el XAML
            InitializeComponent();
            this.DataContext = new Gym2.ViewModel.ManejoUsuariosViewModel();
        }
    }
}
