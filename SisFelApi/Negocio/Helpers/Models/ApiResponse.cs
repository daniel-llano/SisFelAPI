using NPOI.SS.Formula.Functions;

namespace SisFelApi.Negocio.Helpers.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public MetaData Meta { get; set; }
    }
}
