using Microsoft.Extensions.Configuration;
using NextSIT.Utility;
using System;
using System.Threading.Tasks;

namespace ProcesarMaestras
{
    internal class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            var conexionBd = "";
            var numeroReintentos = "";
            var servidorSmtp = "";
            var puertoSmtp = "";
            var usuarioSmtp = "";
            var claveSmtp = "";
            var deSmtp = "";
            var paraSmtp = "";
            try
            {
                conexionBd = string.IsNullOrEmpty(args[0]) ? "" : args[0];
                numeroReintentos = string.IsNullOrEmpty(args[1]) ? "" : args[1];
                servidorSmtp = string.IsNullOrEmpty(args[2]) ? "" : args[2];
                puertoSmtp = string.IsNullOrEmpty(args[3]) ? "" : args[3];
                usuarioSmtp = string.IsNullOrEmpty(args[4]) ? "" : args[4];
                claveSmtp = string.IsNullOrEmpty(args[5]) ? "" : args[5];
                deSmtp = string.IsNullOrEmpty(args[6]) ? "" : args[6];
                paraSmtp = string.IsNullOrEmpty(args[7]) ? "" : args[7];

                Console.WriteLine("Parametros enviados desde la consola");
                Console.WriteLine(args == null ? "No hay parametros" : string.Join('-', args));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Algunos parametros no han sido transferidos a la consola, se utilizaran los valores por defecto. Detalle del error => {exception.Message}");
            }

            IConfigurationBuilder builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables();

            Configuration = builder.Build();

            var mailConfiguration = new Mail
            {
                Servidor = !string.IsNullOrEmpty(servidorSmtp) ? servidorSmtp : Configuration.GetSection("Servidor").Value.ToString(),
                Puerto = int.Parse(!string.IsNullOrEmpty(puertoSmtp) ? puertoSmtp : Configuration.GetSection("Puerto").Value.ToString()),
                Usuario = !string.IsNullOrEmpty(usuarioSmtp) ? usuarioSmtp : Configuration.GetSection("Usuario").Value.ToString(),
                Clave = !string.IsNullOrEmpty(claveSmtp) ? claveSmtp : Configuration.GetSection("Clave").Value.ToString(),
                De = !string.IsNullOrEmpty(deSmtp) ? deSmtp : Configuration.GetSection("De").Value.ToString(),
                Para = !string.IsNullOrEmpty(paraSmtp) ? paraSmtp : Configuration.GetSection("Para").Value.ToString()
            };

