using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfRubro")]
    public class RespuestaRubro
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Rubro")]
        [JsonConverter(typeof(SingleOrArrayConverter<Rubro>))]
        public List<Rubro> Rubros { get; set; } = new List<Rubro>();
    }
    public class Rubro
    {
        public int RUBRO_ID { get; set; }
        [JsonProperty("IdRubro")]
        public string COD_RUBRO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_RUBRO { get; set; }
        [JsonProperty("Fuente_financ_agregada")]
        public string FUENTE_FINANC_AGREGADA { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
