namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Update progress
    /// </summary>
    public class DetailProgressUpdater : ProgressUpdater
    {
        readonly string detailFormat;

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailProgressUpdater(IProgressReporter progressReporter, int totalItems, string detailFormat) :
            base(progressReporter, totalItems)
        {
            this.detailFormat = detailFormat;
            progressReporter.Detail = string.Format(detailFormat, 0, totalItems);
        }

        /// <summary>
        /// Update the progress
        /// </summary>
        public override void Update()
        {
            base.Update();
            ProgressReporter.Detail = string.Format(detailFormat, Count, TotalItems);
        }
    }
}
