using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfEjecutora")]
    public class RespuestaEjecutora
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Ejecutora")]
        [JsonConverter(typeof(SingleOrArrayConverter<Ejecutora>))]
        public List<Ejecutora> Ejecutoras { get; set; } = new List<Ejecutora>();
    }
    public class Ejecutora
    {
        public int EJECUTORA_ID { get; set; }
        [JsonProperty("Departamento")]
        public string DEPARTAMENTO { get; set; }
        [JsonProperty("Provincia")]
        public string PROVINCIA { get; set; }
        [JsonProperty("Distrito")]
        public string DISTRITO { get; set; }
        [JsonProperty("Ruc_ejec")]
        public string RUC_EJEC { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
        [JsonProperty("IdSector")]
        public string ID_SECTOR { get; set; }
        [JsonProperty("IdPliego")]
        public string ID_PLIEGO { get; set; }
        [JsonProperty("IdEjecutora")]
        public string ID_EJECUTORA { get; set; }
        [JsonProperty("SecEjec")]
        public int SEC_EJEC { get; set; }
        [JsonProperty("Nombre")]
        public string NOMBRE { get; set; }
        [JsonProperty("TipoUnidad")]
        public string TIPO_UNIDAD { get; set; }
        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
    }
}
