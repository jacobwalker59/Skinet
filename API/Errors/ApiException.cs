namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null
        , string details = null) 
        : base(statusCode, message)
        {
            Details = details;
        }

        public string Details {get;set;}

        //details is the stack trace
        //using middleware to check about exceptions for server related stuff
    }
}