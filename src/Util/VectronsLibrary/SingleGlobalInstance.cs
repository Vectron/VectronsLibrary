using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace VectronsLibrary;

/// <summary>
/// A class that can only have a Single instance on the running system.
/// Instantiate this class in the constructor to make sure only one instance is running.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SingleGlobalInstance"/> class.
/// </remarks>
/// <param name="gui">The unique key used to create the mutex. Has to be the same for all instances of the application.</param>
public sealed class SingleGlobalInstance(string gui) : IDisposable
{
    private readonly Mutex mutex = InitMutex(gui);
    private bool disposed;
    private bool hasHandle;

    /// <summary>
    /// Gets the guid defined in a <see cref="GuidAttribute"/> on the assembly level.
    /// </summary>
    /// <returns>The found gui string. </returns>
    public static string GetApplicationGui()
    {
        var guidAttribute = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).FirstOrDefault();

        return guidAttribute == null
            ? throw new ArgumentException($"{nameof(GuidAttribute)} is not defined for this app")
            : ((GuidAttribute)guidAttribute).Value;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        if (mutex != null)
        {
            if (hasHandle)
            {
                mutex.ReleaseMutex();
            }

            mutex.Close();
            mutex.Dispose();
        }
    }

    /// <summary>
    /// Returns a value to check if we are the only instance of the mutex. waits Infinite for the release.
    /// </summary>
    /// <returns>True if no other instances are running. else false.</returns>
    public bool GetMutex()
        => GetMutex(Timeout.InfiniteTimeSpan);

    /// <summary>
    /// Returns a value to check if we are the only instance of the mutex.
    /// </summary>
    /// <param name="timeOut">Time that we will try and wait to get the mutex.</param>
    /// <returns>True if no other instances are running. else false.</returns>
    public bool GetMutex(TimeSpan timeOut)
    {
        try
        {
            hasHandle = mutex.WaitOne(timeOut, false);
        }
        catch (AbandonedMutexException)
        {
            hasHandle = true;
        }

        return hasHandle;
    }

    private static Mutex InitMutex(string gui)
    {
        if (string.IsNullOrEmpty(gui))
        {
            throw new ArgumentException($"'{nameof(gui)}' cannot be null or empty.", nameof(gui));
        }

        var mutexId = string.Format(CultureInfo.InvariantCulture, @"Global\{{{0}}}", gui);

        if (!Mutex.TryOpenExisting(mutexId, out var mutex))
        {
            mutex = new Mutex(true, mutexId);
            if (OperatingSystem.IsWindows())
            {
                var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                mutex.SetAccessControl(securitySettings);
            }
        }

        return mutex;
    }
}
