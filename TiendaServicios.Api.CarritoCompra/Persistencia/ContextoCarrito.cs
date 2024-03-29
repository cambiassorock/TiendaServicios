﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;

namespace TiendaServicios.Api.CarritoCompra.Persistencia
{
    public class ContextoCarrito : DbContext
    {

        public ContextoCarrito(DbContextOptions<ContextoCarrito> options) : base(options) { }

        //agregar las clase como tipo entidad que se van  a usar como tablas
        public DbSet<CarritoSesion> CarritoSesion { get; set; }

        public DbSet<CarritoSesionDetalle> CarritoSesionDetalle { get; set; }
    }
}
