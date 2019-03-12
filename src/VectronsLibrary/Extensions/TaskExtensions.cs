using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VectronsLibrary.Extensions
{
    public static class TaskExtensions
    {
        public static Task LogExceptionsAsync(this Task task, ILogger logger)
        {
            task.ContinueWith(
                t =>
                {
                    if (t.Exception != null && logger != null)
                    {
                        var aggregateException = t.Exception.Flatten();
                        foreach (var exception in aggregateException.InnerExceptions)
                        {
                            logger.LogError(exception, "Task Error");
                        }
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        public static Task LogExceptionsAsync<T>(this Task task, ILogger logger, Action<T> action)
            where T : Exception
        {
            task.ContinueWith(
                t =>
                {
                    if (t.Exception != null)
                    {
                        var aggregateException = t.Exception.Flatten();
                        foreach (var exception in aggregateException.InnerExceptions)
                        {
                            if (exception is T ex)
                            {
                                action(ex);
                                continue;
                            }

                            logger.LogError(exception, "Task Error");
                        }
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}