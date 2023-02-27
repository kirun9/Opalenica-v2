namespace Opalenica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

internal static class Token
{
	[DllImport("advapi32.dll", SetLastError = true)]
	static extern bool GetTokenInformation(IntPtr tokenHandle, TOKEN_INFORMATION_CLASS tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength);

	enum TOKEN_INFORMATION_CLASS // from winnt.h
	{
		TokenUser = 1,
		TokenGroups,
		TokenPrivileges,
		TokenOwner,
		TokenPrimaryGroup,
		TokenDefaultDacl,
		TokenSource,
		TokenType,
		TokenImpersonationLevel,
		TokenStatistics,
		TokenRestrictedSids,
		TokenSessionId,
		TokenGroupsAndPrivileges,
		TokenSessionReference,
		TokenSandBoxInert,
		TokenAuditPolicy,
		TokenOrigin,
		TokenElevationType,
		TokenLinkedToken,
		TokenElevation,
		TokenHasRestrictions,
		TokenAccessInformation,
		TokenVirtualizationAllowed,
		TokenVirtualizationEnabled,
		TokenIntegrityLevel,
		TokenUIAccess,
		TokenMandatoryPolicy,
		TokenLogonSid,
		TokenIsAppContainer,
		TokenCapabilities,
		TokenAppContainerSid,
		TokenAppContainerNumber,
		TokenUserClaimAttributes,
		TokenDeviceClaimAttributes,
		TokenRestrictedUserClaimAttributes,
		TokenRestrictedDeviceClaimAttributes,
		TokenDeviceGroups,
		TokenRestrictedDeviceGroups,
		TokenSecurityAttributes,
		TokenIsRestricted,
		TokenProcessTrustLevel,
		TokenPrivateNameSpace,
		TokenSingletonAttributes,
		TokenBnoIsolation,
		TokenChildProcessFlags,
		TokenIsLessPrivilegedAppContainer,
		TokenIsSandboxed,
		TokenIsAppSilo,
		MaxTokenInfoClass
	}

	/// <summary>
	/// The elevation type for a user token.
	/// </summary>
	enum TokenElevationType
	{
		TokenElevationTypeDefault = 1,
		TokenElevationTypeFull,
		TokenElevationTypeLimited
	}

	public static bool IsElevated()
	{
		using var identity = WindowsIdentity.GetCurrent();
		if (identity is null) throw new InvalidOperationException("Couldn't get the current user identity");
		var principal = new WindowsPrincipal(identity);

		// Check if this user has the Administrator role. Id they do, return immediately.
		// If UAC is on, and the process is not elevated, then this will actually retutn false
		if (principal.IsInRole(WindowsBuiltInRole.Administrator)) return true;

		// If we are not running in Vista onwards, we don't have to worry about checking for UAC
		if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 6)
			// Operating system does not support UAC, skipping elevation check
			return false;

		int tokenInfLength = Marshal.SizeOf<int>();
		IntPtr tokenInformation = Marshal.AllocHGlobal(tokenInfLength);
		try
		{
			var token = identity.Token;
			var result = GetTokenInformation(token, TOKEN_INFORMATION_CLASS.TokenElevationType, tokenInformation, tokenInfLength, out tokenInfLength);
			if (!result)
			{
				var exception = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
				throw new InvalidOperationException("Unable to get token information. Win32 Error Code: " + Marshal.GetLastWin32Error(), exception);
			}
			var elevationType = (TokenElevationType)Marshal.ReadInt32(tokenInformation);

			return elevationType switch
			{
				TokenElevationType.TokenElevationTypeFull or TokenElevationType.TokenElevationTypeLimited => true,
				TokenElevationType.TokenElevationTypeDefault or _ => false,
			};
		}
		finally
		{
			if (tokenInformation != IntPtr.Zero) Marshal.FreeHGlobal(tokenInformation);
		}
	}
}
