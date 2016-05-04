using System;
using Interapptive.Shared.Security;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class LicenseAesParams : IAesParams
    {
        private readonly IDatabaseIdentifier databaseId;

        public LicenseAesParams(IDatabaseIdentifier databaseId)
        {
            this.databaseId = databaseId;
        }

        public byte[] InitializationVector { get; } = {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14};

        public byte[] Key
        {
            get
            {
                try
                {
                    return databaseId.Get().ToByteArray();
                }
                catch (Exception ex)
                {
                    throw new EncryptionException(ex.Message, ex);
                }
            }
        }

        public string EmptyValue => "ShipWorks legacy user";
    }
}