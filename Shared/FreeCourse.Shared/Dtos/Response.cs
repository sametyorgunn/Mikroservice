using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool isSuccessful { get; set; }
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode,isSuccessful=true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, isSuccessful = true };
        }

        public List<string> Errors { get; set; }
        public static Response<T>Fail(List<string> errors,int StatusCode) 
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = StatusCode,
                isSuccessful= true

            };
        }
        public static Response<T> Fail(string error, int StatusCode)
        {
            return new Response<T>
            {
                Errors = new List<string>() { error},
                StatusCode = StatusCode,
                isSuccessful = true

            };
        }
    }
}
