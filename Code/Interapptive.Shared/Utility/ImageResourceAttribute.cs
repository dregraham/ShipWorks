using System;
using System.Linq;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute that can be applied to supply the name of an image resource to something
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ImageResourceAttribute : Attribute
    {
        private static readonly Lazy<Assembly> primaryAssembly = 
            new Lazy<Assembly>(() => AppDomain.CurrentDomain
                .GetAssemblies()
                .SingleOrDefault(a => a.GetName().Name == "ShipWorks.Core")); 

        private string resourceKey;
        private string resourceSet;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageResourceAttribute(string resourceKey)
        {
            this.resourceKey = resourceKey;
        }

        /// <summary>
        /// The resource key of the image that should be used
        /// </summary>
        public string ResourceKey
        {
            get { return resourceKey; }
        }

        /// <summary>
        /// The resource set to use to lookup the Resource key.  If null, defaults to AssemblyName.Properties.Resources.
        /// </summary>
        public string ResourceSet
        {
            get { return resourceSet; }
            set { resourceSet = value; }
        }

        /// <summary>
        /// The resource image referenced by the key.  Throws a NotFoundException if not present.
        /// </summary>
        public Image ResourceImage
        {
            get
            {
                Assembly resourceAssembly = primaryAssembly.Value;

                // The only reason this will be null is if we are in a unit test.
                if (resourceAssembly == null || resourceAssembly.FullName.ToUpper(CultureInfo.InvariantCulture).Contains("MSTEST"))
                {
                    return new Bitmap(1, 1);
                }

                string primaryResources = string.IsNullOrEmpty(resourceSet) ? 
                    "ShipWorks.Properties.Resources" : 
                    resourceSet;

                ResourceManager resman = new ResourceManager(primaryResources, resourceAssembly);

                object result = resman.GetObject(ResourceKey);
                if (result == null)
                {
                    throw new NotFoundException(string.Format("The resource '{0}' could not be found in '{1}'.", resourceKey, primaryResources));
                }

                return (Image)result;
            }
        }
    }
}
