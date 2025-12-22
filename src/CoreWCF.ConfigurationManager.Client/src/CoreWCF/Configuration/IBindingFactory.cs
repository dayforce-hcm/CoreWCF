// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client
{
    public interface IBindingFactory
    {
        Binding Create(string bindingType);
    }
}

