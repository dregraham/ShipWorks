using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Interapptive.Shared.IO.UnblockFiles
{
    /// <summary>
    /// Helper class for removing zone information from a file
    /// </summary>
    /// <seealso cref="https://github.com/arcdev/UnblockFiles"/>
    public static class ZoneHelper
    {
        /// <summary>
        /// Remove the zone from a given file
        /// </summary>
        public static int Remove(string filename)
        {
            IPersistFile persistFile = null;
            IZoneIdentifier zoneId = null;

            try
            {
                // need to cast because we can't directly implement the interface in C# code
                persistFile = (IPersistFile) new PersistentZoneIdentifier();
                const int mode = (int) (STGM.READWRITE | STGM.SHARE_EXCLUSIVE);

                URLZONE zone;
                try
                {
                    persistFile.Load(filename, mode);
                    // need to cast because we can't directly implement the interface in C# code
                    zoneId = (IZoneIdentifier) persistFile;
                    var getIdResult = zoneId.GetId(out zone);
                }
                catch (FileNotFoundException)
                {
                    zone = URLZONE.LOCAL_MACHINE;
                }
                catch (UnauthorizedAccessException)
                {
                    zone = URLZONE.INVALID;
                }

                if (zone == URLZONE.LOCAL_MACHINE || zone == URLZONE.INVALID)
                {
                    Debug.WriteLine($"Nothing to remove on '{filename}'");
                    return 0;
                }

                var removeResult = zoneId.Remove();

                persistFile.Save(filename, true);

                return removeResult;
            }
            finally
            {
                // don't forget to release the COM objects
                if (persistFile != null)
                {
                    Marshal.ReleaseComObject(persistFile);
                }

                if (zoneId != null)
                {
                    Marshal.ReleaseComObject(zoneId);
                }
            }
        }
    }
}