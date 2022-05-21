using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfItemPliego")]
    public class RespuestaPliego
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("ItemPliego")]
        [JsonConverter(typeof(SingleOrArrayConverter<Pliego>))]
        public List<Pliego> Pliegos { get; set; } = new List<Pliego>();
    }
    public class Pliego
    {
        public int PLIEGO_ID { get; set; }
        [JsonProperty("IdPliego")]
        public string COD_PLIEGO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_PLIEGO { get; set; } = "SIN NOMBRE";
        [JsonProperty("IdSector")]
        public string COD_SECTOR { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
