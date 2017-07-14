using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Adjustment request status enum for ChannelAdvisor
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorAdjustmentRequestStatus
    {
        Error = -1,

        SubmittedNotProcessed = 0,

        NewRma = 1,

        PendingApproval = 2,

        ProcessingApproval = 3,

        ReadyForReturn = 4,

        PendingReturn = 5,

        ProcessingApproval2 = 6,

        PendingRejection = 7,

        ProcessingRejection = 8,

        ProcessedNotAcknowledged = 10,

        PendingReturnRejection = 11,

        ProcessingReturnRejection = 12,

        AcknowledgedPostProcessingNotComplete = 20,

        PostProcessingComplete = 30,

        RejectionCompleted = 31,

        InformationOnly = 32,

        NoChange = -999
    }
}