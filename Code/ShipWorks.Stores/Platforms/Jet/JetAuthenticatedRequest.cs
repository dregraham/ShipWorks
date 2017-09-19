using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// When processing request, adds authentication info
    /// </summary>
    [Component]
    public class JetAuthenticatedRequest : IJetAuthenticatedRequest
    {
        private readonly IJsonRequest jsonRequest;
        private readonly IJetTokenRepository tokenRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetAuthenticatedRequest(IJsonRequest jsonRequest, IJetTokenRepository tokenRepo)
        {
            this.jsonRequest = jsonRequest;
            this.tokenRepo = tokenRepo;
        }

        /// <summary>
        /// Process the request. If an error is thrown, refresh the token and try again.
        /// </summary>
        public GenericResult<T> Submit<T>(string action, IHttpRequestSubmitter request, IJetStoreEntity store)
        {
            return ProcessRequest<T>(action, request, store, true);
        }

        /// <summary>
        /// Process the request. If an error is thrown, refresh the token and try again.
        /// </summary>
        private GenericResult<T> ProcessRequest<T>(string action, IHttpRequestSubmitter request, IJetStoreEntity store, bool generateNewTokenIfExpired)
        {
            try
            {
                IJetToken token = tokenRepo.GetToken(store);

                if (!token.IsValid)
                {
                    return GenericResult.FromError<T>("Unable to obtain a valid token to authenticate request.");
                }

                token.AttachTo(request);

                return GenericResult.FromSuccess(jsonRequest.Submit<T>(action, ApiLogSource.Jet, request));
            }
            catch (WebException ex) when (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized && generateNewTokenIfExpired)
            {
                tokenRepo.RemoveToken(store);
                return ProcessRequest<T>(action, request, store, false);
            }
            catch (Exception e)
            {
                return GenericResult.FromError<T>(e.Message);
            }
        }
    }
}
