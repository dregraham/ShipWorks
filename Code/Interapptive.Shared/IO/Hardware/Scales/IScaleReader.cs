using System.Threading.Tasks;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Reads the weight from an external scale
    /// </summary>
    public interface IScaleReader
    {
        /// <summary>
        /// Reads the scale.
        /// </summary>
        Task<ScaleReadResult> ReadScale();
    }
}