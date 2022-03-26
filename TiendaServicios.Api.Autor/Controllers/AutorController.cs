using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Controllers
{
    [ApiController]
    [Route("api/autor")]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;


        public AutorController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data) {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorLibroDto>>> GetAutores() {
            return await _mediator.Send(new Consulta.ListaAutor());
        }

        [HttpGet("{idGuid}")]
        public async Task<ActionResult<AutorLibroDto>> GetAutoresxGuid(string idGuid)
        {
            return await _mediator.Send(new ConsultaFiltro.AutorUnico() { AutorGuid = idGuid});
        }
    }
}
