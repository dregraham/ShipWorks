using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the Core DimensionsManager
    /// </summary>
    public class DimensionsManagerWrapper : IDimensionsManager
    {
        private List<DimensionsProfileEntity> enterDimensionsProfileList;

        public DimensionsManagerWrapper()
        {
            enterDimensionsProfileList = new List<DimensionsProfileEntity> {
                new DimensionsProfileEntity()
                {
                    DimensionsProfileID = 0,
                    Name = "Enter Dimensions"
                }
            };
        }


        /// <summary>
        /// Return all the dimensions profiles
        /// </summary>
        public IEnumerable<DimensionsProfileEntity> Profiles(IPackageAdapter defaultPackageAdapter)
        {
            enterDimensionsProfileList = new List<DimensionsProfileEntity> {
                new DimensionsProfileEntity()
                {
                    DimensionsProfileID = 0,
                    Name = "Enter Dimensions",
                    Length = defaultPackageAdapter?.DimsLength ?? 0,
                    Width = defaultPackageAdapter?.DimsWidth ?? 0,
                    Height = defaultPackageAdapter?.DimsHeight ?? 0,
                    Weight = defaultPackageAdapter?.AdditionalWeight ?? 0
                }
            };

            return DimensionsManager.Profiles.Concat(enterDimensionsProfileList); 
        }
    }
}
