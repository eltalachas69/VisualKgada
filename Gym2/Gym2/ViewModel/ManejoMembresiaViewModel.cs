using Gym2.Model;
using Gym2.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gym2.ViewModel
{
    public class ManejoMembresiaViewModel: ViewModelBase
    {
        private readonly IMembresiaRepository _repositorio;
        private ObservableCollection<MembresiaModel> _membresias;
        private MembresiaModel _membresiaSeleccionada;
        private string _nombre;

        public ObservableCollection<MembresiaModel> Membresias
        {
            get => _membresias;
            set { _membresias = value; OnPropertyChanged(nameof(Membresias)); }
        }

        public MembresiaModel MembresiaSeleccionada
        {
            get => _membresiaSeleccionada;
            set
            {
                _membresiaSeleccionada = value;
                OnPropertyChanged(nameof(MembresiaSeleccionada));
                if (_membresiaSeleccionada != null)
                    Nombre = _membresiaSeleccionada.Nombre;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NuevoCommand { get; }

        public ManejoMembresiaViewModel()
        {
            _repositorio = new MembresiaRepository();
            GuardarCommand = new ViewModelCommand(ExecuteGuardar);
            EliminarCommand = new ViewModelCommand(ExecuteEliminar, CanExecuteEditDelete);
            NuevoCommand = new ViewModelCommand(ExecuteNuevo);
            CargarLista();
        }

        private void CargarLista()
        {
            Membresias = new ObservableCollection<MembresiaModel>(_repositorio.GetAllMembresias());
        }

        private void ExecuteGuardar(object obj)
        {
            var membresia = new MembresiaModel
            {
                Id = MembresiaSeleccionada?.Id,
                Nombre = Nombre
            };

            if (string.IsNullOrEmpty(membresia.Id))
                _repositorio.Add(membresia);
            else
                _repositorio.Update(membresia);

            CargarLista();
            ExecuteNuevo(null);
        }

        private void ExecuteEliminar(object obj)
        {
            if (MembresiaSeleccionada != null)
            {
                _repositorio.Delete(MembresiaSeleccionada);
                CargarLista();
                ExecuteNuevo(null);
            }
        }

        private void ExecuteNuevo(object obj)
        {
            MembresiaSeleccionada = null;
            Nombre = string.Empty;
        }

        private bool CanExecuteEditDelete(object obj) => MembresiaSeleccionada != null;

    }
}
