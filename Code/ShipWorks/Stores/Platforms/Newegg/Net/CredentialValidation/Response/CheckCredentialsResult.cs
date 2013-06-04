using System;
using System.Xml;
using System.Xml.Serialization;


namespace ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation.Response
{
    /// <summary>
    /// A data transport object for the results of an ICheckCredentialsRequest from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("NeweggAPIResponse")]
    public class CheckCredentialsResult
    {
        [XmlElement("IsSuccess")]
        public bool IsSuccessful { get; set; }
    }
}
