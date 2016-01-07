using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class StoreLicense : ILicense
    {
        public void Activate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public string Key { get; set; }
        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public string DisabledReason { get; set; }
        public bool IsDisabled { get; set; }
    }
}