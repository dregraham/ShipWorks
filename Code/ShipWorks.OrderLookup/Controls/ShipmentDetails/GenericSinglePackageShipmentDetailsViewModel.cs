using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model base class for single package shipment details
    /// </summary>
    public abstract class GenericSinglePackageShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericSinglePackageShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel,
            IOrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
            ChangeDimensions = new RelayCommand<ScaleReadResult>(ChangeDimensionsAction);
        }

        /// <summary>
        /// Updates dimensions based on ScaleReadResult
        /// </summary>
        private void ChangeDimensionsAction(ScaleReadResult scaleReadResult)
        {
            if (scaleReadResult.HasVolumeDimensions)
            {
                IPackageAdapter packageAdapter = ShipmentModel.PackageAdapters.First();
                if (packageAdapter.DimsProfileID != 0)
                {
                    packageAdapter.DimsProfileID = 0;
                }
                packageAdapter.DimsLength = scaleReadResult.Length;
                packageAdapter.DimsWidth = scaleReadResult.Width;
                packageAdapter.DimsHeight = scaleReadResult.Height;
                packageAdapter.ApplyAdditionalWeight = false;
            }
        }

        /// <summary>
        /// Updates dimensions based on ScaleReadResult
        /// </summary>
        [Obfuscation]
        public RelayCommand<ScaleReadResult> ChangeDimensions { get; }
    }
}