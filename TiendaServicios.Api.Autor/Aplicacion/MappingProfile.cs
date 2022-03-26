using AutoMapper;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class MappingProfile: Profile
    {
        //se registra el mapeo, esto aplica para cada tipo de clase negocio, ejm: libro, autor,etc.
        public MappingProfile() {
            CreateMap<AutorLibro, AutorLibroDto>();
        }
    }
}
