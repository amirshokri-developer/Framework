using System.Runtime.ExceptionServices;

namespace ASh.Framework.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
