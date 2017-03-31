using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Wraps an input and an output in a single flow
    /// </summary>
    internal static class DataFlow
    {
        /// <summary>
        /// Create a new flow
        /// </summary>
        public static DataFlow<TInput, TOutput> Create<TInput, TOutput>(
                ITargetBlock<TInput> inputBlock, IReceivableSourceBlock<TOutput> outputBlock) =>
            new DataFlow<TInput, TOutput>(inputBlock, outputBlock);
    }

    /// <summary>
    /// Wraps an input and an output in a single flow
    /// </summary>
    internal class DataFlow<TInput, TOutput> : ITargetBlock<TInput>, IReceivableSourceBlock<TOutput>
    {
        private readonly ITargetBlock<TInput> inputBlock;
        private readonly IReceivableSourceBlock<TOutput> outputBlock;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataFlow(ITargetBlock<TInput> inputBlock, IReceivableSourceBlock<TOutput> outputBlock)
        {
            this.inputBlock = inputBlock;
            this.outputBlock = outputBlock;
        }

        /// <summary>
        /// Completion task
        /// </summary>
        public Task Completion => inputBlock.Completion;

        /// <summary>
        /// Complete the flow
        /// </summary>
        public void Complete() => inputBlock.Complete();

        /// <summary>
        /// Consume a message
        /// </summary>
        public TOutput ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
        {
            return outputBlock.ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        /// <summary>
        /// Mark the flow as faulted
        /// </summary>
        public void Fault(Exception exception)
        {
            inputBlock.Fault(exception);
        }

        /// <summary>
        /// Link the flow to another flow
        /// </summary>
        public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
        {
            return outputBlock.LinkTo(target, linkOptions);
        }

        /// <summary>
        /// Offer a message
        /// </summary>
        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
        {
            return inputBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        /// <summary>
        /// Release reservation
        /// </summary>
        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            outputBlock.ReleaseReservation(messageHeader, target);
        }

        /// <summary>
        /// Reserve a message
        /// </summary>
        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            return outputBlock.ReserveMessage(messageHeader, target);
        }

        /// <summary>
        /// Try to receive a message
        /// </summary>
        public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
        {
            return outputBlock.TryReceive(filter, out item);
        }

        /// <summary>
        /// Try to receive all messages
        /// </summary>
        public bool TryReceiveAll(out IList<TOutput> items)
        {
            return outputBlock.TryReceiveAll(out items);
        }
    }
}