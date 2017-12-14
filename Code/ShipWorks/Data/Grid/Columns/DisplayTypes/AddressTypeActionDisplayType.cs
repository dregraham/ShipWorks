using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.SortProviders;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.AddressValidation;
using System.Drawing;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Reprsents a column that allows a user to take an action on a row.
    /// </summary>
    public class AddressTypeActionDisplayType : GridColumnDisplayType
    {
        private readonly string addressPrefix;
        private readonly EntityGridAddressSelector addressSelector;

        /// <summary>
        /// Common constructor
        /// </summary>
        public AddressTypeActionDisplayType(string addressPrefix, EntityGridAddressSelector addressSelector)
        {
            this.addressPrefix = addressPrefix;
            this.addressSelector = addressSelector;
            PreviewInputType = GridColumnPreviewInputType.LiteralString;

            GridHyperlinkDecorator linkDecorator = new GridHyperlinkDecorator();
            linkDecorator.LinkClicked += LinkClicked;
            linkDecorator.QueryEnabled += (sender, args) => args.Enabled = LinkEnabled(args.Entity);
      
            Decorate(linkDecorator);
        }

        /// <summary>
        /// Should we show a link on the grid for this status
        /// </summary>
        /// <remarks>currently only show a link if its an international address
        /// with validation errors</remarks>
        private bool LinkEnabled(EntityBase2 entity)
        {
            if (entity == null)
            {
                return false;
            }

            IAddressAdapter address = new AddressAdapter(entity, addressPrefix);
            return SupportsLimitedData(address);
        }

        /// <summary>
        /// Does the adapter support limited data
        /// </summary>
        private static bool SupportsLimitedData(IAddressAdapter adapter) => 
            !string.IsNullOrWhiteSpace(adapter.AddressValidationError) && 
            adapter.AddressType == (int)AddressType.InternationalAmbiguous;
        
        /// <summary>
        /// Get the address adapter
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity) =>
            new AddressAdapter(entity as OrderEntity, addressPrefix);

        /// <summary>
        /// Create the default sort provider to use
        /// </summary>
        public override GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field) =>
            new GridColumnEnumDescriptionSortProvider<AddressType>(field);

        /// <summary>
        /// Get the text to display for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            AddressAdapter address = value as AddressAdapter;
            if (address == null)
            {
                return "";
            }
                        
            string status = EnumHelper.GetDescription((AddressType) address.AddressType);
            return SupportsLimitedData(address) ? status + " (Limited Data)" : status;
        }
        
        private void LinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000113490-international-address-validation", null);

            addressSelector.ShowAddressOptionMenu(e.Row.Grid.SandGrid, e);
        }

        /// <summary>
        /// Get the image to use that is associated with the enum value
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            AddressAdapter address = value as AddressAdapter;
            if (address == null)
            {
                return null;
            }

            return EnumHelper.GetImage((AddressType) address.AddressType);
        }
    }
}

