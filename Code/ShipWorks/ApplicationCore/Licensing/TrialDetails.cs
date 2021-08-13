using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class TrialDetails
    {
        public TrialDetails() : this(false, DateTime.MinValue)
        {
        }
        
        public TrialDetails(bool isInTrial, DateTime endDate)
        {
            IsInTrial = isInTrial;
            EndDate = endDate;
        }
        
        /// <summary>
        /// Whether or not 
        /// </summary>
        public bool IsInTrial { get; set; }
        
        /// <summary>
        /// The end date of the Recurly trial
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// How many days are left in the trial
        /// </summary>
        public int DaysLeftInTrial => (EndDate - DateTime.UtcNow).Days;

        /// <summary>
        /// Whether or not the trial is expired
        /// </summary>
        public bool IsExpired => DaysLeftInTrial < 0;
    }
}