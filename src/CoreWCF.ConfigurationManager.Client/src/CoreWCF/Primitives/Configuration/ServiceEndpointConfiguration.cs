// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client
{
    internal class ClientEndpointConfiguration
    {
        public Uri Address { get; set; }
        public Binding Binding { get; set; }
        public Type Contract { get; set; }
        public Uri ListenUri { get; set; }
        public Action<ClientEndpoint> ConfigureEndpoint { get; set; }
    }
}
