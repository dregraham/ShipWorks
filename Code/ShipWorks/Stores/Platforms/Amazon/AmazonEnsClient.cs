using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Amazon.WebServices.EventNotification;
using System.IdentityModel.Tokens;
using System.Web.Services.Protocols;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Net.Security;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Dispatcher;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;
using Interapptive.Shared.Win32;
using System.Net;
using ShipWorks.ApplicationCore;
using System.Security.Cryptography;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Web client for the Amazon Event Notification Service
    /// </summary>
    public class AmazonEnsClient
    {
        // the store instance the client is operating for
        AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonEnsClient(AmazonStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// The default number of orders to download per request
        /// </summary>
        public static int EventsPerPage
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("amazonEventsPerPage", 100);
            }
        }

        /// <summary>
        /// Tests connectivity and authentication with the Amazon Event Notification service 
        /// </summary>
        public static void TestConnection(ClientCertificate certificate)
        {
            EventNotificationServiceWSE service = CreateService(certificate, "TestConnection");
            GetEvents getEvents = new GetEvents();
            getEvents.EventType = "ssn";

            getEvents.StartDate = DateTime.Now.AddDays(-1);
            getEvents.StartDateSpecified = true;

            getEvents.EventsPerPage = 1;
            getEvents.EventsPerPageSpecified = true;

            getEvents.Limit = 1;
            getEvents.LimitSpecified = true;

            try
            {
                service.GetEvents(getEvents);
            }
            catch (SoapException ex)
            {
                CheckAuthFailure(ex);

                throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
            }
            catch (AsynchronousOperationException ex)
            {
                throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Creates the web service proxy for the Amazon Event Notification Service
        /// </summary>
        private static EventNotificationServiceWSE CreateService(ClientCertificate certificate, string logName)
        {
            EventNotificationServiceWSE service = new EventNotificationServiceWSE();

            // configure security
            AmazonWse.ConfigureWse(service, certificate, new ApiLogEntry(ApiLogSource.Amazon, logName));

            return service;
        }

        /// <summary>
        /// Retrieve the next page of orders from Amazon
        /// </summary>
        public string[] DownloadOrders(DateTime? lastOrderDate)
        {
            // see if we have a paging cookie
            if (store.Cookie.Length > 0)
            {
                // see if we need to wait more time
                if (store.CookieWaitUntil > DateTime.UtcNow)
                {
                    // Just say we didn't get anything
                    return new string[] { };
                }
            }

            try
            {
                // If we don't have a cookie or its expired, get fresh
                if (store.Cookie.Length == 0 || store.CookieExpires <= DateTime.UtcNow)
                {
                    return DownloadOrdersFresh(lastOrderDate);
                }
                else
                {
                    // valid cookie, so use it to retrieve orders
                    try
                    {
                        return DownloadOrdersWithCookie();
                    }
                    catch (SoapException ex)
                    {
                        if (ex.Message.ToLower(CultureInfo.InvariantCulture).IndexOf("cookie") != -1)
                        {
                            return DownloadOrdersFresh(lastOrderDate);
                        }

                        throw;
                    }
                }
            }
            catch (CryptographicException ex)
            {
                throw new AmazonException("There is a problem with your Amazon Certificate:\n\n" + ex.Message, ex);
            }
            catch (SoapException ex)
            {
                CheckAuthFailure(ex);

                throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
            }
            catch (AsynchronousOperationException ex)
            {
                throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    throw new AmazonException(typeof(EventNotificationServiceWSE), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Download orders on a second pass when we have a cookie to use
        /// </summary>
        private string[] DownloadOrdersWithCookie()
        {
            ClientCertificate certificate = new ClientCertificate();
            certificate.Import(store.Certificate);

            EventNotificationServiceWSE service = CreateService(certificate, "GetOrdersWithCookie");

            PagingCookie cookie = new PagingCookie();
            cookie.Cookie = store.Cookie;

            GetMore getMore = new GetMore();
            getMore.PagingCookie = cookie;

            getMore.EventsPerPage = Convert.ToUInt32(EventsPerPage);
            getMore.EventsPerPageSpecified = true;

            EventNotificationPage response = service.GetMore(getMore).Results;

            PagingCookie pagingCookie = response.PagingCookie;
            store.Cookie = pagingCookie.Cookie;
            store.CookieExpires = DateTime.UtcNow + SoapDuration.Parse(pagingCookie.TimeToLive) - TimeSpan.FromMinutes(5);
            store.CookieWaitUntil = DateTime.UtcNow + SoapDuration.Parse(pagingCookie.BackOffTime);

            return ConvertToXmlArray(response.EventNotifications, service.Pipeline.GetInputBehavior<ApiLogEntry>());
        }

        /// <summary>
        /// Download a fresh set of orders since we don't have a cookie
        /// </summary>
        private string[] DownloadOrdersFresh(DateTime? lastOrderDate)
        {
            ClientCertificate certificate = new ClientCertificate();
            certificate.Import(store.Certificate);

            EventNotificationServiceWSE service = CreateService(certificate, "GetOrders");

            GetEvents getEvents = new GetEvents();
            getEvents.EventType = "ssn";

            // if there are no orders, go back a long while
            if (!lastOrderDate.HasValue)
            {
                getEvents.StartDate = DateTime.UtcNow.AddYears(-2);
                getEvents.StartDateSpecified = true;
            }
            else
            {
                getEvents.StartDate = lastOrderDate.Value;
                getEvents.StartDateSpecified = true;
            }

            getEvents.EventsPerPage = Convert.ToUInt32(EventsPerPage);
            getEvents.EventsPerPageSpecified = true;

            getEvents.Limit = 0;
            getEvents.LimitSpecified = true;

            EventNotificationPage response = service.GetEvents(getEvents).Results;

            PagingCookie pagingCookie = response.PagingCookie;
            store.Cookie = pagingCookie.Cookie;
            store.CookieExpires = DateTime.UtcNow + SoapDuration.Parse(pagingCookie.TimeToLive) - TimeSpan.FromMinutes(5);
            store.CookieWaitUntil = DateTime.UtcNow + SoapDuration.Parse(pagingCookie.BackOffTime);

            return ConvertToXmlArray(response.EventNotifications, service.Pipeline.GetInputBehavior<ApiLogEntry>());
        }

        /// <summary>
        /// Convert the given array of order objects into xml strings
        /// </summary>
        private string[] ConvertToXmlArray(EventNotification[] events, ApiLogEntry logEntry)
        {
            if (events == null)
            {
                return new string[0];
            }

            string[] xmlArray = new string[events.Length];

            for (int i = 0; i < events.Length; i++)
            {
                xmlArray[i] = events[i].EventBody;

                logEntry.LogResponseSupplement(xmlArray[i], string.Format("Order[{0}]", i));
            }

            return xmlArray;
        }

        /// <summary>
        /// Looks for an authorization failure and raises a more useful exception
        /// </summary>
        private static void CheckAuthFailure(SoapException ex)
        {
            if (ex.Code.Name == "Client.AuthFailure")
            {
                throw new AmazonException("The X.509 certificate for your store has expired or has been removed from your Amazon account.", ex);
            }
        }
    }
}
