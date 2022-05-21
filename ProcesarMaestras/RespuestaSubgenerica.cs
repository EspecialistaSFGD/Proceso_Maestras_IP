using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfSubgenerica")]
    public class RespuestaSubgenerica
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Subgenerica")]
        [JsonConverter(typeof(SingleOrArrayConverter<Subgenerica>))]
        public List<Subgenerica> Subgenericas { get; set; } = new List<Subgenerica>();
    }
    public class Subgenerica
    {
        public int SUBGENERICA_ID { get; set; }
        [JsonProperty("idSubgenerica")]
        public string COD_SUBGENERICA { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_SUBGENERICA { get; set; }
        [JsonProperty("Tipo_transaccion")]
        public string TIPO_TRANSACCION { get; set; }
        [JsonProperty("Generica")]
        public int GENERICA_ID { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("Ano_eje")]
        public int ANIO_EJE { get; set; }
    }
}
