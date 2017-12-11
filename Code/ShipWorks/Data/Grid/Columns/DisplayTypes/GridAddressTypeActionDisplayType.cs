using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Reprsents a column that allows a user to take an action on a row.
    /// </summary>
    public class GridAddressTypeActionDisplayType : GridColumnDisplayType
    {
        private readonly string addressPrefix;
        private readonly EntityGridAddressSelector addressSelector;

        /// <summary>
        /// Common constructor
        /// </summary>
        public GridAddressTypeActionDisplayType(string addressPrefix, EntityGridAddressSelector addressSelector)
        {
            this.addressPrefix = addressPrefix;
            this.addressSelector = addressSelector;
            PreviewInputType = GridColumnPreviewInputType.LiteralString;

            GridHyperlinkDecorator linkDecorator = new GridHyperlinkDecorator();
            linkDecorator.LinkClicked += LinkClicked;
            linkDecorator.QueryEnabled += (sender, args) => args.Enabled = LinkEnabled(args.Entity);
      
            Decorate(linkDecorator);
        }

        private bool LinkEnabled(object value) => 
            !string.IsNullOrWhiteSpace(new AddressAdapter(value as OrderEntity, addressPrefix).AddressValidationError);

        /// <summary>
        /// Get the text to display for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return "";
            }

            AddressAdapter address = new AddressAdapter(value as OrderEntity, addressPrefix);
            
            string status = EnumHelper.GetDescription((AddressType)address.AddressType);

            return string.IsNullOrWhiteSpace(address.AddressValidationError) ? status : status + " (Limited Data)";
           
        }

        /// <summary>
        /// Create the default sort provider to use
        /// </summary>
        public override GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field) => 
            new GridColumnEnumDescriptionSortProvider<AddressType>(field);

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
            if (value == null)
            {
                return null;
            }
            return EnumHelper.GetImage((AddressType) new AddressAdapter(value as OrderEntity, addressPrefix).AddressType);
        }
    }
}

