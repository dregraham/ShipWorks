namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// Check the system results
    /// </summary>
    public class SystemCheckResult
    {
        /// <summary>
        /// RAM meets requirements
        /// </summary>
        public bool RamMeetsRequirement { get; set; } = true;

        /// <summary>
        /// Description why RAM didn't meet requirements
        /// </summary>
        public string RamDescription { get; set; }

        /// <summary>
        /// CPU meets requirements
        /// </summary>
        public bool CpuMeetsRequirement { get; set; } = true;

        /// <summary>
        /// Description why CPU didn't meet requirements
        /// </summary>

        public string CpuDescription { get; set; }

        /// <summary>
        /// HDD meets requirements
        /// </summary>
        public bool HddMeetsRequirement { get; set; } = true;

        /// <summary>
        /// Description why HDD didn't meet requirements
        /// </summary>

        public string HddDescription { get; set; }

        /// <summary>
        /// OS meets requirements
        /// </summary>
        public bool OsMeetsRequirement { get; set; } = true;

        /// <summary>
        /// Description why OS didn't meet requirements
        /// </summary>

        public string OsDescription { get; set; }
    }
}
