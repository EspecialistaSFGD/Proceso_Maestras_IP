using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfFuncion")]
    public class RespuestaFuncion
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Funcion")]
        [JsonConverter(typeof(SingleOrArrayConverter<Funcion>))]
        public List<Funcion> Funciones { get; set; } = new List<Funcion>();
    }
    public class Funcion
    {
        public int FUNCION_ID { get; set; }
        [JsonProperty("IdFuncion")]
        public string COD_FUNCION { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_FUNCION { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
