using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Process that runs during upgrade to update the database.
    /// </summary>
    public class AddressValidationDatabaseUpgrade
    {
        /// <summary>
        /// Adds Address Validation Filters
        /// </summary>
        /// <param name="sqlAdapter"></param>
        public void Upgrade(SqlAdapter sqlAdapter)
        {
            FilterLayoutContext.InitializeForCurrentSession();

            FilterNodeEntity orderNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

            FilterNodeEntity examplesNode = orderNode.ChildNodes.FirstOrDefault(filter => filter.Filter.Name == "Examples") ?? 
                FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Examples", FilterTarget.Orders), orderNode, orderNode.ChildNodes.Count,sqlAdapter)[0];

            FilterNodeEntity addressValidationNode =
                FilterLayoutContext.Current.AddFilter(
                    FilterHelper.CreateFilterFolderEntity("Address Validation", FilterTarget.Orders), examplesNode,
                    examplesNode.ChildNodes.Count, sqlAdapter)[0];

            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Not Validated", FilterHelper.CreateAddressValidationDefinition(AddressSelector.NotValidated)), addressValidationNode, 0, sqlAdapter);         
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Ready to Go", FilterHelper.CreateAddressValidationDefinition(AddressSelector.ReadyToShip)), addressValidationNode, 0, sqlAdapter);
        }
    }
}