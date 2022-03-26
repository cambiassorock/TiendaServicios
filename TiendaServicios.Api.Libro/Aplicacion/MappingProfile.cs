using AutoMapper;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class MappingProfile: Profile
    {
        //se registra el mapeo, esto aplica para cada tipo de clase negocio, ejm: libro, autor,etc.
        public MappingProfile() {
            CreateMap<LibroMaterial, LibroMaterialDto>();
        }
    }
}
