using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfUnidadMedida")]
    public class RespuestaUnidadMedida
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("UnidadMedida")]
        [JsonConverter(typeof(SingleOrArrayConverter<UnidadMedida>))]
        public List<UnidadMedida> UnidadesMedida { get; set; } = new List<UnidadMedida>();
    }
    public class UnidadMedida
    {
        public int UNIDAD_MEDIDA_ID { get; set; }
        [JsonProperty("IdUnidad")]
        public string COD_UNIDAD_MEDIDA { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_UNIDAD_MEDIDA { get; set; }
        [JsonProperty("Ambito")]
        public string AMBITO { get; set; }
        [JsonProperty("Ambito_en")]
        public string AMBITO_EN { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
