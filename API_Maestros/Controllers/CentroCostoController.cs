using Dto.DtoInputs.DtosCentroCostoInputs;
using Dto.DtoOuputs.DtosCentroCostoOutputs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace API_Maestros.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CentroCostoController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly ICentroCostoRepository<CentroCosto> _CentroCostoRepository;

        public CentroCostoController(IConfiguration configuration, ICentroCostoRepository<CentroCosto> CentroCostoRepository)
        {

            _CentroCostoRepository = CentroCostoRepository;
            _configuration = configuration;

        }


        [HttpGet]
        [Route("ObtenerCentroCostos")]
        [SwaggerOperation(Summary = "Obtiene Centro de costo",
                  Description = "Obtiene json de centro de costo.")]
        public async Task<IActionResult> ObtenerCentroCostos()
        {
            List<CentroCosto> obj_centroCostos = new List<CentroCosto>();
            List<CentroCostoDtoOutputs> obj_centroCostosDtoOutputs = new List<CentroCostoDtoOutputs>();
            try
            {
                obj_centroCostos = await _CentroCostoRepository.ObtenerCentroCostos();

                if (obj_centroCostos == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Centro de Costos",
                        result = ""
                    });
                }
                else
                {

                    foreach (CentroCosto obj in obj_centroCostos)
                    {
                        CentroCostoDtoOutputs obj_ccDtoOutputs = new CentroCostoDtoOutputs();
                        obj_ccDtoOutputs.codCentroCosto = obj.codCentroCosto;
                        obj_ccDtoOutputs.desCentroCosto = obj.desCentroCosto;
                        obj_centroCostosDtoOutputs.Add(obj_ccDtoOutputs);
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Centro de Costos Obtenidos",
                result = obj_centroCostosDtoOutputs
            });
        }


        [HttpPost]
        [Route("ObtenerCentroCostoXCodigo")]
        public async Task<IActionResult> ObtenerCentroCostoXCodigo(CentroCostoDtoInputs cc)
        {
            CentroCosto objcc = new CentroCosto();
            objcc.codAlmacen = cc.codAlmacen;


            List<CentroCosto> obj_ListaCentroCosto = new List<CentroCosto>();
            List<CentroCostoDtoOutputs> obj_CentroCostoDtoOutputs = new List<CentroCostoDtoOutputs>();
            try
            {
                obj_ListaCentroCosto = await _CentroCostoRepository.ObtenerCentroCostosXAlmacen(objcc);

                if (obj_ListaCentroCosto == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Almacen",
                        result = ""
                    });
                }
                else
                {

                    foreach (CentroCosto obj_L in obj_ListaCentroCosto)
                    {
                        CentroCostoDtoOutputs obj = new CentroCostoDtoOutputs();
                        obj.codCentroCosto = obj_L.codCentroCosto;
                        obj.desCentroCosto = obj_L.desCentroCosto;

                        obj_CentroCostoDtoOutputs.Add(obj);
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Centro de Costos Obtenidos",
                result = obj_CentroCostoDtoOutputs
            });
        }


    }
}

