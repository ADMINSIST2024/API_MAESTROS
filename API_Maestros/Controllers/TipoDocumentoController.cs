using Dto.DtoInputs.DtosTipoMovimiento;
using Dto.DtoOuputs.DtoTipoDocumentoOutputs;
using Dto.DtoOuputs.DtoTipoMovimientoOutputs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Interface;

namespace API_Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentoController : ControllerBase
    {
      
        public IConfiguration _configuration;
        public readonly ITipoDocumentoRepository<TipoDocumento> _TipoMovRepository;

        public TipoDocumentoController(IConfiguration configuration, ITipoDocumentoRepository<TipoDocumento> TipoMovimientoRepository)
        {

            _TipoMovRepository = TipoMovimientoRepository;
            _configuration = configuration;

        }
        [HttpPost]
        [Route("ObtenerTipoMovSalida")]
        //[Authorize]
        public async Task<IActionResult> ObtenerTipoMovSalida(TipoDocumentoDtoInputs CodAlmacen)
        {
            TipoDocumento objTipDoc = new TipoDocumento();
            objTipDoc.CodAlmacen = CodAlmacen.codigoAlmacen;

            List<TipoDocumentoDtoOutputs> obj_List_TipoMovDtoOutputs = new List<TipoDocumentoDtoOutputs>();
            List<TipoDocumento> obj_List_TipoDoc = new List<TipoDocumento>();
            try
            {


                obj_List_TipoDoc = await _TipoMovRepository.ObtenerTipoMovXCodAlmacen(objTipDoc);

                if (obj_List_TipoDoc == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Tipo de Documentos",
                        result = ""
                    });
                }
                else
                {

                    foreach (TipoDocumento tipmov in obj_List_TipoDoc)
                    {
                        TipoDocumentoDtoOutputs TipoDocDtoOutputs = new TipoDocumentoDtoOutputs();
                        TipoDocDtoOutputs.codTipDoc = tipmov.CodTipDoc;
                        TipoDocDtoOutputs.desTipDoc = tipmov.DesTipDoc;
                        obj_List_TipoMovDtoOutputs.Add(TipoDocDtoOutputs);

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
                message = "Tipo de Documentos Obtenidos",
                result = obj_List_TipoMovDtoOutputs
            });
        }


        [HttpGet]
        [Route("ObtenerTipoDocInput")]
        public async Task<IActionResult> ObtenerTipoDocInput()
        {


            List<ObtenerTipoDocInputDtoOuputs> obj_List_TipoDocInputDtoOutputs = new List<ObtenerTipoDocInputDtoOuputs>();
            List<TipoDocumento> obj_List_TipoDoc = new List<TipoDocumento>();
            try
            {


                obj_List_TipoDoc = await _TipoMovRepository.ObtenerTipoDocInput();

                if (obj_List_TipoDoc == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Tipo de Documentos",
                        result = ""
                    });
                }
                else
                {

                    foreach (TipoDocumento tip in obj_List_TipoDoc)
                    {
                        ObtenerTipoDocInputDtoOuputs ObtenerTipoDocInputDtoOuputs = new ObtenerTipoDocInputDtoOuputs();
                        ObtenerTipoDocInputDtoOuputs.codigoTipDoc = tip.CodTipDoc;

                        obj_List_TipoDocInputDtoOutputs.Add(ObtenerTipoDocInputDtoOuputs);

                    }
                    Console.WriteLine(obj_List_TipoDocInputDtoOutputs);

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Tipo de Documentos Obtenidos",
                result = obj_List_TipoDocInputDtoOutputs
            });
        }


    }
}