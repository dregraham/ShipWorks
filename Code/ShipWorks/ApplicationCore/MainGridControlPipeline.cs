using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Class for registering observables in the MainGridControl
    /// </summary>
    public class MainGridControlPipeline : IMainGridControlPipeline, IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MainGridControlPipeline));

        // Debouncing observables for searching
        IDisposable quickSearchObservable;
        IDisposable advancedSearchObservable;

        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        public IDisposable Register(MainGridControl mainGridControl)
        {
            return new CompositeDisposable(
                // Wire up observable for debouncing quick search text box
                quickSearchObservable = Observable
                    .FromEventPattern(mainGridControl.SearchTextChangedAdd, mainGridControl.SearchTextChangedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(new SchedulerProvider(() => Program.MainForm).WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing quick search.", ex))
                    .Subscribe(x => mainGridControl.PerformSearch()),

                // Wire up observable for debouncing advanced search text box
                advancedSearchObservable = Observable
                    .FromEventPattern(mainGridControl.FilterEditorDefinitionEditedAdd, mainGridControl.FilterEditorDefinitionEditedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(new SchedulerProvider(() => Program.MainForm).WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing advanced search.", ex))
                    .Subscribe(x => mainGridControl.PerformSearch())
            );
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            advancedSearchObservable?.Dispose();
            quickSearchObservable?.Dispose();
        }
    }
}