            EjecutarProceso(
                    !string.IsNullOrEmpty(conexionBd) ? conexionBd : Configuration.GetConnectionString("ConexionPcm"),
                    int.Parse(!string.IsNullOrEmpty(numeroReintentos) ? numeroReintentos : Configuration.GetSection("NumeroReintentosMaximo").Value.ToString()),
                    mailConfiguration
                    )
                .GetAwaiter()
                .GetResult();
        }


        static async Task EjecutarProceso(string conexion, int numeroReintentosMaximo, Mail mail)
        {
            var repositorio = new Repositorio(conexion);
            var mensajeRespuesta = @"<h2>Proceso de carga masiva de maestras</h2><p>mensaje_respuesta</p>";
            try
            {
                Console.WriteLine($"--------------------------------------------------------------------");
                Console.WriteLine($"    Proceso de carga de Tablas Maestras para el anio configurado");
                Console.WriteLine($"--------------------------------------------------------------------");
                var typeConvertionsManager = TypeConvertionManager.GetNewTypeConvertionManager();
                
                #region Componentes
                var respuesta = await repositorio.EliminarComponentes();
                Console.WriteLine($"Se han eliminado los componentes del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de componentes del servicio web. URL => {respuesta}");
                var cargaConforme = await repositorio.ObtenerComponentes(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de componentes");
                }
                Console.WriteLine($"Se han registrado los componentes correctamente en la base de datos");
                #endregion

                #region Especificas
                respuesta = await repositorio.EliminarEspecificas();
                Console.WriteLine($"Se han eliminado las especificas del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de especificas del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerEspecificas(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de especificas");
                }
                Console.WriteLine($"Se han registrado las especificas correctamente en la base de datos");
                #endregion

                #region Detalle de Especificas
                respuesta = await repositorio.EliminarEspecificasDetalle();
                Console.WriteLine($"Se han eliminado los detalles de especificas del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de detalles de especificas del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerEspecificasDetalle(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de detalle de especificas");
                }
                Console.WriteLine($"Se han registrado los detalles de especificas correctamente en la base de datos");
                #endregion

                #region Fuentes
                respuesta = await repositorio.EliminarFuentes();
                Console.WriteLine($"Se han eliminado las fuentes del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las fuentes del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerFuentes(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de fuentes");
                }
                Console.WriteLine($"Se han registrado las fuentes correctamente en la base de datos");
                #endregion

                #region Funciones
                respuesta = await repositorio.EliminarFuncion();
                Console.WriteLine($"Se han eliminado las funciones del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las funciones del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerFunciones(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de funciones");
                }
                Console.WriteLine($"Se han registrado las funciones correctamente en la base de datos");
                #endregion

                #region Genericas
                respuesta = await repositorio.EliminarGenerica();
                Console.WriteLine($"Se han eliminado las genericas del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las genericas del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerGenericas(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de genericas");
                }
                Console.WriteLine($"Se han registrado las genericas correctamente en la base de datos");
                #endregion

                #region Pliegos
                respuesta = await repositorio.EliminarPliego();
                Console.WriteLine($"Se han eliminado los pliegos del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los pliegos del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerPliegos(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de pliegos");
                }
                Console.WriteLine($"Se han registrado los pliegos correctamente en la base de datos");
                #endregion

                #region Programas
                respuesta = await repositorio.EliminarPrograma();
                Console.WriteLine($"Se han eliminado los programas del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los programas del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerProgramas(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de programas");
                }
                Console.WriteLine($"Se han registrado los programas correctamente en la base de datos");
                #endregion

                #region Programas Presupuesto
                respuesta = await repositorio.EliminarProgramaPresupuesto();
                Console.WriteLine($"Se han eliminado los programas presupuesto del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los programas presupuesto del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerProgramasPresupuesto(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de programas presupuesto");
                }
                Console.WriteLine($"Se han registrado los programas presupuesto correctamente en la base de datos");
                #endregion

                #region Sectores
                respuesta = await repositorio.EliminarSector();
                Console.WriteLine($"Se han eliminado los sectores del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los sectores del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerSectores(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de sectores");
                }
                Console.WriteLine($"Se han registrado los sectores correctamente en la base de datos");
                #endregion

                #region Subgenericas
                respuesta = await repositorio.EliminarSubgenerica();
                Console.WriteLine($"Se han eliminado las subgenericas del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las subgenericas del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerSubgenericas(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de subgenericas");
                }
                Console.WriteLine($"Se han registrado las subgenericas correctamente en la base de datos");
                #endregion

                #region Subgenericas Detalle
                respuesta = await repositorio.EliminarSubgenericaDetalle();
                Console.WriteLine($"Se han eliminado las subgenericas detalle del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las subgenericas detalle del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerSubgenericasDetalle(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de subgenericas detalle");
                }
                Console.WriteLine($"Se han registrado las subgenericas detalle correctamente en la base de datos");
                #endregion

                #region Tipos Recursos
                respuesta = await repositorio.EliminarTipoRecurso();
                Console.WriteLine($"Se han eliminado los tipos de recursos detalle del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los tipos de recursos del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerTiposRecurso(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de tipos de recursos");
                }
                Console.WriteLine($"Se han registrado los tipos de recursos correctamente en la base de datos");
                #endregion

                #region Unidades Medidas
                respuesta = await repositorio.EliminarUnidadMedida();
                Console.WriteLine($"Se han eliminado las unidades medida del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las unidades de medida del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerUnidadesMedida(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de unidades de medida");
                }
                Console.WriteLine($"Se han registrado las unidades de medida correctamente en la base de datos");
                #endregion

                #region Rubros
                respuesta = await repositorio.EliminarRubro();
                Console.WriteLine($"Se han eliminado los rubros del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los rubros del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerRubros(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de rubros");
                }
                Console.WriteLine($"Se han registrado los rubros correctamente en la base de datos");
                #endregion

                #region Ejecutoras
                respuesta = await repositorio.EliminarEjecutora();
                Console.WriteLine($"Se han eliminado las ejecutoras del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las ejecutoras del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerEjecutoras(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de ejecutoras");
                }
                Console.WriteLine($"Se han registrado las ejecutoras correctamente en la base de datos");
                #endregion

                #region Finalidades
                respuesta = await repositorio.EliminarFinalidad();
                Console.WriteLine($"Se han eliminado las finalidades del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las finalidades del servicio web. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerFinalidades(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de finalidades");
                }
                Console.WriteLine($"Se han registrado las finalidades correctamente en la base de datos");
                #endregion

                #region Metas de proyectos
                respuesta = await repositorio.EliminarMetaProyecto();
                Console.WriteLine($"Se han eliminado las metas de proyecto del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de las metas de proyectos del servicio web y cargarlas en la BD. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerMetasProyecto(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de metas de proyecto");
                }
                Console.WriteLine($"Se han registrado las metas de proyecto correctamente en la base de datos");
                #endregion

                
                #region Proyectos
                respuesta = await repositorio.EliminarProyecto();
                Console.WriteLine($"Se han eliminado los proyecto del anio configurado");
                Console.WriteLine($"Se procedera a recuperar la informacion de los proyectos del servicio web y cargarlas en la BD. URL => {respuesta}");
                cargaConforme = await repositorio.ObtenerProyectos(respuesta);
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de proyectos");
                }
                Console.WriteLine($"Se han registrado los proyectos correctamente en la base de datos");
                #endregion

                #region Transferencias
                Console.WriteLine($"Se procedera a recuperar la informacion de las transferencias del servicio web y cargarlas en la BD. URL => {respuesta}");
                cargaConforme = await repositorio.ProcesarTransferencias();
                if (!cargaConforme)
                {
                    throw new Exception("Se procede a finalizar el proceso de carga masiva por error de carga de transferencias");
                }
                Console.WriteLine($"Se han registrado las transferencias correctamente en la base de datos");
                #endregion

                mensajeRespuesta = mensajeRespuesta.Replace("mensaje_respuesta", "El proceso de carga de tablas maestras ha finalizado correctamente");

                repositorio.SendMail(mail, "Proceso de Carga Masiva de Tablas Maestras", mensajeRespuesta);

                return;
            }
            catch (Exception exception)
            {

                var detalle = $"Ocurrió un problema durante el proceso de carga de tablas maestras. Detalle del error : {exception.Message}";
                mensajeRespuesta = mensajeRespuesta.Replace("mensaje_respuesta", detalle);
                repositorio.SendMail(mail, "Proceso de Carga Masiva de Tablas Maestras", mensajeRespuesta);

                Console.WriteLine(exception.Message);

                throw;
            }

        }



    }
}
