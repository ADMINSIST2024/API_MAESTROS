using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface ITipoDocumentoRepository<T> where T : class
    {
        Task<List<T>> ObtenerTipoMovXCodAlmacen(T clase);

        Task<List<T>> ObtenerTipoDocInput();
    }
}
