
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Services.Repository.Interface;
using Newtonsoft.Json;
using System.Linq;
using Models;

namespace Services.Repository.Implementacion
{
    public class CompañiaRepository : ICompañiaRepository<Compania>
    {
        private readonly string? CadenaAS400;
        public CompañiaRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<List<Compania>> ObtenerCompañia()
        {
            var listaCompania = new List<Compania>();

            if (string.IsNullOrWhiteSpace(CadenaAS400))
            {
                Console.WriteLine("La cadena de conexión es nula o vacía");
                return listaCompania;
            }

            try
            {
                using var con = new OleDbConnection(CadenaAS400);
                await con.OpenAsync();

                using var cmd = new OleDbCommand("SP_API_OBTENER_COMPANIA", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (await lector.ReadAsync())
                {
                    var codCiaStr = lector[0]?.ToString()?.Trim();
                    int codCia = int.TryParse(codCiaStr, out var parsedCodCia) ? parsedCodCia : 0;

                    var desCia = lector[1]?.ToString()?.Trim() ?? string.Empty;

                    listaCompania.Add(new Compania
                    {
                        CodCia = codCia,
                        DesCia = desCia
                    });
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrames()?
                    .FirstOrDefault(f =>
                        !string.IsNullOrEmpty(f.GetFileName()) &&
                        f.GetILOffset() != StackFrame.OFFSET_UNKNOWN &&
                        f.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN &&
                        !f.GetMethod().Module.Assembly.GetName().Name.Contains("mscorlib"));

                if (frame != null)
                {
                    string machineName = Environment.MachineName;
                    string userName = Environment.UserName.ToUpperInvariant();
                    string mensaje = ex.Message;
                    int lineaError = frame.GetFileLineNumber();
                    string proyecto = frame.GetMethod().Module.Assembly.GetName().Name;
                    string clase = frame.GetMethod().DeclaringType?.Name ?? "Desconocido";
                    string metodo = frame.GetMethod().Name;
                    string codigoError = frame.GetHashCode().ToString();

                    // Aquí puedes loguear el error si es necesario
                }
            }

            return listaCompania;
        }



        public async Task<List<Compania>> ObtenerCompañia_x_Codigo(Compania comp)
        {
           


            List<Compania> ListaCompañia = new List<Compania>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_COMPANIA_X_CODIGO", con))
                        {
                           
                            cmd.Parameters.AddWithValue("@codcia", comp.CodCia);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Compania obj_BE = new Compania();
                                    obj_BE.CodCia = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.DesCia = lector[1].ToString().Trim();
                                  
                                    ListaCompañia.Add(obj_BE);
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
            return ListaCompañia;
        }
    }
}
