using System;
using System.Reactive;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// View model for the order lookup field manager
    /// </summary>
    [Component]
    public class OrderLookupFieldManagerViewModel : IOrderLookupFieldManager
    {
        private readonly Func<IOrderLookupFieldManager, IOrderLookupFieldManagerDialog> createDialog;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldManagerViewModel(Func<IOrderLookupFieldManager, IOrderLookupFieldManagerDialog> createDialog, IMessageHelper messageHelper)
        {
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Show the field manager dialog
        /// </summary>
        /// <remarks>
        public Unit ShowManager()
        {
            var dialog = createDialog(this);

            messageHelper.ShowDialog(dialog);

            return Unit.Default;
        }
    }
}
