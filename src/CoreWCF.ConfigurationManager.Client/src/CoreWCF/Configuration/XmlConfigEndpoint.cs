// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client
{
    internal class XmlConfigClientEndpoint : IXmlConfigClientEndpoint
    {
        public Uri Address { get; private set; }
        public Binding Binding { get; private set; }
        public Type Contract { get; private set; }

        public XmlConfigClientEndpoint(Type contract, Binding binding, Uri address)
        {
            Contract = contract;
            Binding = binding;
            Address = address;
        }
    }
}
