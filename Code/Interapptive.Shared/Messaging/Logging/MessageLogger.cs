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
    public class MessageLogger : IMessageLogger
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static MessageLogger()
        {
#if DEBUG
            Current = new MessageLogger();
#else
            Current = new ReleaseMessageLogger();
#endif
        }

        /// <summary>
        /// Current instance of the message logger
        /// </summary>
        public static IMessageLogger Current { get; }

        readonly WebClient client;
        readonly string endpoint;
        IObserver<ILogItem> observer;

        /// <summary>
        /// Constructor
        /// </summary>
        private MessageLogger()
        {
            endpoint = $"http://localhost:9809/ShipWorks/{Guid.NewGuid()}";
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
