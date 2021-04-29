using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    public abstract class GenericSinglePackageShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel
    {
        protected GenericSinglePackageShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel,
            IOrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
            ChangeDimensions = new RelayCommand<ScaleReadResult>(ChangeDimensionsAction);
        }

        private void ChangeDimensionsAction(ScaleReadResult scaleReadResult)
        {
            if (scaleReadResult.HasVolumeDimensions)
            {
                IPackageAdapter packageAdapter = ShipmentModel.PackageAdapters.First();
                packageAdapter.DimsProfileID = 0;
                packageAdapter.DimsLength = scaleReadResult.Length;
                packageAdapter.DimsWidth = scaleReadResult.Width;
                packageAdapter.DimsHeight = scaleReadResult.Height;
                packageAdapter.ApplyAdditionalWeight = false;
            }
        }

        [Obfuscation]
        public RelayCommand<ScaleReadResult> ChangeDimensions { get; }
    }
}