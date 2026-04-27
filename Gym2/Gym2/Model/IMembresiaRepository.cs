using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Model
{
    public interface IMembresiaRepository
    {
        void Add(MembresiaModel membresiaModel);
        void Update(MembresiaModel membresiaModel);
        void Delete(MembresiaModel membresiaModel);
        MembresiaModel GetById(string id);
        IEnumerable<MembresiaModel> GetAllMembresias();
    }
}
