using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace VectronsLibrary
{
    public class SingleGlobalInstance : IDisposable
    {
        private readonly Mutex mutex;
        private readonly TimeSpan timeOut;
        private bool hasHandle = false;

        public SingleGlobalInstance()
            : this(Timeout.InfiniteTimeSpan, string.Empty)
        {
        }

        public SingleGlobalInstance(string gui)
                    : this(Timeout.InfiniteTimeSpan, gui)
        {
        }

        public SingleGlobalInstance(TimeSpan timeOut)
            : this(timeOut, string.Empty)
        {
        }

        public SingleGlobalInstance(TimeSpan timeOut, string gui)
        {
            this.timeOut = timeOut;
            mutex = InitMutex(gui);
        }

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
        }

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

        private string GetApplicationGui()
        {
            var guidAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
            var guidAttribute = guidAttributes.FirstOrDefault();

            if (guidAttribute == null)
            {
                throw new ArgumentException($"{nameof(GuidAttribute)} is not defined for this app");
            }

            return ((GuidAttribute)guidAttribute).Value;
        }

        private Mutex InitMutex(string gui)
        {
            var mutexName = string.IsNullOrWhiteSpace(gui) ? GetApplicationGui() : gui;
            string mutexId = string.Format(@"Global\{{{0}}}", mutexName);
            var mutex = new Mutex(true, mutexName);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
            return mutex;
        }
    }
}