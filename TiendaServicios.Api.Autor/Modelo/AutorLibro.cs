using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Modelo
{
    public class AutorLibro
    {
        public int AutorLibroId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        
        //lista de grados
        public ICollection<GradoAcademico> ListaGradoAcademico { get; set; }

        //representacion unica autor
        public string AutorLibroGuid { get; set; }

    }
}
