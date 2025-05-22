using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface ICentroCostoRepository<T> where T : class
    {
        Task<List<T>> ObtenerCentroCostos();

        Task<List<T>> ObtenerCentroCostosXAlmacen(T clase);
    }
}
