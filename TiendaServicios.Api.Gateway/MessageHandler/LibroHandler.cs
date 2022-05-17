using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.RemoteModel;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;
        private readonly IAutoresService _autoresService;

        public LibroHandler(ILogger<LibroHandler> logger, IAutoresService autoresService) {
            _logger = logger;
            _autoresService = autoresService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {

            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");
            var response = await base.SendAsync(request, cancellationToken);

            if(response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
                var resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);

                //ahora debemos llamar al ssrvicio de autor, para traer el nombre del libro.
                var responseAutor = await _autoresService.GetAutor(resultado.AutorLibro ?? Guid.Empty);

                if (responseAutor.resultado)
                {
                    var objAutor = responseAutor.autorLibroRemote;
                    resultado.AutorData = objAutor;
                    var resultadoStr = JsonSerializer.Serialize(resultado); //se prepara el nuevo contenido de la respuesta manipulada
                    response.Content = new StringContent(resultadoStr, System.Text.Encoding.UTF8,"application/json");
                }
            }

            _logger.LogInformation($"Este proceso se hizo en {tiempo.ElapsedMilliseconds}ms");

            return response;
        }

    }
}
