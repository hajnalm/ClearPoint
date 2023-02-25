using System;

namespace TodoList.Api.Utils
{
    public static class Extensions
    {
        public static string GetDetailedErrorMessage(this Exception e)
        {
            return $"Message: {e.Message} StackTrace: {e.StackTrace} InnerException: {e.InnerException?.GetDetailedErrorMessage()}";
        }
    }
}
