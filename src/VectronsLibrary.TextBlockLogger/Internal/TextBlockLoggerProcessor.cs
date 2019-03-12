using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace VectronsLibrary.TextBlockLogger.Internal
{
    internal class TextBlockLoggerProcessor : Observable, IDisposable
    {
        private const int maxQueuedMessages = 1024;
        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>(maxQueuedMessages);
        private readonly Thread _outputThread;
        private readonly TextBlock textBlock;
        private string logData;

        public TextBlockLoggerProcessor(TextBlock textBlock)
        {
            this.textBlock = textBlock ?? throw new ArgumentNullException(nameof(textBlock));

            textBlock.DataContext = this;
            var textBinding = new Binding()
            {
                Path = new PropertyPath(nameof(LogData)),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, textBinding);

            // Start TextBlock message queue processor
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "TextBlock logger queue processing thread"
            };
            _outputThread.Start();
        }

        public string LogData
        {
            get => logData;
            set => SetField(ref logData, value);
        }

        public void Dispose()
        {
            _messageQueue.CompleteAdding();

            try
            {
                _outputThread.Join(1500); // with timeout in-case TextBlock is locked by user input
            }
            catch (ThreadStateException) { }
        }

        public virtual void EnqueueMessage(LogMessageEntry message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            // Adding is completed so just log the message
            WriteMessage(message);
        }

        public void SetTextBlock()
        {
        }

        internal virtual void WriteMessage(LogMessageEntry message)
        {
            textBlock?.Dispatcher.Invoke(() =>
            {
                if (message.LevelString != null)
                {
                    var run1 = new Run(message.LevelString)
                    {
                        Foreground = message.LevelForeground
                    };

                    textBlock.Inlines.Add(run1);
                }

                var run2 = new Run(message.Message)
                {
                    Foreground = message.LevelForeground
                };

                textBlock.Inlines.Add(run2);
            });
        }

        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                }
            }
            catch
            {
                try
                {
                    _messageQueue.CompleteAdding();
                }
                catch
                {
                }
            }
        }
    }
}