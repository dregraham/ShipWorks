using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Log messages as they travel through the system
    /// </summary>
    public class MessageLogger
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static MessageLogger()
        {
            Current = new MessageLogger();
        }

        /// <summary>
        /// Current instance of the message logger
        /// </summary>
        public static MessageLogger Current { get; }

        WebClient client;
        IObserver<ILogItem> observer;
        string endpoint = $"http://localhost:9809/Interapptive.Shared.Messaging/abc123";

        /// <summary>
        /// Constructor
        /// </summary>
        private MessageLogger()
        {
            client = new WebClient();

            Observable.Create<ILogItem>(x =>
            {
                observer = x;
                return Disposable.Create(() => observer = null);
            })
            .ObserveOn(TaskPoolScheduler.Default)
            .Select(x => new { Data = JsonConvert.SerializeObject(x), Endpoint = x.Endpoint })
            .Subscribe(x =>
            {
                try
                {
                    client.UploadString(endpoint + "/" + x.Endpoint, "POST", x.Data);
                }
                catch (Exception)
                {
                    // Since this is just for debug logging, eat all errors
                }
            }, ex => { });
        }

        /// <summary>
        /// Log a send message call
        /// </summary>
        public void LogSend<T>(T message, string method) where T : IShipWorksMessage =>
            observer?.OnNext(new SendMessage(message, method));

        /// <summary>
        /// Log an operation
        /// </summary>
        public void Log(ILogItem logItem) => observer?.OnNext(logItem);
    }
}
