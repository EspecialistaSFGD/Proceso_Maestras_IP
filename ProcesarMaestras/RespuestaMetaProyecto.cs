using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProcesarMaestras
{

    [JsonObject(Title = "ArrayOfSector")]
    public class RespuestaMetaProyecto
    {
        [JsonProperty("@xmlns")]
        public string UriServicio { get; set; } = "http://www.mef.gob.pe/";
        [JsonProperty("Meta")]
        [JsonConverter(typeof(SingleOrArrayConverter<MetaProyecto>))]
        public List<MetaProyecto> MetasProyecto { get; set; } = new List<MetaProyecto>();
    }
    public class MetaProyecto
    {
        public int METAS_PROYECTO_ID { get; set; }
        [JsonProperty("IdMeta")]
        public string COD_METAS_PROYECTO { get; set; }
        [JsonProperty("Nombre")]
        public string DESCRIPCION_METAS_PROYECTO { get; set; }
        [JsonProperty("Monto")]
        public string MONTO { get; set; }
        [JsonProperty("Cantidad_inicial")]
        public string CANTIDAD_INICIAL { get; set; }
        [JsonProperty("Unidad_medida_inicial")]
        public string UNIDAD_MEDIDA_INICIAL { get; set; }
        [JsonProperty("SecEjec")]
        public string SEC_EJEC { get; set; }
        [JsonProperty("SecFunc")]
        public string SEC_FUNC { get; set; }
        [JsonProperty("IdActProy")]
        public string ID_ACT_PROY { get; set; }
        [JsonProperty("IdFinalidad")]
        public string ID_FINALIDAD { get; set; }
        [JsonProperty("Dep")]
        public string DEPARTAMENTO { get; set; }
        [JsonProperty("Pro")]
        public string PROVINCIA { get; set; }
        [JsonProperty("Dis")]
        public string DISTRITO { get; set; }
        [JsonProperty("Componente")]
        public string COMPONENTE { get; set; }
        [JsonProperty("Programa_ppto")]
        public string PROGRAMA_PPTO { get; set; }
        [JsonProperty("IdUnidad")]
        public string ID_UNIDAD { get; set; }
        [JsonProperty("Cantidad")]
        public string CANTIDAD { get; set; }
        [JsonProperty("Funcion")]
        public string FUNCION { get; set; }
        [JsonProperty("Programa")]
        public string PROGRAMA { get; set; }
        [JsonProperty("Sub_programa")]
        public string SUB_PROGRAMA { get; set; }

        [JsonProperty("Estado")]
        public string ESTADO { get; set; }
        [JsonProperty("AnoEje")]
        public int ANIO_EJE { get; set; }
    }
}
