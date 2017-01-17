using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
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
    public class ScanBuffer : IScanBuffer
    {
        private readonly IMessenger messenger;
        private IObserver<string> observer;
        private IntPtr lastHandle;

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

            // Observer.OnNext is called with a letter from the scanner.  We ignore
            // the letter is null or empty. We wait 100 milliseconds to see
            // if there is another input. If we do get another input within the 100
            // milliseconds, the timer restarts. Once the 100 milliseconds is hit,
            // we send all the inputs to SendScanMessage.
            // The ObserveOn makes sure we call the callback method on the
            // WindowsFormsEventLoop.
            scanStream.Where(x => !x.IsNullOrWhiteSpace())
                .BufferUntilInactive(TimeSpan.FromMilliseconds(100))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(SendScanMessage)
                .Subscribe();
        }

        /// <summary>
        /// Append input to the current scan
        /// </summary>
        public void Append(IntPtr handle, string input)
        {
            lastHandle = handle;
            observer.OnNext(input);
        }

        /// <summary>
        /// Send a scan message
        /// </summary>
        private void SendScanMessage(IList<string> characters)
        {
            string text = characters.SelectMany(x => x).Where(c => !char.IsControl(c)).ToString();

            messenger.Send(new ScanMessage(this, text, lastHandle));
        }
    }
}
