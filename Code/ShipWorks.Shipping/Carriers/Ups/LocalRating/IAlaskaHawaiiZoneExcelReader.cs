using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interface for reading Alaska and Hawaii zones from an excel document
    /// </summary>
    public interface IAlaskaHawaiiZoneExcelReader
    {
        /// <summary>
        /// Get the Alaska Hawaii zones from the excel document
        /// </summary>
        IEnumerable<UpsLocalRatingZoneEntity> GetAlaskaHawaiiZones(IWorksheets zoneWorksheets);
    }
}