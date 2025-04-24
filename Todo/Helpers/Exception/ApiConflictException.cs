namespace Todo.Helpers.Exception
{
    public class ApiConflictException : ApiException
    {
        public ApiConflictException(string message) : base(message) { }
        public ApiConflictException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
