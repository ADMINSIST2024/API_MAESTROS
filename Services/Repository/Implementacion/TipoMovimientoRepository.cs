using Models;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Services.Repository.Implementacion
{
    public class TipoMovimientoRepository : ITipoMovimientoRepository<TipoMovimiento>
    {
        private readonly string? CadenaAS400;
        public TipoMovimientoRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<List<TipoMovimiento>> ObtenerTipoMovTransferencia()
        {
            List<TipoMovimiento> ListaTipoMov = new List<TipoMovimiento>();

            try
            {
                if (string.IsNullOrEmpty(CadenaAS400))
                {
                    Console.WriteLine("La cadena de conexión es nula o vacía");
                }
                else
                {
                    using (var con = new OleDbConnection(CadenaAS400))
                    {
                        con.Open();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_TIPO_MOV_TRANSFERENCIA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    TipoMovimiento obj_BE = new TipoMovimiento
                                    {
                                        codTipMov = lector[0].ToString().Trim(),
                                          desTipMov = lector[1].ToString().Trim()
                                    };

                                    ListaTipoMov.Add(obj_BE);
                                }

                                lector.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Obtenga el primer marco de pila (donde se originó la excepción)
                StackFrame? frame = new StackTrace(ex, true).GetFrame(0);

                // Capture detalles esenciales de error para el registro (con operador condicional nulo)
                string sourceMethod = frame?.GetMethod()?.Name;
                string sourceFile = frame?.GetFileName();
                int lineNumber = frame?.GetFileLineNumber() ?? -1; // Establece un valor predeterminado para lineNumber si es null
                string errorMessage = ex.Message;

                // Puedes registrar el error aquí o manejarlo según tus necesidades
                Console.WriteLine($"Error en {sourceMethod} (Archivo: {sourceFile}, Línea: {lineNumber}): {errorMessage}");
            }

            return ListaTipoMov;
        }
    }
}
