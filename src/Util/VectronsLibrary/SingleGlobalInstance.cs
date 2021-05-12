using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace VectronsLibrary
{
    /// <summary>
    /// A class that can only have a Single instance on the running system.
    /// Instantiate this class in the constructor to make sure only one instance is running.
    /// </summary>
    public class SingleGlobalInstance : IDisposable
    {
        private readonly Mutex mutex;
        private readonly TimeSpan timeOut;
        private bool hasHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleGlobalInstance"/> class.
        /// </summary>
        public SingleGlobalInstance()
            : this(Timeout.InfiniteTimeSpan, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleGlobalInstance"/> class.
        /// </summary>
        /// <param name="gui">The unique key used to create the mutex. Has to be the same for all instances of the application.</param>
        public SingleGlobalInstance(string gui)
            : this(Timeout.InfiniteTimeSpan, gui)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleGlobalInstance"/> class.
        /// </summary>
        /// <param name="timeOut">Time that we will try and wait to get the mutex.</param>
        public SingleGlobalInstance(TimeSpan timeOut)
            : this(timeOut, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleGlobalInstance"/> class.
        /// </summary>
        /// <param name="timeOut">Time that we will try and wait to get the mutex.</param>
        /// <param name="gui">The unique key used to create the mutex. Has to be the same for all instances of the application.</param>
        public SingleGlobalInstance(TimeSpan timeOut, string gui)
        {
            this.timeOut = timeOut;
            mutex = InitMutex(gui);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (mutex != null)
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }

                mutex.Close();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a value to check if we are the only instance of the mutex.
        /// </summary>
        /// <returns>True if no other instances are running. else false.</returns>
        public bool GetMutex()
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

        private static string GetApplicationGui()
        {
            var guidAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
            var guidAttribute = guidAttributes.FirstOrDefault();

            return guidAttribute == null
                ? throw new ArgumentException($"{nameof(GuidAttribute)} is not defined for this app")
                : ((GuidAttribute)guidAttribute).Value;
        }

        private static Mutex InitMutex(string gui)
        {
            var mutexName = string.IsNullOrWhiteSpace(gui) ? GetApplicationGui() : gui;
            var mutexId = string.Format(CultureInfo.InvariantCulture, @"Global\{{{0}}}", mutexName);

            if (!Mutex.TryOpenExisting(mutexId, out var mutex))
            {
                mutex = new Mutex(true, mutexId);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
}