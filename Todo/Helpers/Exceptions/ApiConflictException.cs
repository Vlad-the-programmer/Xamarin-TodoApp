namespace Todo.Helpers.Exceptions
{
    public class ApiConflictException : ApiException
    {
        public ApiConflictException(string message) : base(message) { }
        public ApiConflictException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
