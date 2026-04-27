using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Gym2.ViewModel;


namespace Gym2.Views
{
    public partial class MenuCliente : Window
    {
        public MenuCliente()
        {
            InitializeComponent();
            this.DataContext = new MenuClienteViewModel();
        }
    }
}
