﻿using System;
using System.Collections.Generic;
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

        private readonly WebClient client;
        private readonly string endpoint;
        private readonly JsonSerializerSettings settings;
        IObserver<ILogItem> observer;

        /// <summary>
        /// Constructor
        /// </summary>
        private MessageLogger()
        {
            endpoint = $"http://localhost:9809/ShipWorks/{Guid.NewGuid()}";
            client = new WebClient();
            settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            Observable.Create<ILogItem>(x =>
            {
                observer = x;
                return Disposable.Create(() => observer = null);
            })
            .ObserveOn(TaskPoolScheduler.Default)
            .Where(x => x.TrackingPath != null)
            .Select(x => new { Data = JsonConvert.SerializeObject(x, settings), Endpoint = x.Endpoint })
            .Do(x => client.UploadString(endpoint + "/" + x.Endpoint, "POST", x.Data))
            .Subscribe(_ => { }, ex => { });
        }

        /// <summary>
        /// Add a converter to the list
        /// </summary>
        public void AddConverters(Func<IEnumerable<JsonConverter>> getConverters)
        {
            foreach (JsonConverter converter in getConverters())
            {
                settings.Converters.Add(converter);
            }
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
