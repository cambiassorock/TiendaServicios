using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        //estructura de los datos de envio-entrada
        public class Ejecuta : IRequest
        {
            public DateTime? FechaCreacionSesion{ get; set; }

            public List<string> ProductoLista { get; set; }
        }

        //manejador del evento, para ejecutar la transaccion
        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly ContextoCarrito _contexto;

            public Manejador(ContextoCarrito contexto)
            {
                _contexto = contexto;
            }

            //implemnetado del padre IRequestHandler. Devuelve un valor 1->exitoso o 0 errores
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var  carritosesion = new CarritoSesion()
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carritosesion);

                var valor = await _contexto.SaveChangesAsync();

                if (valor == 0)
                {
                    throw new Exception("No se pudo insertar la cabecera de productos");
                }

                //obtenemos el id autogenerado
                int idSesion = carritosesion.CarritoSesionId;

                foreach (var obj in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle()
                    {
                        CarritoSesionId = idSesion,
                        FechaCreacion = DateTime.Now,
                        ProductoSelecionado = obj
                    };

                    _contexto.CarritoSesionDetalle.Add(detalleSesion);
                }

                valor = await _contexto.SaveChangesAsync();
                if (valor == 0)
                {
                    throw new Exception("No se pudo insertar el detalle de productos");
                }

                return Unit.Value;
            }

        }
    }
}
