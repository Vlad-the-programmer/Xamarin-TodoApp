namespace Todo.Helpers.Exception
{
    public class ApiException : System.Exception
    {
        public ApiException(string message) : base(message) { }
        public ApiException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
