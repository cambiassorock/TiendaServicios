using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class AutoresService : IAutoresService
    {

        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<AutoresService> _logger;

        public AutoresService(IHttpClientFactory httpClient, ILogger<AutoresService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool resultado, AutorLibroRemote autorLibroRemote, string ErrorMessage)> GetAutor(Guid? AutorId)
        {
            try
            {
                var cliente = _httpClient.CreateClient("Autores");
                var response = await cliente.GetAsync($"api/autor/{AutorId.ToString()}");

                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var resultado = JsonSerializer.Deserialize<AutorLibroRemote>(contenido, options);
                    return (true, resultado, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return (false, null, e.ToString());
            }
        }
    }
}
