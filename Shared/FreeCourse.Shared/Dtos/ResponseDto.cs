using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool isSuccessful { get; set; }
        public ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode,isSuccessful=true };
        }

        public ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { Data = default(T), StatusCode = statusCode, isSuccessful = true };
        }

        public List<string> Errors { get; set; }
        public static ResponseDto<T>Fail(List<string> errors,int StatusCode) 
        {
            return new ResponseDto<T>
            {
                Errors = errors,
                StatusCode = StatusCode,
                isSuccessful= true

            };
        }
        public static ResponseDto<T> Fail(string error, int StatusCode)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string>() { error},
                StatusCode = StatusCode,
                isSuccessful = true

            };
        }
    }
}
