using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Class for binding packaging types to UI controls
    /// </summary>
    public class PackageTypeBinding
    {
        /// <summary>
        /// The ID for this packaging type.  This should be the int value of the packaging type enum.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackageTypeID { get; set; }

        /// <summary>
        /// The display name of this packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name { get; set; }

        /// <summary>
        /// Override equals so that we compare by PackageTypeID
        /// </summary>
        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            PackageTypeBinding p = (PackageTypeBinding)obj;

            return (PackageTypeID == p.PackageTypeID);
        }

        /// <summary>
        /// Hash code for PackageTypeBinding
        /// </summary>
        public override int GetHashCode()
        {
            return PackageTypeID;
        }

        /// <summary>
        /// Override == so that we compare by PackageTypeID
        /// </summary>
        public static bool operator ==(PackageTypeBinding x, PackageTypeBinding y)
        {
            // Check for null values and compare run-time types.
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.PackageTypeID == y.PackageTypeID;
        }

        /// <summary>
        /// Override != so that we compare by PackageTypeID
        /// </summary>
        public static bool operator !=(PackageTypeBinding x, PackageTypeBinding y)
        {
            return !(x == y);
        }
    }
}
