using Interapptive.Shared.Metrics;

namespace ShipWorks.Products
{
    /// <summary>
    /// A class to track success/failure counts when adding products for telemetry
    /// </summary>
    public class ProductTelemetryCounts
    {
        private readonly string source;

        public double NewSuccessCount { get; private set; }
        public double NewFailureCount { get; private set; }
        public double ExistingSuccessCount { get; private set; }
        public double ExistingFailureCount { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductTelemetryCounts(string source)
        {
            NewSuccessCount = 0;
            NewFailureCount = 0;
            ExistingFailureCount = 0;
            ExistingSuccessCount = 0;
            this.source = source;
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public void AddSuccess(bool isNew)
        {
            if (isNew)
            {
                NewSuccessCount++;
            }
            else
            {
                ExistingSuccessCount++;
            }
        }

        /// <summary>
        /// Add a failure
        /// </summary>
        public void AddFailure(bool isNew)
        {
            if (isNew)
            {
                NewFailureCount++;
            }
            else
            {
                ExistingFailureCount++;
            }
        }

        /// <summary>
        /// Build and send the telemetry event
        /// </summary>
        public void SendTelemetry()
        {
            using (var telemetry = new TrackedEvent("ProductCatalog.Content.Modification"))
            {
                telemetry.AddProperty("ProductCatalog.Content.Modification.Source", source);
                telemetry.AddMetric("ProductCatalog.Content.Modification.Product.New.Quantity.Success", NewSuccessCount);
                telemetry.AddMetric("ProductCatalog.Content.Modification.Product.New.Quantity.Failure", NewFailureCount);
                telemetry.AddMetric("ProductCatalog.Content.Modification.Product.Existing.Quantity.Success", ExistingSuccessCount);
                telemetry.AddMetric("ProductCatalog.Content.Modification.Product.Existing.Quantity.Failure", ExistingFailureCount);
            }
        }
    }
}
