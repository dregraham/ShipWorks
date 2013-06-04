using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// The result of trying to read from a scale
    /// </summary>
    public class ScaleReadResult
    {
        ScaleReadStatus status;
        string message;
        double weight;

        /// <summary>
        /// Constructor for success
        /// </summary>
        public ScaleReadResult(double weight)
        {
            this.weight = weight;
            this.status = ScaleReadStatus.Success;
        }

        /// <summary>
        /// Constructor for a problem
        /// </summary>
        public ScaleReadResult(ScaleReadStatus status, string message)
        {
            if (status == ScaleReadStatus.Success)
            {
                throw new InvalidOperationException("Wrong constructor to use for success.");
            }

            this.status = status;
            this.message = message;
        }

        /// <summary>
        /// The status of reading the scale.  Indicates success or reason for failure.
        /// </summary>
        public ScaleReadStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// The weight read from the scale, of status is Success
        /// </summary>
        public double Weight
        {
            get { return weight; }
        }

        /// <summary>
        /// The error message, if the status is not Success
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Custom equality semantics
        /// </summary>
        public override bool Equals(object obj)
        {
            ScaleReadResult other = obj as ScaleReadResult;
            if (other == null)
            {
                return false;
            }

            return 
                this.Status == other.Status &&
                this.Weight == other.Weight &&
                this.Message == other.Message;
        }

        /// <summary>
        /// Required when overriding Equals
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
