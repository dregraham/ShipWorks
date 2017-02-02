namespace ShipWorks.Common
{
    /// <summary>
    /// Generic manipulator
    /// </summary>
    public interface IManipulator<T>
    {
        /// <summary>
        /// Perform the manipulation
        /// </summary>
        T Manipulate(T input);
    }
}
