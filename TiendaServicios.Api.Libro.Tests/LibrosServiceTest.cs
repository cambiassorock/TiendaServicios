using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        //con este metodo creariamos la data ficticia. El objeto A , es del paquete Genfu
        private IEnumerable<LibroMaterial> ObtenerDatosPrueba() {
            A.Configure<LibroMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(X => X.AutorLibro, () => { return Guid.NewGuid(); })
                .Fill(x => x.LibroMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibroMaterial>(30);
            lista[0].LibroMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibro> CrearContexto() 
        {
            var dataPrueba = ObtenerDatosPrueba().AsQueryable();

            //decirle al contexto que el repo ya no va ser sql server, sino los datos creados con Genfu. Como clase Entidad Entity Framework
            var dbSet = new Mock<DbSet<LibroMaterial>>();
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            //darle propiedad asincrona, por que es un metood asincrono,a los datos generados
            dbSet.As<IAsyncEnumerable<LibroMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibroMaterial>(dataPrueba.GetEnumerator()));

            //para poder hacer consultas sobre la coleccion. para probar el metodo que consulta por un libro. el filtro.
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibroMaterial>(dataPrueba.Provider));

            //definir el contexto con el dbset, que se configuro anteriormente para poder devolverlo.
            var contexto = new Mock<ContextoLibro>();
            contexto.Setup(x => x.LibroMaterial).Returns(dbSet.Object);

            return contexto;
        }

        [Fact]
        public async void GetLibrosxId() {
            //para poder hacer debug del metodo GetLibros y mirar el punto de iterrupcion
            //System.Diagnostics.Debugger.Launch();

            //agregar los mocks del contexto y del mapeo
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            var mockMapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.Ejecuta();
            request.LibroGuid = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mockMapper);

            var libroUnico = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libroUnico);
            Assert.True(libroUnico.LibroMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros() 
        {
            //para poder hacer debug del metodo GetLibros y mirar el punto de iterrupcion
            //System.Diagnostics.Debugger.Launch();


            //1. Emular la instancia de entity framework.
            //para emular las acciones y eventos de un objeto en un ambiente de unit test, usaremos un objeto de Moq
            //se debe incluir el paquete Moq

            //2.emular contexto
            var mockContexto = CrearContexto();

            //3. emular Mapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mockMapper = mapConfig.CreateMapper();

            //4. instanciar clase manejador de Consulta.cs y pasarle los Mock creados.
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mockMapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }


        [Fact]
        public async void GuardarLibro() {
            //para poder hacer debug del metodo GetLibros y mirar el punto de iterrupcion. solo utilizarlo en un metood a la vez.
            //System.Diagnostics.Debugger.Launch();

            //crea BDen memoria para pruebas
            var options = new DbContextOptionsBuilder<ContextoLibro>()
                .UseInMemoryDatabase(databaseName: "BDLibros")
                .Options ;

            var contexto = new ContextoLibro(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de microservicio";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }

    }
}
