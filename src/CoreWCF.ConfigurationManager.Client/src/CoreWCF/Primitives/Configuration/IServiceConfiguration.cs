// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace CoreWCF.ConfigurationManager.Client
{
    internal interface IClientEndpointConfiguration<TClientEndpoint> : IClientEndpointConfiguration where TClientEndpoint : class
    {
    }

    internal interface IClientEndpointConfiguration
    {
        List<ClientEndpointConfiguration> Endpoints { get; }
        Type ClientEndpointType { get; }
    }
}
