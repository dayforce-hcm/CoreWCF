// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using CoreWCF.Runtime;
using SMHttpTransportSecurity = System.ServiceModel.HttpTransportSecurity;
using SMHttpClientCredentialType = System.ServiceModel.HttpClientCredentialType;

namespace CoreWCF.ConfigurationManager.Client
{
    internal static class HttpTransportHelpers
    {
        private const string DefaultRealm = HttpTransportDefaults.Realm;

        internal static void ConfigureTransportProtectionAndAuthentication(HttpsTransportBindingElement https, SMHttpTransportSecurity transportSecurity)
        {
            ConfigureAuthentication(https, transportSecurity);
            https.RequireClientCertificate = (transportSecurity.ClientCredentialType == SMHttpClientCredentialType.Certificate);
        }

        internal static void ConfigureTransportAuthentication(HttpTransportBindingElement http, SMHttpTransportSecurity transportSecurity)
        {
            if (transportSecurity.ClientCredentialType == SMHttpClientCredentialType.Certificate)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.CertificateUnsupportedForHttpTransportCredentialOnly));
            }

            ConfigureAuthentication(http, transportSecurity);
        }

        internal static void DisableTransportAuthentication(HttpTransportBindingElement http)
        {
            DisableAuthentication(http);
        }

        private static void ConfigureAuthentication(HttpTransportBindingElement http, SMHttpTransportSecurity transportSecurity)
        {
            http.AuthenticationScheme = MapToAuthenticationScheme(transportSecurity.ClientCredentialType);
            //http.Realm = transportSecurity.Realm;
            http.ExtendedProtectionPolicy = transportSecurity.ExtendedProtectionPolicy;
        }

        private static AuthenticationSchemes MapToAuthenticationScheme(SMHttpClientCredentialType clientCredentialType)
        {
            AuthenticationSchemes result;
            switch (clientCredentialType)
            {
                case SMHttpClientCredentialType.Certificate:
                // fall through to None case
                case SMHttpClientCredentialType.None:
                    result = AuthenticationSchemes.Anonymous;
                    break;
                case SMHttpClientCredentialType.Basic:
                    result = AuthenticationSchemes.Basic;
                    break;
                case SMHttpClientCredentialType.Digest:
                    result = AuthenticationSchemes.Digest;
                    break;
                case SMHttpClientCredentialType.Ntlm:
                    result = AuthenticationSchemes.Ntlm;
                    break;
                case SMHttpClientCredentialType.Windows:
                    result = AuthenticationSchemes.Negotiate;
                    break;
                case SMHttpClientCredentialType.InheritedFromHost:
                    result = AuthenticationSchemes.None;
                    break;
                default:
                    Fx.Assert("unsupported client credential type");
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
            }
            return result;
        }

        private static void DisableAuthentication(HttpTransportBindingElement http)
        {
            http.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            //http.Realm = DefaultRealm;
            //ExtendedProtectionPolicy is always copied - even for security mode None, Message and TransportWithMessageCredential,
            //because the settings for ExtendedProtectionPolicy are always below the <security><transport> element
            //http.ExtendedProtectionPolicy = extendedProtectionPolicy;
        }
    }
}
