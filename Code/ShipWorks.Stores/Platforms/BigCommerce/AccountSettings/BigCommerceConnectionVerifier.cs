﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// Verify a connection to BigCommerce
    /// </summary>
    [Component]
    public class BigCommerceConnectionVerifier : IBigCommerceConnectionVerifier
    {
        readonly IBigCommerceWebClientFactory webClientFactory;
        readonly Func<BigCommerceStoreEntity, Owned<IBigCommerceStatusCodeProvider>> createStatusCodeProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceConnectionVerifier(IBigCommerceWebClientFactory webClientFactory,
            Func<BigCommerceStoreEntity, Owned<IBigCommerceStatusCodeProvider>> createStatusCodeProvider,
            Func<Type, ILog> createLogger)
        {
            this.createStatusCodeProvider = createStatusCodeProvider;
            this.webClientFactory = webClientFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Verify the connection
        /// </summary>
        public GenericResult<Unit> Verify(BigCommerceStoreEntity store, IBigCommerceAuthenticationPersistenceStrategy persistenceStrategy)
        {
            return ConnectionVerificationNeeded(store, persistenceStrategy) ?
                UpdateConnection(store) :
                GenericResult.FromSuccess(Unit.Default);
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        private static bool ConnectionVerificationNeeded(BigCommerceStoreEntity store, IBigCommerceAuthenticationPersistenceStrategy strategy)
        {
            return store.Fields[(int) BigCommerceStoreFieldIndex.ApiUrl].IsChanged ||
                strategy.ConnectionVerificationNeeded(store);
        }

        /// <summary>
        /// Update the connection information
        /// </summary>
        private GenericResult<Unit> UpdateConnection(BigCommerceStoreEntity store)
        {
            try
            {
                IBigCommerceWebClient webClient = webClientFactory.Create(store);
                webClient.TestConnection();

                using (Owned<IBigCommerceStatusCodeProvider> statusProvider = createStatusCodeProvider(store))
                {
                    statusProvider.Value.UpdateFromOnlineStore();
                }

                return GenericResult.FromSuccess(Unit.Default);
            }
            catch (BigCommerceException ex)
            {
                log.Error(ex);
                return GenericResult.FromError<Unit>(ex);
            }
        }
    }
}