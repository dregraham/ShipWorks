using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Customs;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// A class representing the shipment/package information of an entry in
    /// the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    [Obfuscation(ApplyToMembers = false, Exclude = true, StripAfterObfuscation = false)]
    public class KnowledgebaseEntry
    {
        private List<KnowledgebasePackage> packages;
        private List<KnowledgebaseCustomsItem> customsItems;

        private List<KnowledgebasePackage> originalPackages;
        private List<KnowledgebaseCustomsItem> originalCustomsItems;

        private bool consolidateMultiplePackagesIntoSinglePackage;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
        /// </summary>
        public KnowledgebaseEntry()
            : this(false)
        {
            packages = new List<KnowledgebasePackage>();
            customsItems = new List<KnowledgebaseCustomsItem>();

            originalPackages = new List<KnowledgebasePackage>();
            originalCustomsItems = new List<KnowledgebaseCustomsItem>();

            AppliedCustoms = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry" /> class.
        /// </summary>
        /// <param name="consolidateMultiplePackagesIntoSinglePackage">if set to <c>true</c> [consolidate multiple packages into single package].</param>
        public KnowledgebaseEntry(bool consolidateMultiplePackagesIntoSinglePackage)
        {
            ConsolidateMultiplePackagesIntoSinglePackage = consolidateMultiplePackagesIntoSinglePackage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
        /// </summary>
        /// <param name="serializedJson">The JSON representation of a KnowledgebaseEntry that has been serialized.</param>
        public KnowledgebaseEntry(string serializedJson)
            : this()
        {
            KnowledgebaseEntry deserializedEntry = JsonConvert.DeserializeObject<KnowledgebaseEntry>(serializedJson);

            packages = deserializedEntry.Packages.ToList();
            CustomsItems = deserializedEntry.CustomsItems.ToList();

            // Disregard the customs for any entry that has customs item info that is blank, so
            // blank data doesn't overwrite the order item data when ShipSense gets applied. This
            // came up as a result of UPS allowing shipments to be processed with blank info that
            // was being saved to ShipSense.
            if (CustomsItems.Any(item => string.IsNullOrWhiteSpace(item.Description) || string.IsNullOrWhiteSpace(item.CountryOfOrigin)
                                         || item.UnitValue <= 0 || item.Weight <= 0))
            {
                CustomsItems = new List<KnowledgebaseCustomsItem>();
            }
        }

        /// <summary>
        /// Gets or sets the original packages. This is used to track the the data
        /// that was passed into the entry ApplyTo method, so we can track what was
        /// ShipSense was applied to.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<KnowledgebasePackage> OriginalPackages
        {
            get { return originalPackages; }
            set { originalPackages = new List<KnowledgebasePackage>(value); }
        }

        /// <summary>
        /// Gets or sets the packages associated with a knowledge base entry.
        /// </summary>
        public IEnumerable<KnowledgebasePackage> Packages
        {
            get { return packages; }
            set { packages = new List<KnowledgebasePackage>(value); }
        }

        /// <summary>
        /// Gets or sets the customs items.
        /// </summary>
        public IEnumerable<KnowledgebaseCustomsItem> CustomsItems
        {
            get { return customsItems; }
            set { customsItems = new List<KnowledgebaseCustomsItem>(value); }
        }

        /// <summary>
        /// Gets or sets the original customs items. This is used to track the the data
        /// that was passed into the entry ApplyTo method, so we can track what was
        /// ShipSense was applied to.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<KnowledgebaseCustomsItem> OriginalCustomsItems
        {
            get { return originalCustomsItems; }
            set { originalCustomsItems = new List<KnowledgebaseCustomsItem>(value); }
        }


        /// <summary>
        /// Gets a value indicating whether the knowledge base entry [applied customs] information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [applied customs]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool AppliedCustoms { get; set; }

        /// <summary>
        /// Gets a value indicating whether [consolidate multiple packages into single package]. This 
        /// does not get serialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if [consolidate multiple packages into single package]; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool ConsolidateMultiplePackagesIntoSinglePackage
        {
            get { return consolidateMultiplePackagesIntoSinglePackage; }
            set { consolidateMultiplePackagesIntoSinglePackage = value; }
        }

        /// <summary>
        /// Serializes the an instance to a JSON formatted string.
        /// </summary>
        /// <returns></returns>
        /// /// <exception cref="System.InvalidOperationException">StoreID is required for Knowledge Base Entries, but one was not provided.</exception>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// If the knowledge base entry was not found in the database, this will return true.
        /// If one was found, false is returned.
        /// </summary>
        [JsonIgnore]
        public bool IsNew
        {
            get { return !Packages.Any(); }
        }

        /// <summary>
        /// Applies the data in the Package collection of the knowledge base entry to each of the adapters
        /// provided.
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        /// <exception cref="System.InvalidOperationException">The number packages in the knowledge base entry must match the number of adapters.</exception>
        public void ApplyTo(IEnumerable<IPackageAdapter> adapters)
        {
            // If this is a new entry, then there's nothing to do, just return.
            if (IsNew)
            {
                return;
            }

            IEnumerable<IPackageAdapter> packageAdapters = adapters as IList<IPackageAdapter> ?? adapters.ToList();

            foreach (IPackageAdapter adapter in packageAdapters)
            {
                // Record the original package data that was provided
                originalPackages.Add(new KnowledgebasePackage(adapter));
            }

            if (ConsolidateMultiplePackagesIntoSinglePackage && packageAdapters.Count() == 1)
            {
                // We need to consolidate all of the packages of this entry into a single package
                packageAdapters.ElementAt(0).DimsHeight = packages[0].Height;
                packageAdapters.ElementAt(0).DimsLength = packages[0].Length;
                packageAdapters.ElementAt(0).DimsWidth = packages[0].Width;
                packageAdapters.ElementAt(0).ApplyAdditionalWeight = packages[0].ApplyAdditionalWeight;

                // Sum the package weights 
                packageAdapters.ElementAt(0).Weight = packages.Sum(p => p.Weight);
                packageAdapters.ElementAt(0).AdditionalWeight = packages.Sum(p => p.AdditionalWeight);
            }
            else
            {
                // The entry is not configured to consolidate the packages, so we should
                // have an adapter for each package
                if (Packages.Count() != packageAdapters.Count())
                {
                    throw new InvalidOperationException("The number of packages in the knowledge base entry must match the number of adapters.");
                }

                for (int i = 0; i < Packages.Count(); i++)
                {
                    packageAdapters.ElementAt(i).AdditionalWeight = packages[i].AdditionalWeight;
                    packageAdapters.ElementAt(i).DimsHeight = packages[i].Height;
                    packageAdapters.ElementAt(i).DimsLength = packages[i].Length;
                    packageAdapters.ElementAt(i).Weight = packages[i].Weight;
                    packageAdapters.ElementAt(i).DimsWidth = packages[i].Width;
                    packageAdapters.ElementAt(i).ApplyAdditionalWeight = packages[i].ApplyAdditionalWeight;
                }
            }
        }

        /// <summary>
        /// Applies the data in the Package collection of the knowledge base entry to each of the adapters
        /// provided, then applies the data in the Customs Items collection of the knowledge base entry to the
        /// ShipmentCustomsItemEntities.
        /// 
        /// ***NOTE***
        /// The caller of this method must update the shipment values of any customs additions; 
        /// i.e. shipment.CustomsValue, shipment.CustomsGenerated, etc...
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        /// <param name="shipmentCustomsItems">List of ShipmentCustomsItemEntitys to apply KnowledgebaseCustomsItems</param>
        /// <exception cref="System.InvalidOperationException">The number packages in the knowledge base entry must match the number of adapters.</exception>
        public void ApplyTo(IEnumerable<IPackageAdapter> adapters, EntityCollection<ShipmentCustomsItemEntity> shipmentCustomsItems)
        {
            ApplyTo(adapters);

            // Make a note that customs were applied that can be inspected when 
            // writing out change sets.
            AppliedCustoms = true;

            // Record the original customs data that was provided
            shipmentCustomsItems.ToList().ForEach(i => originalCustomsItems.Add(new KnowledgebaseCustomsItem(i)));

            if (!CustomsItems.Any())
            {
                // We don't have any customs items for this entry, but we do want the change set to 
                // reflect that the before/after are identical, so copy the customs items provided
                // the the current items for this entry. These change are in memory only and only 
                // apply until it is disposed of.
                foreach (ShipmentCustomsItemEntity entity in shipmentCustomsItems)
                {
                    customsItems.Add(new KnowledgebaseCustomsItem(entity));
                }

                return;
            }

            // Explicitly remove the entity, so it gets tracked by LLBLGen
            while (shipmentCustomsItems.Count > 0)
            {
                // Always remove the first item in the collection regardless of the index
                // since the list is shrinking with each removal
                shipmentCustomsItems.RemoveAt(0);
            }

            // Add the kb custom items
            foreach (KnowledgebaseCustomsItem knowledgebaseCustomsItem in CustomsItems)
            {
                shipmentCustomsItems.Add(
                    new ShipmentCustomsItemEntity
                    {
                        Description = knowledgebaseCustomsItem.Description,
                        Quantity = knowledgebaseCustomsItem.Quantity,
                        Weight = knowledgebaseCustomsItem.Weight,
                        UnitValue = knowledgebaseCustomsItem.UnitValue,
                        CountryOfOrigin = knowledgebaseCustomsItem.CountryOfOrigin,
                        HarmonizedCode = knowledgebaseCustomsItem.HarmonizedCode,
                        NumberOfPieces = knowledgebaseCustomsItem.NumberOfPieces,
                        UnitPriceAmount = knowledgebaseCustomsItem.UnitPriceAmount
                    });
            }
        }

        /// <summary>
        /// Applies the package data from the adapters to the Package collection of the knowledge base entry.
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        public void ApplyFrom(IEnumerable<IPackageAdapter> adapters)
        {
            // The intent here is to duplicate the adapter data into our packages, so we'll
            // clear out any existing package data and recreate it based on the adapters
            packages.Clear();

            foreach (IPackageAdapter adapter in adapters)
            {
                KnowledgebasePackage package = new KnowledgebasePackage
                {
                    AdditionalWeight = adapter.AdditionalWeight,
                    Height = adapter.DimsHeight,
                    Length = adapter.DimsLength,
                    Weight = adapter.Weight,
                    Width = adapter.DimsWidth,
                    ApplyAdditionalWeight = adapter.ApplyAdditionalWeight,
                    Hash = adapter.HashCode()
                };

                packages.Add(package);
            }
        }

        /// <summary>
        /// Applies the package data from the adapters to the Package collection of the knowledge base entry,
        /// then applies the ShipmentCustomsItemEntity data to the Customs Items collection of the knowledge base entry
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        /// <param name="shipmentCustomsItems">List of ShipmentCustomsItemEntitys to apply KnowledgebaseCustomsItems</param>
        public void ApplyFrom(IEnumerable<IPackageAdapter> adapters, IEnumerable<ShipmentCustomsItemEntity> shipmentCustomsItems)
        {
            ApplyFrom(adapters);

            customsItems.Clear();

            // Add the kb custom items
            foreach (ShipmentCustomsItemEntity shipmentCustomsItemEntity in shipmentCustomsItems)
            {
                customsItems.Add(new KnowledgebaseCustomsItem(shipmentCustomsItemEntity));
            }
        }

        /// <summary>
        /// Determines if the specified shipment matches up with the data in this
        /// knowledge base entry.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>Returns true if the data is the same, otherwise false.</returns>
        public bool Matches(ShipmentEntity shipment)
        {
            // We're going to look at the hash of the package adapters to see if they match 
            // the package data in the knowledge base entry
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            List<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(shipment).ToList();

            // The Store ID, packages, and customs must all coincide for the 
            // shipment to match the entry
            bool matches = PackagesMatch(packageAdapters);

            if (matches && shipmentType.IsCustomsRequired(shipment))
            {
                matches = CustomsMatch(shipment);
            }

            return matches;
        }

        /// <summary>
        /// Determines if the data in the customs items matches up with the data in this
        /// knowledge base entry.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>Returns true if the data is the same, otherwise false.</returns>
        private bool CustomsMatch(ShipmentEntity shipment)
        {
            if (CustomsItems.Count() != shipment.CustomsItems.Count)
            {
                // Customs items were either added or removed
                return false;
            }

            // Create a new list of knowledge base customs item from the shipment to compare
            // with the customs items on the entry that was fetched
            List<KnowledgebaseCustomsItem> shipmentCustomsItems = shipment.CustomsItems.Select(customsEntity => new KnowledgebaseCustomsItem(customsEntity)).ToList();

            foreach (KnowledgebaseCustomsItem entryCustomsItem in CustomsItems)
            {
                // All items should have a count of 1 based on the hash otherwise the customs items are different
                if (shipmentCustomsItems.Count(c => c.Hash == entryCustomsItem.Hash) != 1)
                {
                    // Something has changed
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the data in the package adapters matches up with the data in this
        /// knowledge base entry.
        /// </summary>
        /// <param name="packageAdapters">The package adapters.</param>
        /// <returns>Returns true if the data is the same, otherwise false.</returns>
        private bool PackagesMatch(List<IPackageAdapter> packageAdapters)
        {
            // Check that the package counts are equal and the hash of the package adapters 
            // matches those of the entry
            if (Packages.Count() != packageAdapters.Count())
            {
                return false;
            }

            foreach (KnowledgebasePackage package in Packages)
            {
                // All packages should have a count of 1 based on the hash otherwise the packages are different
                if (packageAdapters.Count(p => p.HashCode() == package.Hash) != 1)
                {
                    // Something has changed
                    return false;
                }
            }

            return true;
        }

    }
}
