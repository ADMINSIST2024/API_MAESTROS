
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
    public class TipoDocumentoRepository : ITipoDocumentoRepository<TipoDocumento>
    {
        private readonly string? CadenaAS400;
        public TipoDocumentoRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<List<TipoDocumento>> ObtenerTipoDocInput()
        {
            List<TipoDocumento> ListaTipoMov = new List<TipoDocumento>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_TIPODOC_INPUT", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    TipoDocumento obj_BE = new TipoDocumento
                                    {
                                        CodTipDoc = lector[0].ToString().Trim()
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

        public async Task<List<TipoDocumento>> ObtenerTipoMovXCodAlmacen(TipoDocumento TipMov)
        {
            List<TipoDocumento> ListaTipoMov = new List<TipoDocumento>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_TIPO_MOV_SALIDA", con))
                        {

                            cmd.Parameters.AddWithValue("@COD_ALM", TipMov.CodAlmacen);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    TipoDocumento obj_BE = new TipoDocumento();
                                    obj_BE.CodTipDoc = lector[0].ToString().Trim();
                                    obj_BE.DesTipDoc = lector[1].ToString().Trim();

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
            }
            return ListaTipoMov;
        }



    }
}
