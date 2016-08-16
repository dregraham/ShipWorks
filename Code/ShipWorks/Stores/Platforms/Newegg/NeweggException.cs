using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using ShipWorks.Stores.Platforms.Newegg.Net;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class NeweggException : Exception
    {
        /// <summary>
        /// If the exception stems from an invalid or unexpected response, the response will be here.
        /// </summary>
        private NeweggResponse badResponse;

        /// <summary>
        /// Invalid response was returned by Newegg
        /// </summary>
        public Net.NeweggResponse BadResponse
        {
            get { return badResponse; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NeweggException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor for specifying the response from Newegg wasn't valid
        /// </summary>
        public NeweggException(string message, NeweggResponse badResponse)
            : base(message)
        {
            this.badResponse = badResponse;
        }

        /// <summary>
        /// Constructor for just message
        /// </summary>
        public NeweggException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected NeweggException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            badResponse = (NeweggResponse)info.GetValue("badResponse", typeof(NeweggResponse));
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        ///   </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("badResponse", badResponse);
        }
    }
}
