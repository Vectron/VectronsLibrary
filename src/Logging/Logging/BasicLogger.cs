using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace VectronsLibrary.Logging
{
    /// <summary>
    /// A very basic file logger.
    /// </summary>
    public static class BasicLogger
    {
        /// <summary>
        /// Get the directory where log files are stored.
        /// </summary>
        public static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private static readonly string Arrow = " ==> ";
        private static readonly System.Timers.Timer CleanUpTimer = new();
        private static readonly object Locker = new();
        private static readonly string LogFileExtension = ".txt";
        private static readonly BlockingCollection<ErrorMessage> MessageCollection = new();
        private static readonly char Underscore = '_';
        private static int daysBeforLogDelete;
        private static Task? loggingTask;

        /// <summary>
        /// A event when a item is added to the log.
        /// </summary>
        public static event EventHandler<LoggingEventArgs>? StringLogged;

        /// <summary>
        /// Finalize the logging.
        /// </summary>
        public static void CloseApp()
        {
            MessageCollection.CompleteAdding();
            CleanUpTimer.Stop();
            CleanUpTimer.Dispose();
            _ = loggingTask?.Wait(5000);
        }

        /// <summary>
        /// Set the max age of log files.
        /// </summary>
        /// <param name="daysBeforLogDelete">The amount of days to keep a log file.</param>
        public static void SetLogCleanUp(int daysBeforLogDelete)
        {
            if (daysBeforLogDelete > 0)
            {
                BasicLogger.daysBeforLogDelete = daysBeforLogDelete;

                CleanUpTimer.Interval = 1000 * 3600;
                CleanUpTimer.Elapsed += (e, arg) => Task.Factory.StartNew(CleanUpLogFiles);
                CleanUpTimer.Start();
            }
        }

        /// <summary>
        /// Write a message to the log file locate at [applicationdirectory]\log
        /// File format is yy-mm-dd_[AssemblyName]
        /// message will be formatted as: uu:mm:ss ==> [Your Message].
        /// </summary>
        /// <param name="message">the message that needs to be written to the log.</param>
        public static void WriteToLogFile(string message)
        {
            try
            {
                var logFormat = DateTime.Now.ToLongTimeString().ToString() + Arrow;
                var programmName = Assembly.GetCallingAssembly().GetName().Name;
                var pathName = Path.Combine(LogDirectory, programmName);

                programmName = programmName.Replace(' ', Underscore);

                var errorTime = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + Underscore;

                // Check if there is a log directory in the root of the exe, if not create one
                _ = Directory.CreateDirectory(pathName);

                var filePath = Path.Combine(pathName, errorTime + programmName + LogFileExtension);
                var newMessage = logFormat + message;

                StartTaskQue();
                MessageCollection.Add(new ErrorMessage(filePath, newMessage));
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
                CleanUpTimer?.Stop();

                var root = new DirectoryInfo(LogDirectory);
                foreach (var dir in root.GetDirectories())
                {
                    foreach (var file in dir.GetFiles("*" + LogFileExtension))
                    {
                        if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(daysBeforLogDelete))
                        {
                            File.Delete(file.FullName);
                        }
                    }
                }

                CleanUpTimer?.Start();
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
            if (loggingTask == null)
            {
                loggingTask = Task.Factory.StartNew(WriteToLog);
            }
        }

        private static void WriteToLog()
        {
            while (!MessageCollection.IsCompleted)
            {
                try
                {
                    var message = MessageCollection.Take();

                    lock (Locker)
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