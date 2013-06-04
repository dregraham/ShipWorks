using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Amazon.Dime
{
    /// <summary>
    /// DIME-aware web request
    /// </summary>
    [Serializable]
    public class DimeWebRequest : WebRequest
    {
        // the web request we're wrapping
        WebRequest originalRequest;

        // Attachments to be send on the request as DIME attachments
        List<DimeAttachment> attachments;

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected DimeWebRequest(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        /// <summary>
        /// Serialization
        /// </summary>
        protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            // nothing new to add
            base.GetObjectData(serializationInfo, streamingContext);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DimeWebRequest(WebRequest originalRequest, List<DimeAttachment> attachments)
        {
            this.originalRequest = originalRequest;
            this.attachments = attachments;
        }

        /// <summary>
        /// Gets the stream where the request will be written to.  Adding our own DIME-capable
        /// stream into the mix here.
        /// </summary>
        public override Stream GetRequestStream()
        {
            return new DimeInjectionStream(originalRequest.GetRequestStream(), attachments);
        }

        #region Forward all calls to the wrapped object

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return originalRequest.EndGetRequestStream(asyncResult);
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return originalRequest.BeginGetRequestStream(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return originalRequest.BeginGetResponse(callback, state);
        }

        public override void Abort()
        {
            originalRequest.Abort();
        }

        public override System.Net.Cache.RequestCachePolicy CachePolicy
        {
            get
            {
                return originalRequest.CachePolicy;
            }
            set
            {
                originalRequest.CachePolicy = value;
            }
        }

        public override string ConnectionGroupName
        {
            get
            {
                return originalRequest.ConnectionGroupName;
            }
            set
            {
                originalRequest.ConnectionGroupName = value;
            }
        }

        public override long ContentLength
        {
            get
            {
                return originalRequest.ContentLength;
            }
            set
            {
                originalRequest.ContentLength = value;
            }
        }

        public override string ContentType
        {
            get
            {
                return "application/dime";
            }
            set
            {
                originalRequest.ContentType = "application/dime";
            }
        }

        public override ICredentials Credentials
        {
            get
            {
                return originalRequest.Credentials;
            }
            set
            {
                originalRequest.Credentials = value;
            }
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return originalRequest.EndGetResponse(asyncResult);
        }




        public override WebResponse GetResponse()
        {
            return originalRequest.GetResponse();
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                return originalRequest.Headers;
            }
            set
            {
                originalRequest.Headers = value;
            }
        }

        public override string Method
        {
            get
            {
                return originalRequest.Method;
            }
            set
            {
                originalRequest.Method = value;
            }
        }



        public override bool PreAuthenticate
        {
            get
            {
                return originalRequest.PreAuthenticate;
            }
            set
            {
                originalRequest.PreAuthenticate = value;
            }
        }


        public override IWebProxy Proxy
        {
            get
            {
                return originalRequest.Proxy;
            }
            set
            {
                originalRequest.Proxy = value;
            }
        }

        public override Uri RequestUri
        {
            get
            {
                return originalRequest.RequestUri;
            }
        }

        public override int Timeout
        {
            get
            {
                return originalRequest.Timeout;
            }
            set
            {
                originalRequest.Timeout = value;
            }
        }

        public override bool UseDefaultCredentials
        {
            get
            {
                return originalRequest.UseDefaultCredentials;
            }
            set
            {
                originalRequest.UseDefaultCredentials = value;
            }
        }

        #endregion
    }
}
