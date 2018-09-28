using System.ComponentModel;
using System.Windows.Input;

namespace ShipWorks.OrderLookup.Controls.Weight
{
    public interface IOrderLookupWeightControlViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
        string ErrorMessage { get; set; }
        bool IsEnabled { get; set; }
        string WeightUnitDisplay { get; set; }
        ICommand ReadScaleCommand { get; }

        /// <summary>
        /// The order lookup message bus
        /// </summary>
        IOrderLookupMessageBus MessageBus { get; }

        /// <summary>
        /// Will this control accept the apply weight keyboard shortcut
        /// </summary>
        bool AcceptApplyWeightKeyboardShortcut { get; set; }

        /// <summary>
        /// Source of the weight for telemetry
        /// </summary>
        string TelemetrySource { get; set; }
    }
}