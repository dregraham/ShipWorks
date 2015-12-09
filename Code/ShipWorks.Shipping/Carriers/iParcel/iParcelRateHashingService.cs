using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelRateHashingService : RateHashingService
    {
        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.ShipmentFields.Add(IParcelShipmentFields.IParcelAccountID);
                ratingField.ShipmentFields.Add(IParcelShipmentFields.IsDeliveryDutyPaid);
                ratingField.ShipmentFields.Add(OrderFields.OrderTotal);
                ratingField.ShipmentFields.Add(OrderFields.RollupItemCount);
                ratingField.ShipmentFields.Add(IParcelShipmentFields.TrackByEmail);
                ratingField.ShipmentFields.Add(IParcelShipmentFields.TrackBySMS);

                ratingField.PackageFields.Add(IParcelPackageFields.Weight);
                ratingField.PackageFields.Add(IParcelPackageFields.DimsWeight);
                ratingField.PackageFields.Add(IParcelPackageFields.DimsWidth);
                ratingField.PackageFields.Add(IParcelPackageFields.DimsHeight);
                ratingField.PackageFields.Add(IParcelPackageFields.DimsLength);
                ratingField.PackageFields.Add(IParcelPackageFields.DeclaredValue);
                ratingField.PackageFields.Add(IParcelPackageFields.InsuranceValue);
                ratingField.PackageFields.Add(IParcelPackageFields.Insurance);
                ratingField.PackageFields.Add(IParcelPackageFields.InsurancePennyOne);

                return ratingField;
            }
        }
        
        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.IParcel.Packages);
        }

    }
}