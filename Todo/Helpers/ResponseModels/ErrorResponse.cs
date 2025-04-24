using System.Collections.Generic;

namespace Todo.Helpers.ResponseModels
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
