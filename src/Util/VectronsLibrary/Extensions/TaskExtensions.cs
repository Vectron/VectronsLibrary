using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VectronsLibrary.Extensions;

/// <summary>
/// Extension methods for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Log exceptions asynchronous.
    /// </summary>
    /// <param name="task">The task to check for exceptions.</param>
    /// <param name="logger">The <see cref="ILogger"/> to log the error to.</param>
    /// <returns>The original <see cref="Task"/>.</returns>
    public static Task LogExceptionsAsync(this Task task, ILogger logger)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        return task.ContinueWith(
            t =>
            {
                if (t.Exception != null)
                {
                    var aggregateException = t.Exception.Flatten();
                    for (var i = aggregateException.InnerExceptions.Count - 1; i >= 0; i--)
                    {
                        var exception = aggregateException.InnerExceptions[i];
                        logger.LogError(exception, "Task Error");
                    }
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
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
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        return task.ContinueWith(
            t =>
            {
                if (t.Exception != null)
                {
                    var aggregateException = t.Exception.Flatten();
                    for (var i = aggregateException.InnerExceptions.Count - 1; i >= 0; i--)
                    {
                        var exception = aggregateException.InnerExceptions[i];
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
    }
}
