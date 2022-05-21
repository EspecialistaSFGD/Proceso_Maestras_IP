using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfFinalidad")]
    public class RespuestaFinalidad
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Finalidad")]
        [JsonConverter(typeof(SingleOrArrayConverter<Finalidad>))]
        public List<Finalidad> Finalidades { get; set; } = new List<Finalidad>();
    }
    public class Finalidad
    {
        public int FINALIDAD_ID { get; set; }
        [JsonProperty("IdFinalidad")]
        public string COD_FINALIDAD { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_FINALIDAD { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
