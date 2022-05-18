using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<IEventoManejador> _logger;

        public EmailEventoManejador()
        {
        }

        public EmailEventoManejador(ILogger<IEventoManejador> logger)
        {
            _logger = logger;
        }

        public Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"Este es el valor que consumo desde Rabbit MQ Destiantario: {@event.Destinatario}");
            _logger.LogInformation($"Este es el valor que consumo desde Rabbit MQ Titulo: {@event.Titulo}");
            _logger.LogInformation($"Este es el valor que consumo desde Rabbit MQ Contenido: {@event.Contenido}");

            return Task.CompletedTask;
        }
    }
}
