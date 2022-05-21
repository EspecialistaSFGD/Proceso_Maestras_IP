using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfFuente")]
    public class RespuestaFuente
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Fuente")]
        [JsonConverter(typeof(SingleOrArrayConverter<Fuente>))]
        public List<Fuente> Fuentes { get; set; } = new List<Fuente>();
    }
    public class Fuente
    {
        public int FUENTE_ID { get; set; }
        [JsonProperty("IdFuente")]
        public string COD_FUENTE { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_FUENTE { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
        public int EQUIVALENCIA_ID { get; set; }
        public string EQUIVALENCIA { get; set; } = "";
    }
}
