using System;

namespace ShipWorks.Installer.Services
{
    public class SampleService : ISampleService
    {
        public string GetCurrentDate()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}
