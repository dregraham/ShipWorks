using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.LinqSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Responsible for displaying possible validated addresses
    /// </summary>
    public class OrderGridAddressSelector
    {
        /// <summary>
        /// Display the list of available addresses
        /// </summary>
        public void ShowAddressOptionMenu(object sender, GridHyperlinkClickEventArgs e)
        {
            OrderEntity order = e.Row.Entity as OrderEntity;

            if (order == null)
            {
                return;
            }

            // If we won't validate, let the user know why and don't show the address selection menu
            if (order.ShipAddressValidationStatus == (int) AddressValidationStatusType.WillNotValidate)
            {
                MessageHelper.ShowInformation(Program.MainForm, order.ShipAddressValidationError);
                return;
            }

            List<ValidatedAddressEntity> validatedAddresses = GetOrderAddresses(order);
            ValidatedAddressEntity originalValidatedAddress = validatedAddresses.FirstOrDefault(x => x.IsOriginal);
            List<ValidatedAddressEntity> suggestedAddresses = validatedAddresses.Where(x => !x.IsOriginal).ToList();

            var menu = BuildMenu(order, originalValidatedAddress, suggestedAddresses);

            SandGrid grid = sender as SandGrid;
            Debug.Assert(grid != null);

            menu.Show(grid, new Point(e.MouseArgs.X - grid.HScrollOffset, e.MouseArgs.Y - grid.VScrollOffset));
        }

        /// <summary>
        /// Build the context menu
        /// </summary>
        private static ContextMenu BuildMenu(OrderEntity order, ValidatedAddressEntity originalValidatedAddress, List<ValidatedAddressEntity> suggestedAddresses)
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            if (originalValidatedAddress != null)
            {
                menuItems.Add(CreateMenuItem(originalValidatedAddress, order));
            }

            if (suggestedAddresses.Any())
            {
                if (originalValidatedAddress != null)
                {
                    menuItems.Add(new MenuItem("-"));
                }

                menuItems.AddRange(suggestedAddresses.Select(x => CreateMenuItem(x, order)).OrderBy(x => x.Text));
            }

            return new ContextMenu(menuItems.ToArray());
        }

        /// <summary>
        /// Create a menu item from a validated address
        /// </summary>
        private static MenuItem CreateMenuItem(ValidatedAddressEntity validatedAddress, OrderEntity order)
        {
            string title = FormatAddress(validatedAddress.Address) + 
                (validatedAddress.IsOriginal ? " (Original)" : string.Empty);

            return new MenuItem(title, (sender, args) => SelectAddress(order, validatedAddress));
        }

        /// <summary>
        /// Get a list of addresses associated with the specified order
        /// </summary>
        private static List<ValidatedAddressEntity> GetOrderAddresses(OrderEntity order)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                LinqMetaData metaData = new LinqMetaData(adapter);
                return metaData.ValidatedAddress.Where(x => x.ConsumerID == order.OrderID)
                    .WithPath(x => x.Prefetch(y => y.Address))
                    .ToList();
            }
        }

        /// <summary>
        /// Select an address to copy into the order's shipping address
        /// </summary>
        private static void SelectAddress(OrderEntity order, ValidatedAddressEntity validatedAddressEntity)
        {
            AddressAdapter originalAddress = new AddressAdapter();

            AddressAdapter orderAdapter = new AddressAdapter(order, "Ship");
            orderAdapter.CopyTo(originalAddress);
            
            AddressAdapter.Copy(validatedAddressEntity.Address, string.Empty, orderAdapter);

            order.ShipAddressValidationStatus = validatedAddressEntity.IsOriginal ? 
                (int) AddressValidationStatusType.Overridden : 
                (int) AddressValidationStatusType.SuggestedSelected;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ValidatedAddressManager.PropagateAddressChangesToShipments(adapter, order.OrderID, originalAddress, orderAdapter);
                adapter.SaveAndRefetch(order);
            }

            Program.MainForm.ForceHeartbeat();
        }

        /// <summary>
        /// Format the address for display in the menu
        /// </summary>
        private static string FormatAddress(AddressEntity address)
        {
            StringBuilder format = new StringBuilder();
            AddAddressPart(format, address.Street1);
            AddAddressPart(format, address.Street2);
            AddAddressPart(format, address.Street3);
            AddAddressPart(format, address.City);
            AddAddressPart(format, address.StateProvCode);
            AddAddressPart(format, address.PostalCode);
            return format.ToString();
        }

        /// <summary>
        /// Add a part of the address to the string builder if it is not empty
        /// </summary>
        private static void AddAddressPart(StringBuilder format, string addressPart)
        {
            if (string.IsNullOrWhiteSpace(addressPart))
            {
                return;
            }

            if (format.Length > 0)
            {
                format.Append(", ");
            }

            format.Append(addressPart);
        }
    }
}
