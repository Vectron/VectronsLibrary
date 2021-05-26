using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class SingleGlobalInstanceTests
    {
        [TestMethod]
        public void DisposeReleasesMutex()
        {
            var gui = Guid.NewGuid().ToString();
            var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex();
            Assert.IsTrue(hasInstance);
            var task = Task.Run(() =>
            {
                using var instance = new SingleGlobalInstance(gui);
                var hasInstance = instance.GetMutex();
                Assert.IsTrue(hasInstance);
            });

            instance.Dispose();
            var taskNotCancelled = task.Wait(TimeSpan.FromSeconds(5));

            Assert.IsTrue(taskNotCancelled);
        }

        [TestMethod]
        public void GetMutexWhenThreadExits()
        {
            var gui = Guid.NewGuid().ToString();
            void Action()
            {
                var instance = new SingleGlobalInstance(gui);
                var hasInstance = instance.GetMutex();
                Assert.IsTrue(hasInstance);
                Task.Delay(200).Wait();
            }

            var thread1 = new Thread(new ThreadStart(Action));
            var thread2 = new Thread(new ThreadStart(Action));

            thread1.Start();
            thread2.Start();
            var taskNotCancelled1 = thread1.Join(TimeSpan.FromSeconds(1));
            var taskNotCancelled2 = thread2.Join(TimeSpan.FromSeconds(1));
            Assert.IsTrue(taskNotCancelled1);
            Assert.IsTrue(taskNotCancelled2);
        }

        [TestMethod]
        public void OnlyOneInstanceAllowed()
        {
            var gui = Guid.NewGuid().ToString();
            using var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex();
            var taskNotCancelled = Task.Run(() =>
            {
                using var instance = new SingleGlobalInstance(gui);
                var hasInstance = instance.GetMutex();
            }).Wait(TimeSpan.FromSeconds(1));

            Assert.IsTrue(hasInstance);
            Assert.IsFalse(taskNotCancelled);
        }

        [TestMethod]
        public void ThrowsArgumentExceptionWhenNoGuiIsSupplied()
            => _ = Assert.ThrowsException<ArgumentException>(() => SingleGlobalInstance.GetApplicationGui());

        [TestMethod]
        public void ThrowsArgumentExceptionWhenNoVallidGuidGiven()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => new SingleGlobalInstance(string.Empty));
            _ = Assert.ThrowsException<ArgumentException>(() => new SingleGlobalInstance(null!));
        }

        [TestMethod]
        public void WaitTheMaxTimeBeforeError()
        {
            var gui = Guid.NewGuid().ToString();
            using var instance = new SingleGlobalInstance(gui);
            var hasInstance = instance.GetMutex();
            var taskNotCancelled = Task.Run(() =>
            {
                using var instance = new SingleGlobalInstance(gui);
                var hasInstance = instance.GetMutex(TimeSpan.FromSeconds(1));
                Assert.IsFalse(hasInstance);
            }).Wait(TimeSpan.FromSeconds(5));

            Assert.IsTrue(hasInstance);
            Assert.IsTrue(taskNotCancelled);
        }
    }
}