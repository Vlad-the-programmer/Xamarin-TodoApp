using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Todo.Helpers.RequestHelpers
{
    public static class RequestHelper
    {
        public async static Task<bool> HandleRequest(this Task serviceMethod)
        {
            try
            {
                await serviceMethod;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Request error: {ex.Message}");
                return false;
            }
        }
    }
}