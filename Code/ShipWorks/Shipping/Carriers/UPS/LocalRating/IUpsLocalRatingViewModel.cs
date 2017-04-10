﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating
{
    /// <summary>
    /// Represents a Ups Local Rating ViewModel
    /// </summary>
    public interface IUpsLocalRatingViewModel
    {
        /// <summary>
        /// Loads the UpsAccount information to the view model
        /// </summary>
        void Load(UpsAccountEntity upsAccount);

        /// <summary>
        /// Saves view model information to the UpsAccount
        /// </summary>
        /// <returns>true when successful or false when register fails</returns>
        bool Save(UpsAccountEntity upsAccount);
    }
}