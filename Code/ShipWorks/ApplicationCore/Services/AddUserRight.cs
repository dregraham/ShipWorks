using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// The Local Security Authority (LSA) Utility
    /// </summary>
    public static class AddUserRight
    {
        /// <summary>
        /// Adds a privilege to an account
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="userName">Name of an account - "domain\account" or only "account"</param>
        /// <param name="privilegeName">Name of the privilege</param>
        /// <returns>
        /// The windows error code returned by LsaAddAccountRights
        /// </returns>
        public static void SetRight(string domain, string userName, string privilegeName)
        {
            if (!string.IsNullOrWhiteSpace(domain))
            {
                userName = string.Format("{0}\\{1}", domain, userName);
            }

            int accountType = 0; // account-type variable for lookup
            int nameSize = 0; // size of the domain name
            int sidSize = 0; // size of the SID
            IntPtr sid = IntPtr.Zero; // pointer for the SID
            Int32 winErrorCode = 0; // contains the last error
            StringBuilder domainName = null; // StringBuilder for the domain name
            StringBuilder errorMessages = new StringBuilder();

            // get required buffer size
            NativeMethods.LookupAccountName(null, userName, sid, ref sidSize, null, ref nameSize, ref accountType);

            // allocate buffers
            domainName = new StringBuilder(nameSize);
            sid = Marshal.AllocHGlobal(sidSize);

            // lookup the SID for the account
            if (!NativeMethods.LookupAccountName(null, userName, sid, ref sidSize, domainName, ref nameSize, ref accountType))
            {
                winErrorCode = Marshal.GetLastWin32Error();
                errorMessages.AppendLine("An error occurred while Looking up the Account Name, Error Code: " + winErrorCode);
            }
            else
            { 
                // combine all policies
                const int access = (int) (LsaAccessPolicy.PolicyAuditLogAdmin |
                                          LsaAccessPolicy.PolicyCreateAccount |
                                          LsaAccessPolicy.PolicyCreatePrivilege |
                                          LsaAccessPolicy.PolicyCreateSecret |
                                          LsaAccessPolicy.PolicyGetPrivateInformation |
                                          LsaAccessPolicy.PolicyLookupNames |
                                          LsaAccessPolicy.PolicyNotification |
                                          LsaAccessPolicy.PolicyServerAdmin |
                                          LsaAccessPolicy.PolicySetAuditRequirements |
                                          LsaAccessPolicy.PolicySetDefaultQuotaLimits |
                                          LsaAccessPolicy.PolicyTrustAdmin |
                                          LsaAccessPolicy.PolicyViewAuditInformation |
                                          LsaAccessPolicy.PolicyViewLocalInformation);

                IntPtr policyHandle; // initialize a pointer for the policy handle

                LsaUnicodeString systemName = new LsaUnicodeString(); // initialize an empty unicode-string

                // this variable and it's attributes are not used, but LsaOpenPolicy wants them to exists
                LsaObjectAttributes ObjectAttributes = new LsaObjectAttributes();

                // get a policy handle
                Int32 openPolicyResultStatus = NativeMethods.LsaOpenPolicy(ref systemName, ref ObjectAttributes, access, out policyHandle);
                winErrorCode = NativeMethods.LsaNtStatusToWinError(openPolicyResultStatus);

                if (winErrorCode != 0)
                {
                    errorMessages.AppendLine("An error occurred while opening the policy, Error Code: " + winErrorCode);
                }
                else // Now that we have the SID and the policy, we can add the right to the account.
                {
                    // initialize a unicode-string for the privilege name

                    LsaUnicodeString[] userRights = new LsaUnicodeString[1];

                    userRights[0] = new LsaUnicodeString
                    {
                        Buffer = Marshal.StringToHGlobalUni(privilegeName), 
                        Length = (UInt16) (privilegeName.Length * UnicodeEncoding.CharSize), 
                        MaximumLength = (UInt16) ((privilegeName.Length + 1) * UnicodeEncoding.CharSize)
                    };

                    // add the right to the account
                    Int32 addAccountResultStatus = NativeMethods.LsaAddAccountRights(policyHandle, sid, userRights, 1);

                    winErrorCode = NativeMethods.LsaNtStatusToWinError(addAccountResultStatus);

                    if (winErrorCode != 0)
                    {
                        errorMessages.AppendLine("An error occurred while adding the account right, Error Code: " + winErrorCode);
                    }

                    Marshal.FreeHGlobal(userRights[0].Buffer);
                    winErrorCode = NativeMethods.LsaClose(policyHandle);

                    if (winErrorCode != 0)
                    {
                        errorMessages.AppendLine("An error occurred while Closing policy, Error Code: " + winErrorCode);
                    }
                }
                NativeMethods.FreeSid(sid);
            }

            if (errorMessages.Length>0)
            {
                throw new ShipWorksServiceException(errorMessages.ToString());
            }
        }

        /// <summary>
        /// Methods used to change update user accounts
        /// 
        /// Includes methods using P/Invokes, Code analysis states it needs to be in a seperate class.
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("advapi32.dll", PreserveSig = true)]
            internal static extern Int32 LsaOpenPolicy(ref LsaUnicodeString SystemName, ref LsaObjectAttributes ObjectAttributes, Int32 DesiredAccess, out IntPtr PolicyHandle);

            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, PreserveSig = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool LookupAccountName([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpAccountName, IntPtr psid, ref int cbsid, StringBuilder domainName, ref int cbdomainLength, ref int use);

            [DllImport("advapi32.dll", SetLastError = true, PreserveSig = true)]
            public static extern Int32 LsaAddAccountRights(IntPtr PolicyHandle, IntPtr AccountSid, LsaUnicodeString[] UserRights, int CountOfRights);

            [DllImport("advapi32")]
            public static extern IntPtr FreeSid(IntPtr pSid);

            [DllImport("advapi32.dll")]
            public static extern Int32 LsaClose(IntPtr ObjectHandle);

            [DllImport("advapi32.dll")]
            public static extern Int32 LsaNtStatusToWinError(Int32 status);
        }

        /// <summary>
        /// Policy Enumeration
        /// </summary>
        [Flags]
        private enum LsaAccessPolicy : long
        {
            PolicyViewLocalInformation = 0x00000001L,
            PolicyViewAuditInformation = 0x00000002L,
            PolicyGetPrivateInformation = 0x00000004L,
            PolicyTrustAdmin = 0x00000008L,
            PolicyCreateAccount = 0x00000010L,
            PolicyCreateSecret = 0x00000020L,
            PolicyCreatePrivilege = 0x00000040L,
            PolicySetDefaultQuotaLimits = 0x00000080L,
            PolicySetAuditRequirements = 0x00000100L,
            PolicyAuditLogAdmin = 0x00000200L,
            PolicyServerAdmin = 0x00000400L,
            PolicyLookupNames = 0x00000800L,
            PolicyNotification = 0x00001000L
        }

        /// <summary>
        /// Passed to LsaOpenPolicy. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct LsaObjectAttributes
        {
            private readonly int Attributes;
            private readonly int Length;
            private readonly IntPtr RootDirectory;
            private readonly IntPtr SecurityDescriptor;
            private readonly IntPtr SecurityQualityOfService;
            private readonly LsaUnicodeString ObjectName;
        }

        /// <summary>
        /// Used in LsaObjectAttributes.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct LsaUnicodeString : IDisposable
        {
            public UInt16 Length;
            public UInt16 MaximumLength;

            /// <summary>
            /// NOTE: Buffer has to be declared after Length and MaximumLength;
            /// otherwise, you will get winErrorCode: 1734 (The array bounds are invalid.)
            /// and waste lots of time trying to track down what causes the error!
            /// </summary>
            public IntPtr Buffer;

            public void Dispose()
            {
                Marshal.FreeHGlobal(Buffer);
            }
        }
    } 
} 