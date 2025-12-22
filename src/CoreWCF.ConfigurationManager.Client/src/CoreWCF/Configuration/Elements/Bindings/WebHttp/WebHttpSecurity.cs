// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client
{
    public sealed class WebHttpSecurity
    {
        internal const WebHttpSecurityMode DefaultMode = WebHttpSecurityMode.None;
        private WebHttpSecurityMode _mode;
        private HttpTransportSecurity _transportSecurity;

        public WebHttpSecurity()
        {
            _transportSecurity = new HttpTransportSecurity();
        }

        public WebHttpSecurityMode Mode
        {
            get { return _mode; }
            set
            {
                if (!WebHttpSecurityModeHelper.IsDefined(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _mode = value;
                IsModeSet = true;
            }
        }

        internal bool IsModeSet { get; private set; }

        public HttpTransportSecurity Transport
        {
            get { return _transportSecurity; }
            set
            {
                _transportSecurity = (value == null) ? new HttpTransportSecurity() : value;
            }
        }

        internal void DisableTransportAuthentication(HttpTransportBindingElement http)
        {
            HttpTransportHelpers.DisableTransportAuthentication(http);
        }

        internal void EnableTransportAuthentication(HttpTransportBindingElement http)
        {
            HttpTransportHelpers.ConfigureTransportAuthentication(http, _transportSecurity);
        }

        internal void EnableTransportSecurity(HttpsTransportBindingElement https)
        {
            HttpTransportHelpers.ConfigureTransportProtectionAndAuthentication(https, _transportSecurity);
        }

        internal void ApplyAuthorizationPolicySupport(HttpTransportBindingElement httpTransport)
        {
            ////TODO: This is not supported in System.ServiceModel;
            //httpTransport.AlwaysUseAuthorizationPolicySupport =
            //    Transport.AlwaysUseAuthorizationPolicySupport
            //    || Transport.ClientCredentialType == HttpClientCredentialType.InheritedFromHost;
        }
    }
}
