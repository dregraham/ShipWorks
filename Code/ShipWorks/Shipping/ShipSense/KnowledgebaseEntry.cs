using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// A class representing the shipment/package information of an entry in
    /// the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    public class KnowledgebaseEntry
    {
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

            this.packages = deserializedEntry.Packages.ToList();
            this.CustomsItems = deserializedEntry.CustomsItems.ToList();
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
        public string ToJson()
        {
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
    }
}
