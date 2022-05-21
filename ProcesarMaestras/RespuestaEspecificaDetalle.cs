using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfEspecifica_det")]
    public class RespuestaEspecificaDetalle
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Especifica_det")]
        [JsonConverter(typeof(SingleOrArrayConverter<EspecificaDetalle>))]
        public List<EspecificaDetalle> EspecificasDetalle { get; set; } = new List<EspecificaDetalle>();
    }
    public class EspecificaDetalle
    {
        public int ESPECIFICA_DET_ID { get; set; }
        [JsonProperty("IdEspecifica_det")]
        public string COD_ESPECIFICA_DET { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_ESPECIFICA_DET { get; set; }
        [JsonProperty("Generica")]
        public string GENERICA { get; set; }
        [JsonProperty("Tipo_transaccion")]
        public string TIPO_TRANSACCION { get; set; }
        [JsonProperty("Subgenerica")]
        public string SUB_GENERICA { get; set; }
        [JsonProperty("Subgenerica_det")]
        public string SUB_GENERICA_DET { get; set; }
        [JsonProperty("Especifica")]
        public string ESPECIFICA_ID { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("Ano_eje")]
        public int ANIO_EJE { get; set; }
    }
}
