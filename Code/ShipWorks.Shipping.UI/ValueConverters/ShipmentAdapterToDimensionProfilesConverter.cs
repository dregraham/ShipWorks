using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    public class ShipmentAdapterToDimensionProfilesConverter : IValueConverter
    {
        private readonly IDimensionsManager dimensionsManager;

        /// <summary>
        /// To be called by designer
        /// </summary>
        public ShipmentAdapterToDimensionProfilesConverter() : this(null)
        {

        }

        public ShipmentAdapterToDimensionProfilesConverter(IDimensionsManager dimensionsManager)
        {
            this.dimensionsManager = dimensionsManager ?? GetDimensionsManager();
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // only null if in designer
            if (dimensionsManager == null)
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            // Check to see if object is a postal shipment adapter
            if (!(value is ICarrierShipmentAdapter shipmentAdapter))
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            List<DimensionsProfileEntity> dimensionProfiles = dimensionsManager.Profiles(shipmentAdapter.GetPackageAdapters().FirstOrDefault()).ToList();
            

            return dimensionProfiles.ToDictionary(profile => profile.DimensionsProfileID, profile => profile.Name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private IDimensionsManager GetDimensionsManager() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IDimensionsManager>>().Value;
    }
}
