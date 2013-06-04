using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Method for logging in
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum LogonMethod
    {
        [Description("User must type username")]
        TypeUsername = 0,

        [Description("Select username from a list")]
        SelectUsername = 1
    }
}
