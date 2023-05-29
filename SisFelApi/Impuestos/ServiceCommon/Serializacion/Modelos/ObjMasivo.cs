using SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos.modelos;

namespace SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos
{
    public class ObjMasivo
    {
        public List<FacturaTxt> facturaTxt { get; set; }
        public List<string> erroresCabecera { get; set; }
        public List<string> erroresDetalle { get; set; }
    }
}
