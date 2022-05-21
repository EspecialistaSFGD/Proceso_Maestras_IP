﻿using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NextSIT.Utility;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProcesarMaestras
{
    public class Repositorio
    {
        private readonly string Conexion = "";
        private readonly ProxyManager proxyManager;
        private readonly TypeConvertionManager typeConvertionsManager;
        private readonly int TiempoEsperaCargadoMasivo;
        private readonly int BatchSize;

        public Repositorio(string conexion)
        {
            Conexion = conexion;
            proxyManager = ProxyManager.GetNewProxyManager();
            typeConvertionsManager = TypeConvertionManager.GetNewTypeConvertionManager();
            TiempoEsperaCargadoMasivo = 10000;
            BatchSize = 50000;
        }

        //============================================================
        //                         COMPONENTE
        //============================================================
        //Paso 1.- Elimina los componentes del año actual
        public async Task<string> EliminarComponentes()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01A_EliminarComponente", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 2.- Obtiene los componentes para el año actual desde el servicio Web
        public async Task<bool> ObtenerComponentes(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaComponente = typeConvertionsManager.XmlStringToObject<RespuestaComponente>(respuesta.ResponseBody, "ArrayOfComponente");
                Console.WriteLine($"Se han recuperado los componentes desde el servicio del MEF. Numero de Componentes => {respuestaComponente.Componentes.Count}");
                var componentesDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaComponente.Componentes));
                return RegistrarComponentesPorLotes(componentesDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de componentes.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 3.- Registrar los Componentes masivamente
        public bool RegistrarComponentesPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_COMPONENTE";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                         ESPECIFICA
        //============================================================
        //Paso 4.- Elimina las especificas del año actual
        public async Task<string> EliminarEspecificas()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01B_EliminarEspecifica", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 5.- Obtiene las especificas para el año actual desde el servicio Web
        public async Task<bool> ObtenerEspecificas(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaEspecificas = typeConvertionsManager.XmlStringToObject<RespuestaEspecifica>(respuesta.ResponseBody, "ArrayOfEspecifica");
                Console.WriteLine($"Se han recuperado las especificas desde el servicio del MEF. Numero de Especificas => {respuestaEspecificas.Especificas.Count}");
                var especificasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaEspecificas.Especificas));
                return RegistrarEspecificasPorLotes(especificasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de especificas.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 6.- Registrar las especificas masivamente
        public bool RegistrarEspecificasPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_ESPECIFICA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                     ESPECIFICA_DETALLE
        //============================================================
        //Paso 7.- Elimina los detalles de especificas del año actual
        public async Task<string> EliminarEspecificasDetalle()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01C_EliminarEspecificaDetalle", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 8.- Obtiene los detalles de especificas para el año actual desde el servicio Web
        public async Task<bool> ObtenerEspecificasDetalle(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaEspecificasDetalle = typeConvertionsManager.XmlStringToObject<RespuestaEspecificaDetalle>(respuesta.ResponseBody, "ArrayOfEspecifica_det");
                Console.WriteLine($"Se han recuperado los detalles de las especificas desde el servicio del MEF. Numero de detalles de Especificas => {respuestaEspecificasDetalle.EspecificasDetalle.Count}");
                var especificasDetalleDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaEspecificasDetalle.EspecificasDetalle));
                return RegistrarEspecificasDetallePorLotes(especificasDetalleDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de detalle de especificas.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 9.- Registrar los detalles de especificas masivamente
        public bool RegistrarEspecificasDetallePorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_ESPECIFICA_DET";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                          FUENTE
        //============================================================
        //Paso 10.- Elimina las fuentes del año actual
        public async Task<string> EliminarFuentes()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01D_EliminarFuente", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 11.- Obtiene las fuentes para el año actual desde el servicio Web
        public async Task<bool> ObtenerFuentes(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaFuentes = typeConvertionsManager.XmlStringToObject<RespuestaFuente>(respuesta.ResponseBody, "ArrayOfFuente");
                Console.WriteLine($"Se han recuperado las fuentes desde el servicio del MEF. Numero de Fuentes => {respuestaFuentes.Fuentes.Count}");
                var FuentesDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaFuentes.Fuentes));
                return RegistrarFuentesePorLotes(FuentesDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de fuentes.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 12.- Registrar las fuentes masivamente
        public bool RegistrarFuentesePorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_FUENTE";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                        FUNCION
        //============================================================
        //Paso 13.- Elimina las funciones del año actual
        public async Task<string> EliminarFuncion()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01E_EliminarFuncion", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 14.- Obtiene las funciones para el año actual desde el servicio Web
        public async Task<bool> ObtenerFunciones(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaFunciones = typeConvertionsManager.XmlStringToObject<RespuestaFuncion>(respuesta.ResponseBody, "ArrayOfFuncion");
                Console.WriteLine($"Se han recuperado las funciones desde el servicio del MEF. Numero de Funciones => {respuestaFunciones.Funciones.Count}");
                var funcionesDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaFunciones.Funciones));
                return RegistrarFuncionesPorLotes(funcionesDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de funciones.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 15.- Registrar las funciones masivamente
        public bool RegistrarFuncionesPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_FUNCION";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                        GENERICA
        //============================================================
        //Paso 13.- Elimina las genericas del año actual
        public async Task<string> EliminarGenerica()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01F_EliminarGenerica", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 14.- Obtiene las genericas para el año actual desde el servicio Web
        public async Task<bool> ObtenerGenericas(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaGenericas = typeConvertionsManager.XmlStringToObject<RespuestaGenerica>(respuesta.ResponseBody, "ArrayOfGenerica");
                Console.WriteLine($"Se han recuperado las genericas desde el servicio del MEF. Numero de Genericas => {respuestaGenericas.Genericas.Count}");
                var genericasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaGenericas.Genericas));
                return RegistrarGenericasPorLotes(genericasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de genericas.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 15.- Registrar las genericas masivamente
        public bool RegistrarGenericasPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_GENERICA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                        PLIEGO
        //============================================================
        //Paso 16.- Elimina los pliegos del año actual
        public async Task<string> EliminarPliego()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01G_EliminarPliego", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 17.- Obtiene los pliegos para el año actual desde el servicio Web
        public async Task<bool> ObtenerPliegos(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaPliegos = typeConvertionsManager.XmlStringToObject<RespuestaPliego>(respuesta.ResponseBody, "ArrayOfItemPliego");
                Console.WriteLine($"Se han recuperado los pliegos desde el servicio del MEF. Numero de Pliegos => {respuestaPliegos.Pliegos.Count}");
                //corregir datos sin nombre
                respuestaPliegos.Pliegos = respuestaPliegos.Pliegos
                    .Select(
                    pliego =>
                    {
                        if (string.IsNullOrEmpty(pliego.DESCRIPCION_PLIEGO)) pliego.DESCRIPCION_PLIEGO = "PLIEGO SIN NOMBRE";
                        return pliego;
                    }).ToList();
                var genericasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaPliegos.Pliegos));
                return RegistrarPliegosPorLotes(genericasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de pliegos.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 18.- Registrar los pliegos masivamente
        public bool RegistrarPliegosPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_PLIEGO";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                        PROGRAMA
        //============================================================
        //Paso 19.- Elimina los programas del año actual
        public async Task<string> EliminarPrograma()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01H_EliminarPrograma", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 20.- Obtiene los programas para el año actual desde el servicio Web
        public async Task<bool> ObtenerProgramas(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaProgramas = typeConvertionsManager.XmlStringToObject<RespuestaPrograma>(respuesta.ResponseBody, "ArrayOfProgramaNombre");
                Console.WriteLine($"Se han recuperado los programas desde el servicio del MEF. Numero de Programas => {respuestaProgramas.Programas.Count}");
                var programasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaProgramas.Programas));
                return RegistrarProgramasPorLotes(programasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de programas.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 21.- Registrar los programas masivamente
        public bool RegistrarProgramasPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_PROGRAMA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }


        //============================================================
        //                   PROGRAMA_PRESUPUESTO
        //============================================================
        //Paso 22.- Elimina los programas presupuesto del año actual
        public async Task<string> EliminarProgramaPresupuesto()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01I_EliminarProgramaPresupuesto", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 23.- Obtiene los programas presupuesto para el año actual desde el servicio Web
        public async Task<bool> ObtenerProgramasPresupuesto(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaProgramasPresupuesto = typeConvertionsManager.XmlStringToObject<RespuestaProgramaPresupuesto>(respuesta.ResponseBody, "ArrayOfPrograma_PPTO_nombre");
                Console.WriteLine($"Se han recuperado los programas presupuesto desde el servicio del MEF. Numero de Programas Presupuesto => {respuestaProgramasPresupuesto.ProgramasPresupuesto.Count}");
                var programasPptoDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaProgramasPresupuesto.ProgramasPresupuesto));
                return RegistrarProgramasPresupuestoPorLotes(programasPptoDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de programas presupuesto.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 24.- Registrar los programas masivamente
        public bool RegistrarProgramasPresupuestoPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_PROGRAMA_PPTO";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }


        //============================================================
        //                           SECTOR
        //============================================================
        //Paso 25.- Elimina los sectores del año actual
        public async Task<string> EliminarSector()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01J_EliminarSector", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 26.- Obtiene los sectores para el año actual desde el servicio Web
        public async Task<bool> ObtenerSectores(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaSector = typeConvertionsManager.XmlStringToObject<RespuestaSector>(respuesta.ResponseBody, "ArrayOfSector");
                Console.WriteLine($"Se han recuperado los sectores desde el servicio del MEF. Numero de Sectores => {respuestaSector.Sectores.Count}");
                var sectoresDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaSector.Sectores));
                return RegistrarSectoresPorLotes(sectoresDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de sectores.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 27.- Registrar los sectores masivamente
        public bool RegistrarSectoresPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_SECTOR";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                      SUBGENERICA
        //============================================================
        //Paso 28.- Elimina las subgenericas del año actual
        public async Task<string> EliminarSubgenerica()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01K_EliminarSubgenerica", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 29.- Obtiene las subgenericas para el año actual desde el servicio Web
        public async Task<bool> ObtenerSubgenericas(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaSubgenericas = typeConvertionsManager.XmlStringToObject<RespuestaSubgenerica>(respuesta.ResponseBody, "ArrayOfSubgenerica");
                Console.WriteLine($"Se han recuperado las subgenericas desde el servicio del MEF. Numero de Subgenericas => {respuestaSubgenericas.Subgenericas.Count}");
                var genericasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaSubgenericas.Subgenericas));
                return RegistrarSubgenericasPorLotes(genericasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de subgenericas.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 30.- Registrar las subgenericas masivamente
        public bool RegistrarSubgenericasPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_SUBGENERICA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                   SUBGENERICA_DETALLE
        //============================================================
        //Paso 30.- Elimina las subgenericas detalle del año actual
        public async Task<string> EliminarSubgenericaDetalle()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01L_EliminarSubgenericaDetalle", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 31.- Obtiene las subgenericas detalle para el año actual desde el servicio Web
        public async Task<bool> ObtenerSubgenericasDetalle(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaSubgenericasDetalle = typeConvertionsManager.XmlStringToObject<RespuestaSubgenericaDetalle>(respuesta.ResponseBody, "ArrayOfSubgenerica_det");
                Console.WriteLine($"Se han recuperado las subgenericas detalle desde el servicio del MEF. Numero de Subgenericas Detalle => {respuestaSubgenericasDetalle.SubgenericasDetalle.Count}");
                var genericasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaSubgenericasDetalle.SubgenericasDetalle));
                return RegistrarSubgenericasDetallePorLotes(genericasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de subgenericas detalle.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 32.- Registrar las subgenericas detalle masivamente
        public bool RegistrarSubgenericasDetallePorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_SUBGENERICA_DET";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                       TIPO_RECURSO
        //============================================================
        //Paso 33.- Elimina los Tipos de Recurso del año actual
        public async Task<string> EliminarTipoRecurso()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01M_EliminarTipoRecurso", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 34.- Obtiene los tipos de recurso para el año actual desde el servicio Web
        public async Task<bool> ObtenerTiposRecurso(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaTiposRecursos = typeConvertionsManager.XmlStringToObject<RespuestaTipoRecurso>(respuesta.ResponseBody, "ArrayOfTipoRecurso");
                Console.WriteLine($"Se han recuperado los tipos de recursos desde el servicio del MEF. Numero de Tipos Recurso => {respuestaTiposRecursos.TiposRecurso.Count}");
                var tiposRecursosDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaTiposRecursos.TiposRecurso));
                return RegistrarTiposRecursosPorLotes(tiposRecursosDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de tipos de recursos.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 35.- Registrar los tipos de recursos masivamente
        public bool RegistrarTiposRecursosPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_TIPO_RECURSO";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                       UNIDAD_MEDIDA
        //============================================================
        //Paso 36.- Elimina las unidades de medida del año actual
        public async Task<string> EliminarUnidadMedida()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01N_EliminarUnidadMedida", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 37.- Obtiene las unidades de medida para el año actual desde el servicio Web
        public async Task<bool> ObtenerUnidadesMedida(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaUnidadesMedida = typeConvertionsManager.XmlStringToObject<RespuestaUnidadMedida>(respuesta.ResponseBody, "ArrayOfUnidadMedida");
                Console.WriteLine($"Se han recuperado las unidades de medida desde el servicio del MEF. Numero de Unidades de Medida => {respuestaUnidadesMedida.UnidadesMedida.Count}");
                var unidadesMedidaDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaUnidadesMedida.UnidadesMedida));
                return RegistrarUnidadesMedidaPorLotes(unidadesMedidaDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de unidades de medida.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 38.- Registrar las unidades de medida masivamente
        public bool RegistrarUnidadesMedidaPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_UNIDAD_MEDIDA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                       RUBRO
        //============================================================
        //Paso 39.- Elimina los rubros del año actual
        public async Task<string> EliminarRubro()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01O_EliminarRubro", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 40.- Obtiene los tipos de recurso para el año actual desde el servicio Web
        public async Task<bool> ObtenerRubros(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaRubros = typeConvertionsManager.XmlStringToObject<RespuestaRubro>(respuesta.ResponseBody, "ArrayOfRubro");
                Console.WriteLine($"Se han recuperado los tipos de recursos desde el servicio del MEF. Numero de Rubros => {respuestaRubros.Rubros.Count}");
                var rubrosDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaRubros.Rubros));
                return RegistrarRubrosPorLotes(rubrosDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de tipos de recursos.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 41.- Registrar los tipos de recursos masivamente
        public bool RegistrarRubrosPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_RUBRO";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                       EJECUTORA
        //============================================================
        //Paso 42.- Elimina las ejecutoras del año actual
        public async Task<string> EliminarEjecutora()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01P_EliminarEjecutora", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 43.- Obtiene las ejecutoras para el año actual desde el servicio Web
        public async Task<bool> ObtenerEjecutoras(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaEjecutoras = typeConvertionsManager.XmlStringToObject<RespuestaEjecutora>(respuesta.ResponseBody, "ArrayOfEjecutora");
                Console.WriteLine($"Se han recuperado las ejecutoras desde el servicio del MEF. Numero de Ejecutoras => {respuestaEjecutoras.Ejecutoras.Count}");
                var ejecutorasDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaEjecutoras.Ejecutoras));
                return RegistrarEjecutorasPorLotes(ejecutorasDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de ejecutoras.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 44.- Registrar las ejecutoras masivamente
        public bool RegistrarEjecutorasPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_EJECUTORA";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                       FINALIDAD
        //============================================================
        //Paso 45.- Elimina las finalidades del año actual
        public async Task<string> EliminarFinalidad()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaAnulacionBd>("dbo.01Q_EliminarFinalidad", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.FirstOrDefault().UrlServicio;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 46.- Obtiene las finalidades para el año actual desde el servicio Web
        public async Task<bool> ObtenerFinalidades(string url)
        {
            try
            {
                var request = new ProxyManager.Request();
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = url;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };

                respuesta = await proxyManager.CallServiceAsync(request);
                var respuestaEjecutoras = typeConvertionsManager.XmlStringToObject<RespuestaFinalidad>(respuesta.ResponseBody, "ArrayOfFinalidad");
                Console.WriteLine($"Se han recuperado las finalidades desde el servicio del MEF. Numero de Finalidades => {respuestaEjecutoras.Finalidades.Count}");
                var finalidadesDataTable = typeConvertionsManager.ArrayListToDataTable(new ArrayList(respuestaEjecutoras.Finalidades));
                return RegistrarFinalidadesPorLotes(finalidadesDataTable);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion de ejecutoras.\nError Asociado: {exception.Message}");
                return false;
            }
        }

        //Paso 47.- Registrar las finalidades masivamente
        public bool RegistrarFinalidadesPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "MAPA_INVERSIONES.DIM_FINALIDAD";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }


        //============================================================
        //                       METAS_PROYECTO
        //============================================================

        //Paso 48.- Procesar las metas de proyecto
        public async Task<bool> ProcesarMetasProyecto()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaProcesoBd>("MAPA_INVERSIONES.PROCESAR_DIM_METAS_PROYECTO", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                         PROYECTO
        //============================================================

        //Paso 49.- Procesar los proyectos
        public async Task<bool> ProcesarProyectos()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaProcesoBd>("MAPA_INVERSIONES.PROCESAR_DIM_PROYECTO", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }
        }

        //============================================================
        //                         TRANSFERENCIAS
        //============================================================

        //Paso 49.- Procesar los proyectos
        public async Task<bool> ProcesarTransferencias()
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();

                var respuesta = await conexionSql.QueryAsync<RespuestaProcesoBd>("MAPA_INVERSIONES.PROCESAR_TRANSFERENCIAS", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }
        }

        //Paso 51.- Enviar mail por concepto de error o éxito
        public void SendMail(Mail configuracion, string asunto, string mensaje)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(configuracion.De);
                string[] destinatarios = configuracion.Para.Split(";");

                foreach (string destinatario in destinatarios) email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = asunto;//"Notificaciones Mapa Inversiones - Sincronizacion de Datos del MEF";
                email.Body = new TextPart(TextFormat.Html) { Text = mensaje };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(configuracion.Servidor, configuracion.Puerto, SecureSocketOptions.StartTls);
                smtp.Authenticate(configuracion.De, configuracion.Clave);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al enviar la notificacion de la carga fallida. Detalle del error => {exception.Message}");
            }
        }


        /*
        //Paso 1.- Obtener el listado de Ejecutoras
        public async Task<List<WebServiceEjecutar>> ObtenerListadoInvocaciones()
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();
            try
            {
                var respuesta = await conexionSql.QueryAsync<WebServiceEjecutar>("dbo.02A_GenerarListadoEjecutora", commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return respuesta.ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }
        }

        //Paso 2.- Recuperar los datos de la ejecutora
        public async Task<List<Item>> ObtenerItemsPorEjecutora(WebServiceEjecutar ejecutora, int numeroReintentosMaximo)
        {
            var itemsRespuesta = new List<Item>();
            try
            {
                var invocacionesErradas = new List<string>();
                var numeroReintento = 0;
                var request = new ProxyManager.Request();

                Console.WriteLine($"Consulta de servicio : { ejecutora.UrlWebService }.");
                request.HttpMethod = ProxyManager.HttpMethod.Get;
                request.Uri = ejecutora.UrlWebService;
                request.MediaType = ProxyManager.MediaType.Xml;
                var respuesta = new ProxyManager.Response { Ok = false };
                while (!respuesta.Ok && (numeroReintento <= numeroReintentosMaximo))
                {
                    if (numeroReintento > 0)
                    {
                        Console.WriteLine($"Se procede con el reintento numero : {numeroReintento} de consulta al servicio");
                    }
                    try
                    {
                        respuesta = await proxyManager.CallServiceAsync(request);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Error al intentar comunicarse con el servicio del MEF. Detalle del error => {exception.Message}");
                        respuesta.Ok = false;
                    }
                    numeroReintento++;
                }

                if (respuesta.Ok)
                {
                    var listadoItems = typeConvertionsManager.XmlStringToObject<RespuestaServicio>(respuesta.ResponseBody, "DataGasto");
                    itemsRespuesta = listadoItems.Items;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al intentar recuperar la informacion del servicio.\nError Asociado: {exception.Message}");
                itemsRespuesta =  new List<Item>();
            }
            return itemsRespuesta;
        }

        //Paso 3.- Eliminar Item antiguos de la unidad ejecutora
        public async Task<bool> EliminarItemsPorEjecutora(WebServiceEjecutar ejecutora)
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@SecEjec", ejecutora.SecEjec, DbType.String, ParameterDirection.Input, 20);

                var respuesta = await conexionSql.QueryAsync<WebServiceEjecutar>("dbo.02B_EliminarItemGastoPipSgPorEjecutora", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 4.- Registrar los Item de la unidad ejecutora
        public bool RegistrarItemsPorLotes(DataTable valores)
        {
            using var conexionSql = new SqlConnection(Conexion);
            conexionSql.Open();

            using SqlBulkCopy bulkCopy = new(conexionSql);
            bulkCopy.BulkCopyTimeout = TiempoEsperaCargadoMasivo;
            bulkCopy.BatchSize = BatchSize;
            bulkCopy.DestinationTableName = "dbo.ItemGastoPIPSG";

            try
            {
                bulkCopy.WriteToServer(valores);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;

            }
            finally
            {
                conexionSql.Close();
            }
        }
        //Paso 5.- Actualizar la unidad ejecutora a estado procesado
        public async Task<bool> ActualizarEjecutora(WebServiceEjecutar ejecutora)
        {
            using var conexionSql = new SqlConnection(Conexion);
            try
            {
                conexionSql.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@SecEjec", ejecutora.SecEjec, DbType.String, ParameterDirection.Input, 20);

                var respuesta = await conexionSql.QueryAsync<WebServiceEjecutar>("dbo.02C_ActualizarEjecutora", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 1200);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                conexionSql.Close();
            }

        }

        //Paso 6.- Enviar mail por concepto de error o éxito
        public void SendMail(Mail configuracion, string asunto, string mensaje)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(configuracion.De);
                string[] destinatarios = configuracion.Para.Split(";");

                foreach(string destinatario in destinatarios) email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = asunto;//"Notificaciones Mapa Inversiones - Sincronizacion de Datos del MEF";
                email.Body = new TextPart(TextFormat.Html) { Text = mensaje };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(configuracion.Servidor, configuracion.Puerto, SecureSocketOptions.StartTls);
                smtp.Authenticate(configuracion.De, configuracion.Clave);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocurrio un problema al enviar la notificacion de la carga fallida. Detalle del error => { exception.Message }");
            }
        } 
        */
    }
}