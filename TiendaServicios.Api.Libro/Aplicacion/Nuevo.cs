using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        //estructura de los datos de envio-entrada
        public class Ejecuta : IRequest { 
            public string Titulo { get; set; }

            public DateTime? FechaPublicacion{ get; set; }

            public Guid? AutorLibro { get; set; }
        }

        //validaciones de campos entrada Api. se hereda de AbstractValidator y se le manda la clase que quiero validar
        public class EjecutaValidacion : AbstractValidator<Ejecuta> {

            public EjecutaValidacion() {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }

        }

        //manejador del evento, para ejecutar la transaccion
        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly ContextoLibro _contexto;

            public Manejador(ContextoLibro contexto) {
                _contexto = contexto;
            }

            //implemnetado del padre IRequestHandler. Devuelve un valor 1->exitoso o 0 errores
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                LibroMaterial libroMaterial = new LibroMaterial() {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro,
                };

                _contexto.LibroMaterial.Add(libroMaterial);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar el libro");
            }
        }
    }
}
