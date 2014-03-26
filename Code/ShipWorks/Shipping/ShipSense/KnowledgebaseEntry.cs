using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// A class representing the shipment/package information of an entry in
    /// the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    public class KnowledgebaseEntry
    {
        private long storeID = -1;
        private List<KnowledgebasePackage> packages;
        private List<KnowledgebaseCustomsItem> customsItems;
        private bool consolidateMultiplePackagesIntoSinglePackage;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
        /// </summary>
        public KnowledgebaseEntry()
            : this(false)
        {
            packages = new List<KnowledgebasePackage>();
            customsItems = new List<KnowledgebaseCustomsItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
        /// </summary>
        /// <param name="storeID">The store id to which this kb entry belongs.</param>
        public KnowledgebaseEntry(long storeID)
            : this()
        {
            this.storeID = storeID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
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

            StoreID = deserializedEntry.StoreID;
            packages = deserializedEntry.Packages.ToList();
            CustomsItems = deserializedEntry.CustomsItems.ToList();
        }

        /// <summary>
        /// Gets or sets the store ID for this kb entry.
        /// </summary>
        public long StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        /// <summary>
        /// Gets or sets the packages associated with a knowledge base entry.
        /// </summary>
        public IEnumerable<KnowledgebasePackage> Packages
        {
            get { return packages; }
            set { packages = new List<KnowledgebasePackage>(value);}
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
            if (storeID == -1)
            {
                throw new InvalidOperationException("StoreID is required for Knowledge Base Entries, but one was not provided.");
            }

            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Applies the data in the Package collection of the knowledge base entry to each of the adapters
        /// provided.
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        /// <exception cref="System.InvalidOperationException">The number packages in the knowledge base entry must match the number of adapters.</exception>
        public void ApplyTo(IEnumerable<IPackageAdapter> adapters)
        {
            if (!Packages.Any())
            {
                // We have no packages, so this must be a brand new KB Entry, just return.
                return;
            }

            IEnumerable<IPackageAdapter> packageAdapters = adapters as IList<IPackageAdapter> ?? adapters.ToList();
            if (ConsolidateMultiplePackagesIntoSinglePackage && packageAdapters.Count() == 1)
            {
                // We need to consolidate all of the packages of this entry into a single package
                packageAdapters.ElementAt(0).Height = packages[0].Height;
                packageAdapters.ElementAt(0).Length = packages[0].Length;
                packageAdapters.ElementAt(0).Width = packages[0].Width;
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
                    packageAdapters.ElementAt(i).Height = packages[i].Height;
                    packageAdapters.ElementAt(i).Length = packages[i].Length;
                    packageAdapters.ElementAt(i).Weight = packages[i].Weight;
                    packageAdapters.ElementAt(i).Width = packages[i].Width;
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

            if (!CustomsItems.Any())
            {
                return;
            }

            // Explicitly remove the entity, so it gets tracked by LLBLGen
            for (int index = 0; index < shipmentCustomsItems.Count; index++)
            {
                // Always remove the first item in the collection regardless of the index
                // since the list is shrinking with each removal
                shipmentCustomsItems.RemoveAt(0);
            }

            // Add the kb custom items
            foreach (KnowledgebaseCustomsItem knowledgebaseCustomsItem in CustomsItems)
            {
                shipmentCustomsItems.Add(
                    new ShipmentCustomsItemEntity()
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

            foreach(IPackageAdapter adapter in adapters)
            {
                KnowledgebasePackage package = new KnowledgebasePackage
                {
                    AdditionalWeight = adapter.AdditionalWeight,
                    Height = adapter.Height,
                    Length = adapter.Length,
                    Weight = adapter.Weight,
                    Width = adapter.Width,
                    ApplyAdditionalWeight = adapter.ApplyAdditionalWeight
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
                customsItems.Add(
                    new KnowledgebaseCustomsItem()
                    {
                        Description = shipmentCustomsItemEntity.Description,
                        Quantity = shipmentCustomsItemEntity.Quantity,
                        Weight = shipmentCustomsItemEntity.Weight,
                        UnitValue = shipmentCustomsItemEntity.UnitValue,
                        CountryOfOrigin = shipmentCustomsItemEntity.CountryOfOrigin,
                        HarmonizedCode = shipmentCustomsItemEntity.HarmonizedCode,
                        NumberOfPieces = shipmentCustomsItemEntity.NumberOfPieces,
                        UnitPriceAmount = shipmentCustomsItemEntity.UnitPriceAmount
                    });
            }
        }
    }
}
