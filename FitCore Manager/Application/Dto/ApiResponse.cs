using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }                                                 
        public string Error { get; set; }
        public ApiResponse(bool isSuccess, string message, T data, string error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Error = error;
        }
    }
}
