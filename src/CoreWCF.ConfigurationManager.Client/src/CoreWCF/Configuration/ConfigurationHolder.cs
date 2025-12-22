// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ConfigurationHolder : IConfigurationHolder
    {
        private readonly IDictionary<string, Binding> _bindings = new Dictionary<string, Binding>();
        private readonly ISet<ClientEndpoint> _clientEndpoints = new HashSet<ClientEndpoint>();
        private readonly IServiceProvider _provider;
        private readonly IBindingFactory _factoryBinding;

        public ISet<ClientEndpoint> ClientEndpoints => _clientEndpoints;

        public ConfigurationHolder(IServiceProvider serviceProvider, IBindingFactory factory)
        {
            _provider = serviceProvider;
            _factoryBinding = factory;
        }

        public void AddBinding(Binding binding)
        {
            _bindings.Add(binding.Name, binding);
        }

        public Binding ResolveBinding(string bindingType, string name)
        {
            if (string.IsNullOrEmpty(bindingType))
            {
                throw new ArgumentNullException(bindingType);
            }

            Binding binding;
            if (string.IsNullOrEmpty(name))
            {
                binding = _factoryBinding.Create(bindingType);
            }
            else if (!_bindings.TryGetValue(name, out binding))
            {
                throw new BindingNotFoundException();
            }

            return binding;

        }

        public IXmlConfigClientEndpoint GetXmlConfigClientEndpoint(ClientEndpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            Type contract = ServiceReflector.ResolveTypeFromName(endpoint.Contract);
            Binding binding = ResolveBinding(endpoint.Binding, endpoint.BindingConfiguration);
            return new XmlConfigClientEndpoint(contract, binding, endpoint.Address);
        }

        public void AddClientEndpoint(string name, string address, string contract, string bindingType, string bindingName)
        {
            var endpoint = new ClientEndpoint
            {
                Address = address,
                Binding = bindingType,
                Contract = contract,
                Name = name,
                BindingConfiguration = bindingName
                
            };

            _clientEndpoints.Add(endpoint);
        }        
    }
}
