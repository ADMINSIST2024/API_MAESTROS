
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
    public class CentroCostoRepository : ICentroCostoRepository<CentroCosto>
    {
        private readonly string? CadenaAS400;
        public CentroCostoRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<List<CentroCosto>> ObtenerCentroCostos()
        {
            var listaCentroCosto = new List<CentroCosto>();

            try
            {
                if (string.IsNullOrEmpty(CadenaAS400))
                {
                    return listaCentroCosto; // Retorna lista vacía si la cadena de conexión es nula
                }

                using var con = new OleDbConnection(CadenaAS400);
                await con.OpenAsync();

                using var cmd = new OleDbCommand("SP_API_OBTENER_CENTRO_COSTOS", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (await lector.ReadAsync())
                {
                    listaCentroCosto.Add(new CentroCosto
                    {
                        codCentroCosto = Convert.ToInt32(lector[0]?.ToString()?.Trim() ?? "0"),
                        desCentroCosto = lector[1]?.ToString()?.Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrames()?.FirstOrDefault(f =>
                    !string.IsNullOrEmpty(f.GetFileName()) &&
                    f.GetILOffset() != StackFrame.OFFSET_UNKNOWN &&
                    f.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN &&
                    !f.GetMethod().Module.Assembly.GetName().Name.Contains("mscorlib"));

                if (frame != null)
                {
                    string MachineName = Environment.MachineName;
                    string UserName = Environment.UserName.ToUpper();
                    string Mensaje = ex.Message;
                    int LineaError = frame.GetFileLineNumber();
                    string Proyecto = frame.GetMethod().Module.Assembly.GetName().Name;
                    string Clase = frame.GetMethod().DeclaringType.Name;
                    string Metodo = frame.GetMethod().Name;
                    string CodigoError = frame.GetHashCode().ToString();
                }
            }

            return listaCentroCosto;
        }

        public async Task<List<CentroCosto>> ObtenerCentroCostosXAlmacen(CentroCosto clase)
        {
            var listaCentroCosto = new List<CentroCosto>();

            try
            {
                if (string.IsNullOrEmpty(CadenaAS400))
                {
                    return listaCentroCosto; // Retorno seguro si la cadena de conexión es nula
                }

                using var con = new OleDbConnection(CadenaAS400);
                await con.OpenAsync();

                using var cmd = new OleDbCommand("SP_API_OBTENER_CENTRO_COSTOS_X_ALMACEN", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CODIGO_ALMACEN", clase.codAlmacen);

                using var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                while (await lector.ReadAsync())
                {
                    listaCentroCosto.Add(new CentroCosto
                    {
                        codCentroCosto = Convert.ToInt32(lector[0]?.ToString()?.Trim() ?? "0"),
                        desCentroCosto = lector[1]?.ToString()?.Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrames()?.FirstOrDefault(f =>
                    !string.IsNullOrEmpty(f.GetFileName()) &&
                    f.GetILOffset() != StackFrame.OFFSET_UNKNOWN &&
                    f.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN &&
                    !f.GetMethod().Module.Assembly.GetName().Name.Contains("mscorlib"));

                if (frame != null)
                {
                    string MachineName = Environment.MachineName;
                    string UserName = Environment.UserName.ToUpper();
                    string Mensaje = ex.Message;
                    int LineaError = frame.GetFileLineNumber();
                    string Proyecto = frame.GetMethod().Module.Assembly.GetName().Name;
                    string Clase = frame.GetMethod().DeclaringType.Name;
                    string Metodo = frame.GetMethod().Name;
                    string CodigoError = frame.GetHashCode().ToString();
                }
            }

            return listaCentroCosto;
        }


    }
}
