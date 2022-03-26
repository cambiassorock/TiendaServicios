using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        //IRequest<List<LibroMaterialDto>> indica que retorna una lista de libros
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        //IRequestHandler<ListaLibro, List<LibroMaterialDto>> indica lo que va retornar, coma  y su tipo
        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly ContextoCarrito _contexto;
            private readonly ILibrosService _librosService;
            private readonly IAutoresService _autoresService;

            public Manejador(ContextoCarrito contexto, ILibrosService librosService, IAutoresService autoresService)
            {
                _contexto = contexto;
                _librosService = librosService;
                _autoresService = autoresService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionDetalleId == request.CarritoSesionId).ToListAsync();

                List<CarritoDetalleDto> listaProductos = new List<CarritoDetalleDto>();

                foreach (var libro in carritoSesionDetalle)
                {

                    //llamamos a servicio para saber el nombre del libro
                    var responseLibro = await _librosService.GetLibro(new Guid(libro.ProductoSelecionado));
                    if (responseLibro.resultado)
                    {
                        var objetoLibro = responseLibro.libroRemote;

                        //llamamos a servicio para saber el nombre del autor
                        var responseAutor = await _autoresService.GetAutor(objetoLibro.AutorLibro);
                        if (responseAutor.resultado)
                        {
                            var objetoAutor = responseAutor.autorLibroRemote;

                            var carritoDetalle = new CarritoDetalleDto()
                            {
                                LibroId = objetoLibro.LibroMaterialId,
                                FechaPublicacion = objetoLibro.FechaPublicacion,
                                TituloLibro = objetoLibro.Titulo,
                                AutorLibro = $"{objetoAutor.Nombre} {objetoAutor.Apellido}"
                            };
                            listaProductos.Add(carritoDetalle);
                        }
                    }
                }

                var carritoSesionDto = new CarritoDto()
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaProductos
                };

                return carritoSesionDto;
            }
        }
    }
}
