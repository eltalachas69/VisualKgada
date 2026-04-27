using Gym2.Model;
using Gym2.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32; // Para OpenFileDialog
using System.IO;

namespace Gym2.ViewModel
{
    public class ManejoEjerciciosViewModel : ViewModelBase
    {
        // Campos y Repositorio
        private readonly IEjerciciosRepository _repositorio;
        private ObservableCollection<EjerciciosModel> _ejercicios;
        private EjerciciosModel _ejercicioSeleccionado;
        private string _nombre;
        private string _musculos;
        private int _dificultad;
        private byte[] _imagen;
        public byte[] Imagen
        {
            get => _imagen;
            set { _imagen = value; OnPropertyChanged(nameof(Imagen)); }
        }

        public ICommand SeleccionarImagenCommand { get; }

        // Propiedades para Binding
        public ObservableCollection<EjerciciosModel> Ejercicios
        {
            get => _ejercicios;
            set { _ejercicios = value; OnPropertyChanged(nameof(Ejercicios)); }
        }

        public EjerciciosModel EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set
            {
                _ejercicioSeleccionado = value;
                OnPropertyChanged(nameof(EjercicioSeleccionado));
                CargarDatosParaEditar();
            }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public string MusculosTrabajados
        {
            get => _musculos;
            set { _musculos = value; OnPropertyChanged(nameof(MusculosTrabajados)); }
        }

        public int Dificultad
        {
            get => _dificultad;
            set { _dificultad = value; OnPropertyChanged(nameof(Dificultad)); }
        }

        // Comandos CRUD
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NuevoCommand { get; }

        public ManejoEjerciciosViewModel()
        {
            _repositorio = new EjerciciosRepository();
            GuardarCommand = new ViewModelCommand(ExecuteGuardar);
            EliminarCommand = new ViewModelCommand(ExecuteEliminar, CanExecuteEditDelete);
            NuevoCommand = new ViewModelCommand(ExecuteNuevo);
            SeleccionarImagenCommand = new ViewModelCommand(ExecuteSeleccionarImagen);

            CargarLista();
        }

        private void ExecuteSeleccionarImagen(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                // Convertir la imagen seleccionada a byte[]
                Imagen = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void CargarLista()
        {
            Ejercicios = new ObservableCollection<EjerciciosModel>(_repositorio.GetAllEjercicios());
        }

        private void CargarDatosParaEditar()
        {
            if (EjercicioSeleccionado != null)
            {
                Nombre = EjercicioSeleccionado.Nombre;
                MusculosTrabajados = EjercicioSeleccionado.MusculosTrabajados;
                Dificultad = EjercicioSeleccionado.Dificultad;
            }
        }

        private void ExecuteGuardar(object obj)
        {
            var ejercicio = new EjerciciosModel
            {
                Id = EjercicioSeleccionado?.Id, // Si es nulo, es un "Add", si tiene ID es "Update"
                Nombre = Nombre,
                MusculosTrabajados = MusculosTrabajados,
                Dificultad = Dificultad,
                Imagen = Imagen
            };

            if (string.IsNullOrEmpty(ejercicio.Id))
                _repositorio.Add(ejercicio);
            else
                _repositorio.Update(ejercicio);

            CargarLista();
            ExecuteNuevo(null); // Limpiar campos
        }

        private void ExecuteEliminar(object obj)
        {
            if (EjercicioSeleccionado != null)
            {
                _repositorio.Delete(EjercicioSeleccionado);
                CargarLista();
                ExecuteNuevo(null);
            }
        }

        private void ExecuteNuevo(object obj)
        {
            EjercicioSeleccionado = null;
            Nombre = string.Empty;
            MusculosTrabajados = string.Empty;
            Dificultad = 0;
        }

        private bool CanExecuteEditDelete(object obj) => EjercicioSeleccionado != null;
    }

}

