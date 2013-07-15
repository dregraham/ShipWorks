namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    public interface IExecutionModeInitializer
    {
        /// <summary>
        /// Intended for setting up/initializing any dependencies for an execution mode/context.
        /// </summary>
        /// <param name="executionMode">The execution mode.</param>
        void Initialize(IExecutionMode executionMode);
    }
}
