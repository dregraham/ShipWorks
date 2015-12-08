using System;
using System.Reflection;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.ShipSense.Packaging
{
    /// <summary>
    /// A DTO of package information that will be serialized in an entry to the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    [Obfuscation(Exclude = true)]
    public class KnowledgebasePackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebasePackage"/> class.
        /// </summary>
        public KnowledgebasePackage()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebasePackage"/> class.
        /// </summary>
        /// <param name="packageAdapter">The package adapter.</param>
        public KnowledgebasePackage(IPackageAdapter packageAdapter)
        {
            Length = packageAdapter.DimsLength;
            Width = packageAdapter.DimsWidth;
            Height = packageAdapter.DimsHeight;
            Weight = packageAdapter.Weight;
            ApplyAdditionalWeight = packageAdapter.ApplyAdditionalWeight;
            AdditionalWeight = packageAdapter.AdditionalWeight;
        }

        /// <summary>
        /// Gets or sets the hash value that identifies this package configuration.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the whether the additional weight should be applied.
        /// </summary>
        public bool ApplyAdditionalWeight { get; set; }
        
        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight { get; set; }
    }
}
