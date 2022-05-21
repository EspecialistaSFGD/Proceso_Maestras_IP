using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{
    [JsonObject(Title = "ArrayOfComponente")]
    public class RespuestaComponente
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Componente")]
        [JsonConverter(typeof(SingleOrArrayConverter<Componente>))]
        public List<Componente> Componentes { get; set; } = new List<Componente>();
    }
    public class Componente
    {
        public int COMPONENTE_ID { get; set; }
        [JsonProperty("IdComponente")]
        public string COD_COMPONENTE { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_COMPONENTE { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }

}
