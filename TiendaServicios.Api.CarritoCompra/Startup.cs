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
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteService;

namespace TiendaServicios.Api.CarritoCompra
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
            services.AddDbContext<ContextoCarrito>(options => {
                options.UseMySQL(Configuration.GetConnectionString("ConexionDatabase"));
            });

            //para poder usar mediatr de CQRS con injeccion. al agregarlo una vez, ya no es necesario para otras clases.
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

            //add hhtp client para consumir servicio Libros por http
            services.AddHttpClient("Libros", config =>{
                config.BaseAddress = new Uri(Configuration["services:Libros"]);
            });

            //add hhtp client para consumir servicio Libros por http
            services.AddHttpClient("Autores", config => {
                config.BaseAddress = new Uri(Configuration["services:Autores"]);
            });

            //add IService interface
            services.AddScoped<ILibrosService, LibrosService>();
            services.AddScoped<IAutoresService, AutoresService>();

            services.AddControllers();
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
        }
    }
}
