using System;
using System.IO;
using System.Text;


namespace NDesk.Options
{
    public static class OptionSetExtensions
    {
        public static void WriteOptionDescriptions(this OptionSet instance, StringBuilder sb)
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            using (var writer = new StringWriter(sb))
                instance.WriteOptionDescriptions(writer);
        }
    }
}
