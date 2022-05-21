using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfSector")]
    public class RespuestaSector
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Sector")]
        [JsonConverter(typeof(SingleOrArrayConverter<Sector>))]
        public List<Sector> Sectores { get; set; } = new List<Sector>();
    }
    public class Sector
    {
        public int SECTOR_ID { get; set; }
        [JsonProperty("IdSector")]
        public string COD_SECTOR { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_SECTOR { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
