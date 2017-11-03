namespace ShipWorks.Shipping.Carriers.FedEx.Api.Environment
{
    /// <summary>
    /// FedEx web service versions
    /// </summary>
    public static class FedExWebServiceVersions
    {
        /// <summary>
        /// Gets the major version of the Ship WebService
        /// </summary>
        public static string Ship =>
            new WebServices.Ship.VersionId().Major.ToString();
    }
}
