using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Text
{
    /// <summary>
    /// StringWriter for providing a custom Encoding
    /// </summary>
    public class EncodingStringWriter : StringWriter
    {
        Encoding encoding = null;

        /// <summary>
        /// Return our custom encoding
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EncodingStringWriter(Encoding encoding)
        {
            this.encoding = encoding;
        }
    }
}
