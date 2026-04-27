using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym2.Model
{
    public class ClienteModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? MembresiaId { get; set; } // Puede ser nulo según tu base de datos
    }
}
