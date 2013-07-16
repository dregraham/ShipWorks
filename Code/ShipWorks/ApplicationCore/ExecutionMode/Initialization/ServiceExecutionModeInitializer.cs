

namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    public class ServiceExecutionModeInitializer : ExecutionModeInitializerBase
    {
        public override void Initialize(IExecutionMode executionMode)
        {
            PerformCommonInitialization(executionMode);
        }
    }
}
