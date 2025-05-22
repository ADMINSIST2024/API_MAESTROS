using Dto.DtoInputs.DtosCompañiaInputs;
using Dto.DtoOuputs.DtosCompañiaOutputs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace API_Maestros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
     
        public IConfiguration _configuration;
        public readonly ICompañiaRepository<Compania> _CompañiaRepository;
        public CompaniaController(IConfiguration configuration, ICompañiaRepository<Compania> CompañiaRepository)
        {

            _CompañiaRepository = CompañiaRepository;
            _configuration = configuration;

        }


        [HttpGet]
        [Route("ObtenerCompania")]
        [SwaggerOperation(Summary = "Obtiene lista de compañias",
                  Description = "Obtiene json de compañias.")]
        //[Authorize]
        public async Task<IActionResult> ObtenerCompania()
        {
            List<Compania> obj_compañia = new List<Compania>();
            List<CompañiaDtoOutputs> obj_compañiaDtoOutputs = new List<CompañiaDtoOutputs>();
            try
            {
                obj_compañia = await _CompañiaRepository.ObtenerCompañia();

                if (obj_compañia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Compañias",
                        result = ""
                    });
                }
                else
                {

                    foreach (Compania compañia in obj_compañia)
                    {
                        CompañiaDtoOutputs compañiaDtoOutputs = new CompañiaDtoOutputs();
                        compañiaDtoOutputs.CodCia = compañia.CodCia;
                        compañiaDtoOutputs.DesCia = compañia.DesCia;
                        obj_compañiaDtoOutputs.Add(compañiaDtoOutputs);
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
                message = "Compañias Obtenidas",
                result = obj_compañiaDtoOutputs
            });
        }


        [HttpPost]
        [Route("ObtenerCompañia_x_Codigo")]
        //[Authorize]
        public async Task<IActionResult> ObtenerCompañia_x_Codigo(CompañiaDtoInputs comp)
        {
            Compania objCompañia = new Compania();
            objCompañia.CodCia = comp.CodCia;

            List<CompañiaDtoOutputs> obj_compañiaDtoOutputs = new List<CompañiaDtoOutputs>();
            List<Compania> obj_compañia = new List<Compania>();
            try
            {


                obj_compañia = await _CompañiaRepository.ObtenerCompañia_x_Codigo(objCompañia);

                if (obj_compañia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Compañias",
                        result = ""
                    });
                }
                else
                {

                    foreach (Compania compañia in obj_compañia)
                    {
                        CompañiaDtoOutputs compañiaDtoOutputs = new CompañiaDtoOutputs();
                        compañiaDtoOutputs.CodCia = compañia.CodCia;
                        compañiaDtoOutputs.DesCia = compañia.DesCia;
                        obj_compañiaDtoOutputs.Add(compañiaDtoOutputs);
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
                message = "Compañias Obtenidas",
                result = obj_compañiaDtoOutputs
            });
        }

    }
}
