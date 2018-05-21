namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Account
{
    /// <summary>
    /// Data structure for new endicia accounts credentials
    /// </summary>
    public struct EndiciaNewAccountCredentials
    {
        /// <summary>
        /// Internet Password
        /// </summary>
        public string WebPassword { get; set; }

        /// <summary>
        /// Softare Password
        /// </summary>
        public string TemporaryPassPhrase { get; set; }

        /// <summary>
        /// Challenge Question
        /// </summary>
        public string ChallengeQuestion { get; set; }

        /// <summary>
        /// Challenge Answer
        /// </summary>
        public string ChallengeAnswer { get; set; }
    }
}
