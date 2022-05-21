using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfEspecifica")]
    public class RespuestaEspecifica
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Especifica")]
        [JsonConverter(typeof(SingleOrArrayConverter<Especifica>))]
        public List<Especifica> Especificas { get; set; } = new List<Especifica>();
    }
    public class Especifica
    {
        public int ESPECIFICA_ID { get; set; }
        [JsonProperty("IdEspecifica")]
        public string COD_ESPECIFICA { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_ESPECIFICA { get; set; }
        [JsonProperty("Generica")]
        public string GENERICA { get; set; }
        [JsonProperty("Tipo_transaccion")]
        public string TIPO_TRANSACCION { get; set; }
        [JsonProperty("Subgenerica")]
        public string SUB_GENERICA { get; set; }
        [JsonProperty("Subgenerica_det")]
        public string SUB_GENERICA_DET { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("Ano_eje")]
        public int ANIO_EJE { get; set; }
    }
}
