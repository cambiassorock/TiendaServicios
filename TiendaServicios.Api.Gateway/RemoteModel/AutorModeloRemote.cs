using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Gateway.RemoteModel
{
    public class AutorModeloRemote
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        //representacion unica autor
        public string AutorLibroGuid { get; set; }
    }
}
