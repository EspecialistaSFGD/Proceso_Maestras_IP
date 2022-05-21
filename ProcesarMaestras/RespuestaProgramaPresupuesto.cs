using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfPrograma_PPTO_nombre")]
    public class RespuestaProgramaPresupuesto
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Programa_PPTO_nombre")]
        [JsonConverter(typeof(SingleOrArrayConverter<ProgramaPresupuesto>))]
        public List<ProgramaPresupuesto> ProgramasPresupuesto { get; set; } = new List<ProgramaPresupuesto>();
    }
    public class ProgramaPresupuesto
    {
        public int PROGRAMA_PPTO_ID { get; set; }
        [JsonProperty("IdPrograma_ppto")]
        public string COD_PROGRAMA_PPTO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_PROGRAMA_PPTO { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
