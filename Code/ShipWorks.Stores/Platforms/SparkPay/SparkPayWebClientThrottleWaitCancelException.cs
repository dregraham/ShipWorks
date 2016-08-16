namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayWebClientThrottleWaitCancelException : SparkPayException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayWebClientThrottleWaitCancelException() :
            base("Waiting for SparkPay to stop throttling was canceled.", null)
        {

        }
    }
}
