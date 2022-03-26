using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {
        //IRequest<List<LibroMaterialDto>> indica que retorna una lista de libros
        public class Ejecuta : IRequest<List<LibroMaterialDto>> { }

        //IRequestHandler<ListaLibro, List<LibroMaterialDto>> indica lo que va retornar, coma  y su tipo
        public class Manejador : IRequestHandler<Ejecuta, List<LibroMaterialDto>>
        {
            private readonly ContextoLibro _contexto;

            private readonly IMapper _mapper;

            public Manejador(ContextoLibro contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async  Task<List<LibroMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibroMaterial.ToListAsync();
                var librosDto = _mapper.Map<List<LibroMaterial>, List<LibroMaterialDto>>(libros);

                return librosDto;
            }
        }

    }
}
