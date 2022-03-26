using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {
        public class Ejecuta : IRequest<LibroMaterialDto>
        {
            public Guid LibroGuid { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, LibroMaterialDto>
        {
            private readonly ContextoLibro _contexto;

            private readonly IMapper _mapper;

            public Manejador(ContextoLibro contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<LibroMaterialDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = await  _contexto.LibroMaterial.Where(x =>  x.LibroMaterialId == request.LibroGuid).FirstOrDefaultAsync();
                if (libro == null)
                    throw new Exception("No se encontro el autor");

                var libroDto = _mapper.Map<LibroMaterial, LibroMaterialDto>(libro);
                return libroDto;
            }
        }
    }
}