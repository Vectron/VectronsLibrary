using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="SingleGlobalInstance"/> class.
/// </summary>
[TestClass]
public class SingleGlobalInstanceTests
{
    /// <summary>
    /// Test if the mutex is released when disposing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [TestMethod]
    public async Task DisposeReleasesMutexAsync()
    {
        var gui = Guid.NewGuid().ToString();
        using var firstReset = new ManualResetEventSlim(initialState: false);
        using var secondReset = new ManualResetEventSlim(initialState: false);
        var firstSource = new TaskCompletionSource<bool>();
        var secondSource = new TaskCompletionSource<bool>();

        var firstTask = Task.Run(() => GetMutex(gui, TimeSpan.FromMilliseconds(100), firstReset.WaitHandle, firstSource));
        var result1 = await firstSource.Task.ConfigureAwait(false);
        firstReset.Set();
        await firstTask.ConfigureAwait(false);
        var secondTask = Task.Run(() => GetMutex(gui, TimeSpan.FromMilliseconds(100), secondReset.WaitHandle, secondSource));
        var result2 = await secondSource.Task.ConfigureAwait(false);
        secondReset.Set();
        await Task.WhenAll(firstTask, secondTask).ConfigureAwait(false);

        Assert.IsTrue(result1, "Main mutex not gotten");
        Assert.IsTrue(result2, "Task mutex not gotten");
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="Guid"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowsArgumentExceptionWhenNoValidGuidGiven()
    {
        _ = Assert.ThrowsException<ArgumentException>(SingleGlobalInstance.GetApplicationGui);
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            using var singleGlobalInstance = new SingleGlobalInstance(string.Empty);
        });
        _ = Assert.ThrowsException<ArgumentException>(() =>
        {
            using var singleGlobalInstance = new SingleGlobalInstance(null!);
        });
    }

    /// <summary>
    /// Test if an error is thrown when the max wait time elapsed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [TestMethod]
    public async Task WaitTheMaxTimeBeforeErrorAsync()
    {
        var gui = Guid.NewGuid().ToString();
        using var firstReset = new ManualResetEventSlim(initialState: false);
        using var secondReset = new ManualResetEventSlim(initialState: false);
        var firstSource = new TaskCompletionSource<bool>();
        var secondSource = new TaskCompletionSource<bool>();

        var firstTask = Task.Run(() => GetMutex(gui, TimeSpan.FromMilliseconds(100), firstReset.WaitHandle, firstSource));
        var result1 = await firstSource.Task.ConfigureAwait(false);
        var secondTask = Task.Run(() => GetMutex(gui, TimeSpan.FromMilliseconds(10), secondReset.WaitHandle, secondSource));
        var result2 = await secondSource.Task.ConfigureAwait(false);

        firstReset.Set();
        secondReset.Set();
        await Task.WhenAll(firstTask, secondTask).ConfigureAwait(false);

        Assert.IsTrue(result1, "Main mutex not gotten");
        Assert.IsFalse(result2, "Task mutex gotten");
    }

    private static void GetMutex(string gui, TimeSpan timeout, WaitHandle stop, TaskCompletionSource<bool> taskCompletionSource)
    {
        using var instance = new SingleGlobalInstance(gui);
        var hasInstance = instance.GetMutex(timeout);
        taskCompletionSource.SetResult(hasInstance);
        if (!hasInstance)
        {
            return;
        }

        _ = stop.WaitOne(TimeSpan.FromSeconds(1));
    }
}
