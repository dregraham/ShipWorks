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

                ratingField.ShipmentFields.Add(IParcelPackageFields.Weight);
                ratingField.ShipmentFields.Add(IParcelPackageFields.DimsWeight);
                ratingField.ShipmentFields.Add(IParcelPackageFields.DimsWidth);
                ratingField.ShipmentFields.Add(IParcelPackageFields.DimsHeight);
                ratingField.ShipmentFields.Add(IParcelPackageFields.DimsLength);
                ratingField.ShipmentFields.Add(IParcelPackageFields.DeclaredValue);
                ratingField.ShipmentFields.Add(IParcelPackageFields.InsuranceValue);
                ratingField.ShipmentFields.Add(IParcelPackageFields.Insurance);
                ratingField.ShipmentFields.Add(IParcelPackageFields.InsurancePennyOne);

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