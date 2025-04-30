namespace Todo.Helpers.Exceptions
{
    public class ApiUnauthorizedException : ApiException
    {
        public ApiUnauthorizedException(string message) : base(message) { }
        public ApiUnauthorizedException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
