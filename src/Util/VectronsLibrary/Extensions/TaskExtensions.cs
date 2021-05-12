using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VectronsLibrary.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Task"/>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Log exceptions async.
        /// </summary>
        /// <param name="task">The task to check for exceptions.</param>
        /// <param name="logger">The <see cref="ILogger"/> to log the error to.</param>
        /// <returns>The original <see cref="Task"/>.</returns>
        public static Task LogExceptionsAsync(this Task task, ILogger logger)
        {
            _ = task.ContinueWith(
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
                },
                TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        /// <summary>
        /// Invoke action when certain exception type is thrown else log the exception.
        /// </summary>
        /// <typeparam name="T">The exception type where a action needs to be executed.</typeparam>
        /// <param name="task">The task to check for exceptions.</param>
        /// <param name="logger">The <see cref="ILogger"/> to log the error to.</param>
        /// <param name="action">Action to do when exception is of type <typeparamref name="T"/>.</param>
        /// <returns>The original <see cref="Task"/>.</returns>
        public static Task LogExceptionsAsync<T>(this Task task, ILogger logger, Action<T> action)
            where T : Exception
        {
            _ = task.ContinueWith(
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
                },
                TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}