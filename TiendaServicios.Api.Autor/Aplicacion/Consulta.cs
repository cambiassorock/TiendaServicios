using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        //IRequest<List<AutorLibro>> indica que retorna una lista de autores
        public class ListaAutor : IRequest<List<AutorLibroDto>> { }

        //IRequestHandler<ListaAutor, List<AutorLibro>> indica lo que va retornar, coma  y su tipo
        public class Manejador : IRequestHandler<ListaAutor, List<AutorLibroDto>>
        {
            private readonly ContextoAutor _contexto;

            private readonly IMapper _mapper;

            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async  Task<List<AutorLibroDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores = await _contexto.AutorLibro.ToListAsync();
                var autoresDto = _mapper.Map<List<AutorLibro>, List<AutorLibroDto>>(autores);

                return autoresDto;
            }
        }

    }
}
