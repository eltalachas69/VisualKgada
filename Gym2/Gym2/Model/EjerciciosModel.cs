using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Model
{
    public class EjerciciosModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string MusculosTrabajados { get; set; }
        public int Dificultad  { get; set; }
        public byte[] Imagen { get; set; }

    }
}
