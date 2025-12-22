// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client
{
    public interface IConfigurationHolder
    {
        ISet<ClientEndpoint> ClientEndpoints { get; }
        void AddBinding(Binding binding);
        void AddClientEndpoint(string name, string address, string contract, string bindingType, string bindingName);
        Binding ResolveBinding(string bindingType, string name);
        IXmlConfigClientEndpoint GetXmlConfigClientEndpoint(ClientEndpoint endPoint);
    }
}
