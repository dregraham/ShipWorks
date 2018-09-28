using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.Weight
{
    [Component]
    public class OrderLookupWeightControlViewModel : INotifyPropertyChanged, IOrderLookupWeightControlViewModel
    {
        private readonly PropertyChangedHandler handler;
        private bool acceptApplyWeightKeyboardShortcut;
        private string telemetrySource;
        private readonly IScaleReader scaleReader;
        public event PropertyChangedEventHandler PropertyChanged;

        public OrderLookupWeightControlViewModel(IOrderLookupMessageBus messageBus, IScaleReader scaleReader)
        {
            MessageBus = messageBus;
            MessageBus.PropertyChanged += MessageBusPropertyChanged;
            this.scaleReader = scaleReader;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ReadScaleCommand = new RelayCommand(async () => await ApplyWeight(UI.Controls.WeightControl.ButtonTelemetryKey));
            WeightUnitDisplay = "lbs";
            IsEnabled = true;
        }      

        [Obfuscation(Exclude = true)] 
        public string ErrorMessage { get; set; }
        
        [Obfuscation(Exclude = true)]
        public bool IsEnabled { get; set; }
        
        [Obfuscation(Exclude = true)]
        public string WeightUnitDisplay { get; set; }
        
        [Obfuscation(Exclude = true)]
        public ICommand ReadScaleCommand { get; }
        
        /// <summary>
        /// The order lookup message bus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; }
        
        /// <summary>
        /// Will this control accept the apply weight keyboard shortcut
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AcceptApplyWeightKeyboardShortcut
        {
            get => acceptApplyWeightKeyboardShortcut;
            set => handler.Set(nameof(AcceptApplyWeightKeyboardShortcut), ref acceptApplyWeightKeyboardShortcut, value);
        }
        
        /// <summary>
        /// Source of the weight for telemetry
        /// </summary>
        public string TelemetrySource
        {
            get => telemetrySource;
            set => handler.Set(nameof(TelemetrySource), ref telemetrySource, value);
        }
        
        /// <summary>
        /// Apply the weight from the scale
        /// </summary>
        private async Task ApplyWeight(string invocationMethod)
        {
            ErrorMessage = string.Empty;
            IsEnabled = false;

            using (ITrackedDurationEvent durationEvent = StartTrackingApplyWeight())
            {
                ScaleReadResult result = await scaleReader.ReadScale();
                SetTelemetryProperties(durationEvent, result, invocationMethod);

                IsEnabled = true;

                if (result.Status != ScaleReadStatus.Success)
                {
                    ErrorMessage = result.Message;
                    return;
                }

                MessageBus.ShipmentAdapter.ContentWeight = result.Weight;
            }
        }
        
        /// <summary>
        /// Start tracking the apply weight command
        /// </summary>
        private ITrackedDurationEvent StartTrackingApplyWeight()
        {
            return TrackedDurationEvent.Dummy;
//            return string.IsNullOrEmpty(TelemetrySource) ?
//                TrackedDurationEvent.Dummy :
//                startDurationEvent("Shipment.Scale.Weight.Applied");
        }

        /// <summary>
        /// Set telemetry properties when applying weight
        /// </summary>
        private void SetTelemetryProperties(ITrackedDurationEvent telemetryEvent, ScaleReadResult result, string invocationMethod)
        {
//            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.Source", TelemetrySource);
//            telemetryEvent.AddMetric(Controls.WeightControl.ShipmentQuantityTelemetryKey, 1);
//            telemetryEvent.AddMetric(Controls.WeightControl.PackageQuantityTelemetryKey, 1);
//            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.InvocationMethod", invocationMethod);
//            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ScaleType", result.ScaleType.ToString());
//            telemetryEvent.AddProperty("Shipment.Scale.Weight.Applied.ShortcutKey.Used",
//                                       invocationMethod == Controls.WeightControl.KeyboardShortcutTelemetryKey ?
//                                           new KeyboardShortcutData(shortcutManager.GetWeighShortcut()).ShortcutText :
//                                           "N/A");
//            telemetryEvent.AddMetric("Shipment.Scale.Weight.Applied.ShortcutKey.ConfiguredQuantity", 1);
        }
        
        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && MessageBus.Order != null)
            {
                handler.RaisePropertyChanged(nameof(MessageBus));
            }
        }
    }
}