using System;

namespace API.Errors
{
    public class ApiResponse
    {
        
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            //null colescing operator, if this is null execute what is to the right
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

         private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resource Found, It Was Not",
                500 => "Errors Are A Path To The Dark Side",
                _ => null
            };
        }
    }
}