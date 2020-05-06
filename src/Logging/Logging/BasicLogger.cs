using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace VectronsLibrary.Logging
{
    public static class BasicLogger
    {
        public static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string Arrow = " ==> ";
        private static readonly object locker = new object();
        private static readonly string LogFileExtension = ".txt";
        private static readonly CancellationTokenSource taskCancellationTokenSource = new CancellationTokenSource();
        private static readonly char Underscore = '_';
        private static System.Timers.Timer cleanUpTimer = new System.Timers.Timer();
        private static int daysBeforLogDelete = 0;
        private static Task loggingTask;
        private static BlockingCollection<ErrorMessage> messageCollection;

        public delegate void StringLoggedEventHandler(object sender, LoggingEventArgs e);

        public static event StringLoggedEventHandler StringLogged;

        public static void CloseApp()
        {
            messageCollection.CompleteAdding();

            cleanUpTimer.Stop();
            cleanUpTimer.Dispose();
            cleanUpTimer = null;

            loggingTask.Wait(5000);
        }

        public static void SetLogCleanUp(int daysBeforLogDelete)
        {
            if (daysBeforLogDelete > 0)
            {
                BasicLogger.daysBeforLogDelete = daysBeforLogDelete;

                cleanUpTimer.Interval = 1000 * 3600;
                cleanUpTimer.Elapsed += (e, arg) => Task.Factory.StartNew(CleanUpLogFiles);
                cleanUpTimer.Start();
            }
        }

        /// <summary>
        /// Write a message to the log file locate at [applicationdirectory]\log
        /// File format is yy-mm-dd_[AssemblyName]
        /// message will be formatted as: uu:mm:ss ==> [Your Message]
        /// </summary>
        /// <param name="message">the message that needs to be written to the log</param>
        public static void WriteToLogFile(string message)
        {
            try
            {
                string logFormat = DateTime.Now.ToLongTimeString().ToString() + Arrow;
                string programmName = Assembly.GetCallingAssembly().GetName().Name;
                string pathName = Path.Combine(LogDirectory, programmName);

                programmName = programmName.Replace(' ', Underscore);

                string errorTime = DateTime.Now.ToString("yyyy-MM-dd") + Underscore;

                // Check if there is a log directory in the root of the exe, if not create one
                Directory.CreateDirectory(pathName);

                string filePath = Path.Combine(pathName, errorTime + programmName + LogFileExtension);
                string newMessage = logFormat + message;

                StartTaskQue();
                messageCollection.Add(new ErrorMessage(filePath, newMessage));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private static void CleanUpLogFiles()
        {
            try
            {
                cleanUpTimer?.Stop();

                var root = new DirectoryInfo(LogDirectory);
                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    foreach (var file in dir.GetFiles("*" + LogFileExtension))
                    {
                        if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(daysBeforLogDelete))
                        {
                            File.Delete(file.FullName);
                        }
                    }
                }

                cleanUpTimer?.Start();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine("Failed to clean Log files");
            }
        }

        private static void OnStringLoggedEventHandler(LoggingEventArgs e)
        {
            StringLogged?.Invoke(null, e);
        }

        private static void StartTaskQue()
        {
            if (messageCollection == null)
            {
                messageCollection = new BlockingCollection<ErrorMessage>();
            }

            if (loggingTask == null)
            {
                loggingTask = Task.Factory.StartNew(WriteToLog);
            }
        }

        private static void WriteToLog()
        {
            while (!messageCollection.IsCompleted)
            {
                try
                {
                    ErrorMessage message = messageCollection.Take();

                    lock (locker)
                    {
                        // define the streamwrite to create a log file and append to that file if the name allready excist
                        using (var writer = new StreamWriter(message.FilePath, true))
                        {
                            // write the message to the log file with format: uu:mm:ss ==> Log message
                            writer.WriteLine(message.Message);
                        }

                        OnStringLoggedEventHandler(new LoggingEventArgs(message.Message, message.FilePath));
                    }
                }
                catch (InvalidOperationException)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }
    }
}