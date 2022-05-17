using System;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteInterface
{
    public interface IAutoresService
    {
        Task<(bool resultado, AutorLibroRemote autorLibroRemote, string ErrorMessage)> GetAutor(Guid? LibroId);
    }
}
