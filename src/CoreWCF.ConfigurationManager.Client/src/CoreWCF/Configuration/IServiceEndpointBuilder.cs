// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ServiceModel.Description;

namespace CoreWCF.ConfigurationManager.Client
{
    public interface IServiceEndpointBuilder 
    {
        ServiceEndpoint ConstructServiceEndPoint(string name);
    }
}
