using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for dealing with Windows security
    /// </summary>
    public static class SecurityUtility
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
        [NDependIgnoreLongMethod]
        public static void AddPrivilegeToAccount(string domain, string userName, string privilegeName)
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
                const int access = (int) (NativeMethods.LsaAccessPolicy.PolicyAuditLogAdmin |
                                          NativeMethods.LsaAccessPolicy.PolicyCreateAccount |
                                          NativeMethods.LsaAccessPolicy.PolicyCreatePrivilege |
                                          NativeMethods.LsaAccessPolicy.PolicyCreateSecret |
                                          NativeMethods.LsaAccessPolicy.PolicyGetPrivateInformation |
                                          NativeMethods.LsaAccessPolicy.PolicyLookupNames |
                                          NativeMethods.LsaAccessPolicy.PolicyNotification |
                                          NativeMethods.LsaAccessPolicy.PolicyServerAdmin |
                                          NativeMethods.LsaAccessPolicy.PolicySetAuditRequirements |
                                          NativeMethods.LsaAccessPolicy.PolicySetDefaultQuotaLimits |
                                          NativeMethods.LsaAccessPolicy.PolicyTrustAdmin |
                                          NativeMethods.LsaAccessPolicy.PolicyViewAuditInformation |
                                          NativeMethods.LsaAccessPolicy.PolicyViewLocalInformation);

                IntPtr policyHandle; // initialize a pointer for the policy handle

                NativeMethods.LsaUnicodeString systemName = new NativeMethods.LsaUnicodeString(); // initialize an empty unicode-string

                // this variable and it's attributes are not used, but LsaOpenPolicy wants them to exists
                NativeMethods.LsaObjectAttributes ObjectAttributes = new NativeMethods.LsaObjectAttributes();

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

                    NativeMethods.LsaUnicodeString[] userRights = new NativeMethods.LsaUnicodeString[1];

                    userRights[0] = new NativeMethods.LsaUnicodeString
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

            if (errorMessages.Length > 0)
            {
                throw new Win32Exception(errorMessages.ToString());
            }
        }

    }
}
