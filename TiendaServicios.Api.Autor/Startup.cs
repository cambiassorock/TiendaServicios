using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.ManejadorRabbit;
using TiendaServicios.Api.Autor.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;
using TiendaServicios.RabbitMQ.Bus.Implement;

namespace TiendaServicios.Api.Autor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //para poder usar entity framework con injeccion
            services.AddDbContext<ContextoAutor>(options => {
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDatabase"));
            });

            //para poder usar mediatr de CQRS con injeccion. al agregarlo una vez, ya no es necesario para otras clases.
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

            //a los controladores se agrega AddFluentValidation para incicializar el Fluent. al agregarlo una vez, ya no es necesario para otras clases.
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //para poder usar Mapper con injeccion
            services.AddAutoMapper(typeof(Consulta.Manejador).Assembly);

            //Para poder utilizar Rabbit MQ Event, del proyecto RabbitMQ.Bus. 
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scoperFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scoperFactory);
            }); //con esto ya se puede injectar dentro de cualquier clase.
            services.AddTransient<EmailEventoManejador>();

            //Para poder utilizar Rabbit MQ Event, para escuchar en el tubo, y tomar el mensaje evento
            services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>(); //con esto ya se puede injectar dentro de cualquier clase.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Para poder utilizar Rabbit MQ Event, para escuchar en el tubo, y tomar el mensaje evento.
            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Suscribe<EmailEventoQueue, EmailEventoManejador>();
        }
    }
}
