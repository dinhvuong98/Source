using System;
using System.Threading.Tasks;
namespace Utilities.Helpers
{
    public static class AsyncHelper
    {
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return func.Invoke().Result;
        }

        public static void RunSync(Func<Task> action)
        {
            action.Invoke();
        }
    }
}
