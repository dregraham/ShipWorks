using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// A pre-processor that allows can be called prior to processing a shipment.
    /// </summary>
    public class ShipmentTypePreProcessor
    {

        /// <summary>
        /// Uses the synchronizer to check whether an account exists and call the counterRatesProcessing callback
        /// provided when trying to process a shipment without any accounts for this shipment type in ShipWorks,
        /// otherwise the shipment is unchanged.
        /// </summary>
        /// <param name="synchronizer">The synchronizer.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="counterRatesProcessing">The counter rates processing.</param>
        /// <param name="selectedRate">The selected rate.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">An account must be created to process this shipment.</exception>
        public virtual List<ShipmentEntity> Run(IShipmentProcessingSynchronizer synchronizer, ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity> { shipment };

            if (synchronizer.HasAccounts)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }
            else
            {
                // Null values are passed because the rates don't matter for the general case; we're only
                // interested in grabbing the account that was just created
                CounterRatesProcessingArgs eventArgs = new CounterRatesProcessingArgs(null, null, shipment);

                // Invoke the counter rates callback
                if (counterRatesProcessing == null || counterRatesProcessing(eventArgs) != DialogResult.OK)
                {
                    // The user canceled, so we need to stop processing
                    shipments = null;
                }
                else
                {
                    // The user created an account, so try to grab the account and use it
                    // to process the shipment
                    ShippingSettings.CheckForChangesNeeded();
                    if (synchronizer.HasAccounts)
                    {
                        shipments.ForEach(s =>
                        {
                            // Assign the account ID and save the shipment
                            synchronizer.SaveAccountToShipment(s);
                            using (SqlAdapter adapter = new SqlAdapter(true))
                            {
                                adapter.SaveAndRefetch(s);
                                adapter.Commit();
                            }
                        });
                    }
                    else
                    {
                        // There still aren't any accounts for some reason, so throw an exception
                        throw new ShippingException("An account must be created to process this shipment.");
                    }
                }
            }

            return shipments;
        }
    }
}
