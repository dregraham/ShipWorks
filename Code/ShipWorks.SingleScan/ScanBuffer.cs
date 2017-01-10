using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Buffer characters that make up a scan
    /// </summary>
    [Component]
    public class ScanBuffer :IScanBuffer
    {
        readonly IMessenger messenger;
        private IObserver<string> observer;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanBuffer(IMessenger messenger, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;

            IObservable<string> scanStream = Observable.Create<string>(x =>
                {
                    observer = x;
                    return Disposable.Create(() => observer = null);
                })
                .Where(x => !string.IsNullOrEmpty(x))
                .Publish()
                .RefCount();

            scanStream.Where(x => !string.IsNullOrEmpty(x))
                .BufferUntilInactive(TimeSpan.FromMilliseconds(100))
                .Where(x => x.Any())
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(SendScanMessage)
                .Subscribe();
        }

        /// <summary>
        /// Append input to the current scan
        /// </summary>
        public void Append(IntPtr handle, string input)
        {
            observer.OnNext(input);
        }

        /// <summary>
        /// Send a scan message
        /// </summary>
        private void SendScanMessage(IList<string> characters)
        {
            messenger.Send(new ScanMessage(this, characters.Aggregate((obj, item) => obj += item)));
        }
    }
}
