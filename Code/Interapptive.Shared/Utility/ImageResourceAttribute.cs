using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute that can be applied to supply the name of an image resource to something
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ImageResourceAttribute : Attribute
    {
        string resourceKey;

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
        /// The resource image referenced by the key.  Throws a NotFoundException if not present.
        /// </summary>
        public Image ResourceImage
        {
            get
            {
                Assembly primaryAssembly = Assembly.GetEntryAssembly();
                string primaryResources = string.Format("{0}.Properties.Resources", primaryAssembly.GetName().Name);

                ResourceManager resman = new ResourceManager(primaryResources, primaryAssembly);

                object result = resman.GetObject(ResourceKey);
                if (result == null)
                {
                    throw new NotFoundException(string.Format("The resource '{0}' could not be found in '{1}'.", resourceKey, primaryResources));
                }

                return (Image) result;
            }
        }
    }
}
