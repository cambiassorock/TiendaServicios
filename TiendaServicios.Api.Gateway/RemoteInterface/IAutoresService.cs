using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.RemoteModel;

namespace TiendaServicios.Api.Gateway.InterfaceRemote
{
    public interface IAutoresService
    {
        Task<(bool resultado, AutorModeloRemote autorLibroRemote, string ErrorMessage)> GetAutor(Guid? LibroId);
    }
}
