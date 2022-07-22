using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ObtenerProyectosPorAnoResult")]
    public class RespuestaProyecto
    {
        //[JsonProperty("@xmlns")]
        //public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Proyecto")]
        [JsonConverter(typeof(SingleOrArrayConverter<Proyecto>))]
        public List<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
    public class Proyecto
    {
        public int PROYECTO_ID { get; set; }
        [JsonProperty("IdProyecto")]
        public string COD_PROYECTO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_PROYECTO { get; set; }
        [JsonProperty("IdProyectoSNIP")]
        public string ID_PROYECTO_SNIP { get; set; }
        [JsonProperty("TipoActProy")]
        public string TIPO_ACT_PROYECTO { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
