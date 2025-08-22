using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Message = message ?? "İşlem başarılı.";
            Data = data;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Data = default;
        }
    }
}