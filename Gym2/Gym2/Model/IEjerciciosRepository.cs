using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Model
{
    public interface IEjerciciosRepository
    {
        void Add(EjerciciosModel ejerciciosModel);
        void Update(EjerciciosModel ejerciciosModel);
        void Delete(EjerciciosModel ejerciciosModel);
        EjerciciosModel GetById(string id);
        IEnumerable<EjerciciosModel> GetAllEjercicios();
    }
}
