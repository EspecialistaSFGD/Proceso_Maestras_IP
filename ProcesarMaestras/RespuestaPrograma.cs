using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfProgramaNombre")]
    public class RespuestaPrograma
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("ProgramaNombre")]
        [JsonConverter(typeof(SingleOrArrayConverter<Programa>))]
        public List<Programa> Programas { get; set; } = new List<Programa>();
    }
    public class Programa
    {
        public int PROGRAMA_ID { get; set; }
        [JsonProperty("IdPrograma")]
        public string COD_PROGRAMA { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_PROGRAMA { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
