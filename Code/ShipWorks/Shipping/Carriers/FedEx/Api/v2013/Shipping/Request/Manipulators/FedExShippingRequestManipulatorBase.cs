﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
{
    public abstract class FedExShippingRequestManipulatorBase : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;
        private readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingRequestManipulatorBase" /> class.
        /// </summary>
        protected FedExShippingRequestManipulatorBase()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingRequestManipulatorBase" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        protected FedExShippingRequestManipulatorBase(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingRequestManipulatorBase" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        protected FedExShippingRequestManipulatorBase(ICarrierSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
            fedExSettings = new FedExSettings(settingsRepository);
        }

        protected FedExSettings FedExSettings
        {
            get { return fedExSettings; }
        }

        protected ICarrierSettingsRepository SettingsRepository
        {
            get { return settingsRepository; }
        }

        public abstract void Manipulate(CarrierRequest request);

        /// <summary>
        /// Helper method to get shipment currency
        /// </summary>
        protected string GetShipmentCurrencyType(ShipmentEntity shipment)
        {
            return EnumHelper.GetApiValue(fedExSettings.GetCurrencyType(shipment));
        }
    }
}
