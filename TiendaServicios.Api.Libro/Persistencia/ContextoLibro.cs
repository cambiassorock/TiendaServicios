using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibro : DbContext
    {

        //para poder ejecutar la simulacion de prueba unitaria
        public ContextoLibro() { } 

        public ContextoLibro(DbContextOptions<ContextoLibro> options) : base(options) { }

        //agregar las clase como tipo entidad que se van  a usar como tablas
        public virtual DbSet<LibroMaterial> LibroMaterial { get; set; }
        //se coloca como virtual para que se pueda sobreescribir a futuro el metodo. para poder ejecutar la simulacion de prueba unitaria
    }
}
