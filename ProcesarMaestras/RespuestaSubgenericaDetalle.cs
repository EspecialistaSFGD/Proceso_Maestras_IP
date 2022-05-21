using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfSubgenerica_det")]
    public class RespuestaSubgenericaDetalle
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Subgenerica_det")]
        [JsonConverter(typeof(SingleOrArrayConverter<SubgenericaDetalle>))]
        public List<SubgenericaDetalle> SubgenericasDetalle { get; set; } = new List<SubgenericaDetalle>();
    }
    public class SubgenericaDetalle
    {
        public int SUBGENERICA_DET_ID { get; set; }
        [JsonProperty("IdSubgenerica_det")]
        public string COD_SUBGENERICA_DET { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_SUBGENERICA_DET { get; set; }
        [JsonProperty("Tipo_transaccion")]
        public string TIPO_TRANSACCION { get; set; }
        [JsonProperty("Cateoria_gasto")]
        public string CATEGORIA_GASTO { get; set; }
        [JsonProperty("Tipo_act_proy")]
        public string TIPO_ACT_PROY { get; set; }
        [JsonProperty("Generica")]
        public int GENERICA_ID { get; set; }
        [JsonProperty("Subgenerica")]
        public int SUBGENERICA_ID { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("Ano_eje")]
        public int ANIO_EJE { get; set; }
    }
}
