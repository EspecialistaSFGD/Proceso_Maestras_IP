using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfTipoRecurso")]
    public class RespuestaTipoRecurso
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("TipoRecurso")]
        [JsonConverter(typeof(SingleOrArrayConverter<TipoRecurso>))]
        public List<TipoRecurso> TiposRecurso { get; set; } = new List<TipoRecurso>();
    }
    public class TipoRecurso
    {
        public int TIPO_RECURSO_ID { get; set; }
        [JsonProperty("IdTipoRecurso")]
        public string COD_TIPO_RECURSO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_TIPO_RECURSO { get; set; }
        [JsonProperty("IdFuenteFinanc")]
        public string ID_FUENTE_FINANCIAMIENTO { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
