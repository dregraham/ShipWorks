using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Reference)]
    public class ReferenceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// ctor
        /// </summary>
        public ReferenceViewModel(IViewModelOrchestrator orchestrator)
        {
            Orchestrator = orchestrator;
            Orchestrator.PropertyChanged += OrchestratorPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order lookup Orchestrator
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IViewModelOrchestrator Orchestrator { get; private set; }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void OrchestratorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && Orchestrator.Order != null)
            {
                handler.RaisePropertyChanged(nameof(Orchestrator));
            }
        }
    }
}
