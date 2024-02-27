using System;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using", Justification = "We want to dispose in the right spot of a test")]
    [TestMethod]
    public async Task DisposeReleasesMutexAsync()
    {
        var gui = Guid.NewGuid().ToString();
        using var instance = new SingleGlobalInstance(gui);
        var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
        Assert.IsTrue(hasInstance, "Main mutex not gotten");

        var task = Task.Run(() =>
        {
            using var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
            return hasInstance;
        });

        instance.Dispose();
        var result = await task.ConfigureAwait(true);
        Assert.IsTrue(result, "Task mutex not gotten");
    }

    /// <summary>
    /// Test if the mutex is gotten when the thread exits.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [TestMethod]
    public async Task GetMutexWhenThreadExitsAsync()
    {
        var gui = Guid.NewGuid().ToString();
        var task = Task.Run(() =>
        {
            using var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
            return hasInstance;
        });

        var result = await task;
        Assert.IsTrue(result, "Task mutex not gotten");

        using var instance = new SingleGlobalInstance(gui);
        var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
        Assert.IsTrue(hasInstance, "Main mutex not gotten");
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
        using var instance = new SingleGlobalInstance(gui);
        var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
        Assert.IsTrue(hasInstance, "Main mutex not gotten");

        var task = Task.Run(() =>
        {
            using var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex(TimeSpan.FromMilliseconds(100));
            return hasInstance;
        });

        var result = await task.ConfigureAwait(true);
        Assert.IsFalse(result, "Task mutex gotten");
    }
}
