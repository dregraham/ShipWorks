using System;
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
        public const string AddressValidationFolderName = "Address Validation";
        public const string ExamplesFolderName = "Examples";
        public const string NotValidated = "Not Validated";
        public const string ReadyToGo = "Ready to Go";

        /// <summary>
        /// Adds Address Validation Filters
        /// </summary>
        /// <param name="sqlAdapter"></param>
        public void Upgrade(SqlAdapter sqlAdapter)
        {
            FilterLayoutContext.InitializeForCurrentSession();

            FilterNodeEntity orderNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

            // if Address Validation folder already exists, don't do antyhing.
            if (AddressValidationFolderExists(orderNode))
            {
                return;
            }

            FilterNodeEntity examplesNode = orderNode.ChildNodes.FirstOrDefault(filter => filter.Filter.Name == ExamplesFolderName) ?? 
                FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterFolderEntity(ExamplesFolderName, FilterTarget.Orders), orderNode, orderNode.ChildNodes.Count,sqlAdapter)[0];

            FilterNodeEntity addressValidationNode =
                FilterLayoutContext.Current.AddFilter(
                    FilterHelper.CreateFilterFolderEntity(AddressValidationFolderName, FilterTarget.Orders), examplesNode,
                    examplesNode.ChildNodes.Count, sqlAdapter)[0];

            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity(NotValidated, FilterHelper.CreateAddressValidationDefinition(AddressSelector.NotValidated)), addressValidationNode, 0, sqlAdapter);         
            FilterLayoutContext.Current.AddFilter(FilterHelper.CreateFilterEntity(ReadyToGo, FilterHelper.CreateAddressValidationDefinition(AddressSelector.ReadyToShip)), addressValidationNode, 0, sqlAdapter);
        }

        /// <summary>
        /// Indicates if AddressValidationFolder exists in Examples folder.
        /// </summary>
        private bool AddressValidationFolderExists(FilterNodeEntity orderNode)
        {
            return orderNode.ChildNodes
                .Where(node => node.Filter?.Name == ExamplesFolderName)
                .SelectMany(node => node.ChildNodes)
                .Any(node => node.Filter?.Name == AddressValidationFolderName);
        }
    }
}