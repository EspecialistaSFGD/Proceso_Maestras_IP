using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfGenerica")]
    public class RespuestaGenerica
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Generica")]
        [JsonConverter(typeof(SingleOrArrayConverter<Generica>))]
        public List<Generica> Genericas { get; set; } = new List<Generica>();
    }
    public class Generica
    {
        public int GENERICA_ID { get; set; }
        [JsonProperty("IdGenerica")]
        public string COD_GENERICA { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_GENERICA { get; set; }
        [JsonProperty("Tipo_transaccion")]
        public string TIPO_TRANSACCION { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("Ano_eje")]
        public int ANIO_EJE { get; set; }
    }
}
