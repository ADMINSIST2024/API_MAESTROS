using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface ITipoMovimientoRepository<T> where T : class
    {

        Task<List<T>> ObtenerTipoMovTransferencia();
    }
}
