using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Step identifiers to be used to track progress with the Post2xMigrationUtility
    /// </summary>
    public enum Post2xMigrationStep
    {
        /// <summary>
        /// Indicates that all the steps that happen right after the v3 schema is current are done.  This is before
        /// the UI Post Migration UI steps\pages kick in.
        /// </summary>
        PostMigrationPreparation = 0,

        /// <summary>
        /// Configure all existing 2x users with 3x settings, ensure the SuperUser exists, and register the computer
        /// </summary>
        ConfigureInitialData = 1,

        /// <summary>
        /// Update all the EffectiveXXX fields for ebay order items
        /// </summary>
        UpdateEbayEffectiveFields = 2,

        /// <summary>
        /// Validate that all module URLs start with http or https
        /// </summary>
        ValidateModuleUrls = 3,

        /// <summary>
        /// Sets EndiciaAccount.EndiciaReseller for Express1.  See intermediate migration table v2m_Express1Account.
        /// </summary>
        UpdateExpress1Data = 4,

        /// <summary>
        /// Moves data from intermediate migration table v2m_MivaAttributeItem into MivaItemAttribute 
        /// </summary>
        MivaItemAttriteData = 5,
    }
}
