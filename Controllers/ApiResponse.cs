using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_web_api.Controllers
{
    public class ApiResponse<T>
    {
        public bool Success {get; set;}
        public int StatusCode {get;set;}
        public string Message {get; set;} = string.Empty;
        public T? Data {get; set;}
        public List<string>? Errors {get; set;}
        public DateTime TimeStam {get; set;}


        private ApiResponse(bool success, String message, T data,List<String> errors, int statusCode){
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            TimeStam = DateTime.UtcNow;
        }

        public static ApiResponse<T> SuccessResponse(T data,int statuscode,string message = ""){
            return new ApiResponse<T>(true,message,data,null,statuscode);
        }

        
        public static ApiResponse<T> ErrorResponse(List<string>errors,int statuscode,string message = ""){
            return new ApiResponse<T>(false,message,default(T),errors,statuscode);
        }
    }
}