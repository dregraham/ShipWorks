using System.Linq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;

namespace ShipWorks.Data.Administration.Scripts.Update
{
    /// <summary>
    /// Process that runs during upgrade to update the database.
    /// </summary>
    public class BestRateToAddressValidation : IUpdateDatabaseProcess
    {
        /// <summary>
        /// Adds Address Validation Filters
        /// </summary>
        public void Process()
        {
            FilterLayoutContext.InitializeForCurrentSession();

            FilterNodeEntity orderNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

            FilterNodeEntity examplesNode = orderNode.ChildNodes.FirstOrDefault(filter => filter.Filter.Name == "Examples");
            if (examplesNode == null)
            {
                examplesNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Address Validation", FilterTarget.Orders), orderNode, orderNode.ChildNodes.Count)[0];
            }

            FilterNodeEntity addressValidationNode = FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity("Address Validation", FilterTarget.Orders), examplesNode, 3)[0];
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Ready to Go", FilterHelper.CreateAddressValidationDefinition(AddressValidationStatusType.Valid, AddressValidationStatusType.Overridden, AddressValidationStatusType.Adjusted)), addressValidationNode, 0);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Address to Look at", FilterHelper.CreateAddressValidationDefinition(AddressValidationStatusType.Error, AddressValidationStatusType.NeedsAttention, AddressValidationStatusType.NotValid, AddressValidationStatusType.WillNotValidate, AddressValidationStatusType.SuggestedSelected)), addressValidationNode, 0);
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity("Not Validated", FilterHelper.CreateAddressValidationDefinition(AddressValidationStatusType.Error, AddressValidationStatusType.NotChecked, AddressValidationStatusType.Pending)), addressValidationNode, 0);
        }
    }
}