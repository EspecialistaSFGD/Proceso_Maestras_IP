namespace ProcesarMaestras
{
    public class RespuestaAnulacionBd
    {
        public bool EjecutadoCorrectamente { get; set; }
        public string Mensaje { get; set; }
        public string UrlServicio { get; set; }
    }

    public class RespuestaProcesoBd
    {
        public int HttpStatus { get; set; }
        public string RespuestaXml { get; set; }
    }
}
