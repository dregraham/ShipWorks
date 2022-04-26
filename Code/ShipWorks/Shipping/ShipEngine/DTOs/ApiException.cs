﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShipWorks.Shipping.ShipEngine.DTOs
//{
//    /// <summary>
//    /// API Exception
//    /// </summary>
//    public class ApiException : Exception
//    {
//        /// <summary>
//        /// Gets or sets the error code (HTTP status code)
//        /// </summary>
//        /// <value>The error code (HTTP status code).</value>
//        public int ErrorCode { get; set; }

//        /// <summary>
//        /// Gets or sets the error content (body json object)
//        /// </summary>
//        /// <value>The error content (Http response body).</value>
//        public dynamic ErrorContent { get; private set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ApiException"/> class.
//        /// </summary>
//        public ApiException() { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ApiException"/> class.
//        /// </summary>
//        /// <param name="errorCode">HTTP status code.</param>
//        /// <param name="message">Error message.</param>
//        public ApiException(int errorCode, string message) : base(message)
//        {
//            this.ErrorCode = errorCode;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ApiException"/> class.
//        /// </summary>
//        /// <param name="errorCode">HTTP status code.</param>
//        /// <param name="message">Error message.</param>
//        /// <param name="errorContent">Error content.</param>
//        public ApiException(int errorCode, string message, dynamic errorContent = null) : base(message)
//        {
//            this.ErrorCode = errorCode;
//            this.ErrorContent = errorContent;
//        }
//    }

//}
