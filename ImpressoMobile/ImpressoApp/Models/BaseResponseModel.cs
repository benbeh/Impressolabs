using System;
namespace ImpressoApp.Models
{
    public class BaseResponseModel
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
    }
}
