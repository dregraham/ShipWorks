using System;
using System.IO;
using System.Text;


namespace NDesk.Options
{
    /// <summary>
    /// Extension methods for the OptionSet class
    /// </summary>
    public static class OptionSetExtensions
    {
        /// <summary>
        /// Writes the option descriptions using a string builder
        /// </summary>
        /// <param name="instance">Option set for which to write</param>
        /// <param name="sb">String builder into which the description will be written</param>
        public static void WriteOptionDescriptions(this OptionSet instance, StringBuilder sb)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("instance");
            }
            
            using (var writer = new StringWriter(sb))
            {
                instance.WriteOptionDescriptions(writer);
            }
        }
    }
}
