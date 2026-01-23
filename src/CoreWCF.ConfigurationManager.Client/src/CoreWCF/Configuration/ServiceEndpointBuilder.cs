// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Options;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ServiceEndpointBuilder : /*CommunicationObject, */IServiceEndpointBuilder
    {        
        private readonly IConfigurationHolder _configHolder;
        private readonly ClientModelOptions _options;
        public ServiceEndpointBuilder(IConfigurationHolder configHolder, IOptions<ClientModelOptions> options)
        {
            _configHolder = configHolder;
            _options = options.Value;
        }
        public ServiceEndpoint ConstructServiceEndPoint(string name)
        {
            ClientEndpoint clientEndpointConfiguration = _configHolder.ClientEndpoints.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (clientEndpointConfiguration == null)
            {
                throw new ArgumentException(SR.Format(SR.UnknownClientEndpointConfiguration, name));
            }

            Type contract = ServiceReflector.ResolveTypeFromName(clientEndpointConfiguration.Contract);
            Binding binding = _configHolder.ResolveBinding(clientEndpointConfiguration.Binding, clientEndpointConfiguration.BindingConfiguration);
            
            var serviceEndpoint = new ServiceEndpoint(
                ContractDescription.GetContract(contract), 
                binding, 
                new System.ServiceModel.EndpointAddress(clientEndpointConfiguration.Address)
            );

            return serviceEndpoint;
        }
    }
}
