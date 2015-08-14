using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Class to encapsulate the parameters needed to pass to FedEx shipping clerk constructors.
    /// </summary>
    public class FedExShippingClerkParameters
    {
        private bool hasDoneVersionCapture;
        private bool forceVersionCapture;
        private ILabelRepository labelRepository;
        private IFedExRequestFactory requestFactory;
        private ICarrierSettingsRepository settingsRepository;
        private ICertificateInspector certificateInspector;
        private ILog log;
        private IExcludedServiceTypeRepository excludedServiceTypeRepository;
        
        /// <summary>
        /// Notes if version capture has been done
        /// </summary>
        public bool HasDoneVersionCapture
        {
            get { return hasDoneVersionCapture; }
            set { hasDoneVersionCapture = value; }
        }

        /// <summary>
        /// Notes if version capture should be forced
        /// </summary>
        public bool ForceVersionCapture
        {
            get { return forceVersionCapture; }
            set { forceVersionCapture = value; }
        }

        /// <summary>
        /// Label repository for the shipping clerk
        /// </summary>
        public ILabelRepository LabelRepository
        {
            get { return labelRepository; }
            set { labelRepository = value; }
        }

        /// <summary>
        /// Response factory for the shipping clerk
        /// </summary>
        public IFedExRequestFactory RequestFactory
        {
            get { return requestFactory; }
            set { requestFactory = value; }
        }

        /// <summary>
        /// Settings repository for the shipping clerk
        /// </summary>
        public ICarrierSettingsRepository SettingsRepository
        {
            get { return settingsRepository; }
            set { settingsRepository = value; }
        }

        /// <summary>
        /// Certificate inspector for the shipping clerk
        /// </summary>
        public ICertificateInspector Inspector
        {
            get { return certificateInspector; }
            set { certificateInspector = value; }
        }

        /// <summary>
        /// Log for the shipping clerk
        /// </summary>
        public ILog Log
        {
            get { return log; }
            set { log = value; }
        }

        /// <summary>
        /// Excluded Service Type Repository for the shipping clerk
        /// </summary>
        public IExcludedServiceTypeRepository ExcludedServiceTypeRepository
        {
            get { return excludedServiceTypeRepository; }
            set { excludedServiceTypeRepository = value; }
        }
    }
}
