﻿using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using Microsoft.ApplicationInsights.DataContracts;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for finish processing a batch of labels
    /// </summary>
    [ActionTask("Finish processing a batch of labels", "FinishProcessingBatch", ActionTaskCategory.Administration, true)]
    public class FinishProcessingBatchTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PlaySoundTask));

        /// <summary>
        /// Create the editor for editing the task's settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            throw new NotImplementedException("FinishProcessingBatch task should not editable");
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            string extraData = context?.Queue?.ExtraData;
            if (string.IsNullOrWhiteSpace(extraData))
            {
                log.Warn("Could not get extra data for shipment batch telemetry");
                return;
            }

            try
            {
                ExtraData data = SerializationUtility.DeserializeFromXml<ExtraData>(extraData);

                EventTelemetry eventTelemetry = new EventTelemetry("Shipping.Printing.Labels");
                eventTelemetry.Metrics.Add(TrackedDurationEvent.DurationMetricKey,
                    Math.Floor(DateTime.UtcNow.Subtract(data.StartingTime).TotalMilliseconds));
                eventTelemetry.Metrics.Add(Telemetry.TotalShipmentsKey, data.ShipmentCount);
                eventTelemetry.Metrics.Add(Telemetry.TotalSuccessfulShipmentsKey,
                    Math.Max(data.ShipmentCount - data.ShipmentErrorCount, 0));
                eventTelemetry.Properties.Add("Shipping.Printing.Labels.Workflow", data.WorkflowName);
                eventTelemetry.Properties.Add("Shipping.Printing.Labels.ProcessingTaskCount", data.ProcessingTaskCount.ToString());
                eventTelemetry.Properties.Add(Telemetry.ParallelActionQueueUsed, data.ParallelActionQueueUsed.ToString());

                Telemetry.TrackEvent(eventTelemetry);
            }
            catch (Exception)
            {
                // Ignore crashes, since telemetry data is not essential
            }
        }

        /// <summary>
        /// Input is not required to play a sound
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement => ActionTaskInputRequirement.None;

        /// <summary>
        /// Create extra data that is serialized
        /// </summary>
        public static string CreateExtraData(DateTime startTime, int count, int errorCount, string workflowName, int processingTaskCount)
        {
            return SerializationUtility.SerializeToXml(new ExtraData
            {
                StartingTime = startTime,
                ShipmentCount = count,
                ShipmentErrorCount = errorCount,
                WorkflowName = workflowName,
                ProcessingTaskCount = processingTaskCount
            });
        }

        /// <summary>
        /// Container for extra data used by the task
        /// </summary>
        public class ExtraData
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ExtraData()
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IConfigurationData configData = lifetimeScope.Resolve<IConfigurationData>();
                    IConfigurationEntity configurationEntity = configData.FetchReadOnly();
                    ParallelActionQueueUsed = configurationEntity.UseParallelActionQueue;
                }
            }

            /// <summary>
            /// Time the batch started
            /// </summary>
            public DateTime StartingTime { get; set; }

            /// <summary>
            /// Number of labels printed
            /// </summary>
            public int ShipmentCount { get; set; }

            /// <summary>
            /// How many shipments had errors
            /// </summary>
            public int ShipmentErrorCount { get; set; }

            /// <summary>
            /// Name of the workflow used to process the shipment batch
            /// </summary>
            public string WorkflowName { get; set; }

            /// <summary>
            /// Number of taskes used to process the shipments
            /// </summary>
            public int ProcessingTaskCount { get; set; }

            /// <summary>
            /// Was the parallel action queue used
            /// </summary>
            public bool ParallelActionQueueUsed { get; set; }
        }
    }
}
