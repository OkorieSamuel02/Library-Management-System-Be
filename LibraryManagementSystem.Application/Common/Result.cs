using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Common
{
    public class Result<T>
    {
        public string message {  get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public T? Data { get; set; } 
        public HttpStatusCode statusCode { get; set; }


        public static Result<T> Success(string message, T data, HttpStatusCode code)
        {
            return new Result<T>
            {
                Data = data,
                message = message,
                IsSuccess = true,
                statusCode = code
            };
        }

        public static Result<T> Failure(string message, HttpStatusCode code)
        {
            return new Result<T>
            {
                message = message,
                IsSuccess = false,
                Data = default!,
                statusCode = code
            };
        }
    }
}
