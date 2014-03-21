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

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseEntry"/> class.
        /// </summary>
        public KnowledgebaseEntry()
        {
            packages = new List<KnowledgebasePackage>();
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
            IEnumerable<IPackageAdapter> packageAdapters = adapters as IList<IPackageAdapter> ?? adapters.ToList();
            
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
                    Width = adapter.Width
                };

                packages.Add(package);
            }
        }
    }
}
