using Dto.DtoOuputs.DtosTipoMovimientoOutputs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Interface;

namespace API_Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMovimientoController : ControllerBase
    {
        
        public IConfiguration _configuration;
        public readonly ITipoMovimientoRepository<TipoMovimiento> _TipoMovRepository;
        public TipoMovimientoController(IConfiguration configuration, ITipoMovimientoRepository<TipoMovimiento> TipoMovimientoRepository)
        {

            _TipoMovRepository = TipoMovimientoRepository;
            _configuration = configuration;

        }
        [HttpGet]
        [Route("ObtenerTipoMovTransferencia")]
        //[Authorize]
        public async Task<IActionResult> ObtenerTipoMovTransferencia()
        {

            List<ObtenerTipoMovTransferenciaDtoOutputs> obj_List_TipoMovDtoOutputs = new List<ObtenerTipoMovTransferenciaDtoOutputs>();
            List<TipoMovimiento> obj_List_TiMov = new List<TipoMovimiento>();
            try
            {


                obj_List_TiMov = await _TipoMovRepository.ObtenerTipoMovTransferencia();

                if (obj_List_TiMov == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Tipo de Movimientos",
                        result = ""
                    });
                }
                else
                {

                    foreach (TipoMovimiento tipmov in obj_List_TiMov)
                    {
                        ObtenerTipoMovTransferenciaDtoOutputs obj = new ObtenerTipoMovTransferenciaDtoOutputs();
                        obj.codTipMov = tipmov.codTipMov;
                        obj.desTipMov = tipmov.desTipMov;
                        obj_List_TipoMovDtoOutputs.Add(obj);

                    }
                    Console.WriteLine(obj_List_TipoMovDtoOutputs);

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Tipo de Movimiento Obtenidos",
                result = obj_List_TipoMovDtoOutputs
            });
        }




    }
}

