WCF Client Configuration (Library) limitations

Scope
- This document summarizes client configuration elements and attributes from the official WCF client config schema that are not supported by this library or are intentionally ignored.
- Reference: https://learn.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/wcf/client

General Notes
- The library focuses on a subset of WCF client features compatible with CoreWCF and System.ServiceModel on .NET (Core/5+/8/9) and .NET Framework 4.7.2, with intentional gaps for legacy or host-dependent features.
- Unsupported features are either ignored (default values are used) or mapped to safe fallbacks.

Bindings (client/bindings)

basicHttpBinding (element)
- Unsupported attributes
  - allowCookies (boolean): Ignored by this library; binding uses framework default (false). The configuration element exists but `HttpBindingBaseElement.AllowCookies` throws PlatformNotSupportedException.
  - bypassProxyOnLocal (boolean): Ignored; binding uses framework default (false). The configuration element exists but `HttpBindingBaseElement.BypassProxyOnLocal` throws PlatformNotSupportedException.
  - hostNameComparisonMode (enum): Not applied on netstandard2.0/.NET (CoreWCF). In this library it is not set on the runtime `BasicHttpBinding`. On .NET Framework, default may appear as StrongWildcard.
- Supported (for clarity)
  - proxyAddress, useDefaultWebProxy, timeouts, buffer sizes, messageEncoding, textEncoding, transferMode, readerQuotas are applied.

basicHttpBinding/security (child of basicHttpBinding)
- transport (child of security)
  - realm (string): Not applied by this library (assignment is commented out).
  - extendedProtectionPolicy: Not implemented on this path.
  - proxyCredentialType: Not configurable via config; binding uses framework default (None).

webHttpBinding (element)
- Unsupported attributes (defined in config element but not applied to runtime binding)
  - allowCookies (boolean)
  - bypassProxyOnLocal (boolean)
  - hostNameComparisonMode (enum)
  - proxyAddress (Uri)
  - useDefaultWebProxy (boolean)
- Notes
  - MaxBufferPoolSize, MaxBufferSize, MaxReceivedMessageSize, TransferMode, ReaderQuotas, WriteEncoding and timeouts are applied.

webHttpBinding/security (child of webHttpBinding)
- transport (child of security)
  - proxyCredentialType: Not configurable via config; binding uses framework default.
  - realm: Not applied (assignment is commented out in `HttpTransportSecurityElement`).
  - extendedProtectionPolicy: Not implemented on this path.
- Additional
  - AlwaysUseAuthorizationPolicySupport: Not supported.

customBinding (element)
- Unsupported child elements (presence results in configuration error)
  - compositeDuplex
  - pnrpPeerResolver
  - reliableSession
  - windowsStreamSecurity
  - sslStreamSecurity
  - transactionFlow

customBinding/security (child of customBinding)
- Unsupported attributes
  - allowSerializedSigningTokenOnReply: Not supported (commented out in `SecurityElementBase`).
  - messageProtectionOrder: Not supported by this library.

Transport/HTTP(S)
- Transport authentication configuration is limited to what CoreWCF/System.ServiceModel supports.
- Some advanced transport-level authorization policy knobs are intentionally ignored.

Proxy Settings
- basicHttpBinding: `proxyAddress` and `useDefaultWebProxy` are supported and applied; `bypassProxyOnLocal` is ignored.
- webHttpBinding: `proxyAddress`, `useDefaultWebProxy`, and `bypassProxyOnLocal` are not applied by this library.

Known Defaults Applied by Library
- BasicHttpBinding.AllowCookies: false (ignored setting).
- BasicHttpBinding.BypassProxyOnLocal: false (ignored setting).
- BasicHttpBinding.HostNameComparisonMode: default StrongWildcard may appear on .NET Framework; not applied on netstandard2.0/.NET.
- Missing or null transport security objects are created with defaults.

Compatibility Matrix
- .NET 8/9: Uses CoreWCF/System.ServiceModel client features where available; unsupported members are ignored.
- .NET Framework 4.7.2: Some legacy defaults (e.g., HostNameComparisonMode) may still appear, but unsupported toggles are not enforced by this library.

Notes for Consumers
- If a configuration attribute is not supported, the library will use the framework default or a safe fallback without throwing, unless the value is invalid by specification (e.g., undefined enum values).
- Prefer explicit runtime configuration for advanced features instead of config when targeting CoreWCF.
