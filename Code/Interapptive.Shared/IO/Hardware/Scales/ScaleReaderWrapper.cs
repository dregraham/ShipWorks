using System.Threading.Tasks;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Wrapper for the static class ScaleReader
    /// </summary>
    /// <seealso cref="Interapptive.Shared.IO.Hardware.Scales.IScaleReader" />
    public class ScaleReaderWrapper : IScaleReader
    {
        /// <summary>
        /// Reads the scale.
        /// </summary>
        public Task<ScaleReadResult> ReadScale() => ScaleReader.ReadScale();
    }
}