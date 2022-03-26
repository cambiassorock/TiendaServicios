using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        //estructura de los datos de envio-entrada
        public class Ejecuta : IRequest { 
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public DateTime? FechaNacimiento { get; set; }
        }

        //validaciones de campos entrada Api. se hereda de AbstractValidator y se le manda la clase que quiero valdair
        public class EjecutaValidacion : AbstractValidator<Ejecuta> {

            public EjecutaValidacion() {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }

        }

        //manejador del evento, para ejecutar la transaccion
        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly ContextoAutor _contexto;

            public Manejador(ContextoAutor contexto) {
                _contexto = contexto;
            }

            //implemnetado del padre IRequestHandler. Devuelve un valor 1->exitoso o 0 errores
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                AutorLibro autorLibro = new AutorLibro() {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Guid.NewGuid().ToString()
                };

                _contexto.AutorLibro.Add(autorLibro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar el autor");
            }
        }
    }
}
