using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ClientEndpointConfigurationBuilder
    {
        private readonly List<Action<ClientEndpointConfigurationBuilder>> _configDelegates = new List<Action<ClientEndpointConfigurationBuilder>>();
        private List<ClientEndpointConfiguration> _endpoints = new List<ClientEndpointConfiguration>();
        private Type _endpointType;

        public ClientEndpointConfigurationBuilder(Type serviceType)
        {
            _endpointType = serviceType;
        }

        public void Configure(Action<ClientEndpointConfigurationBuilder> configDelegate)
        {
            _configDelegates.Add(configDelegate);
        }

        public void AddClientEndpoint(Type implementedContract, Binding binding, Uri address)
        {
            _endpoints.Add(new ClientEndpointConfiguration(implementedContract, binding, address));
        }

        private struct ClientEndpointConfiguration
        {
            public ClientEndpointConfiguration(Type contract, Binding binding, Uri address)
            {
                Contract = contract;
                Binding = binding;
                Address = address;
            }

            public Uri Address { get; set; }
            public Binding Binding { get; set; }
            public Type Contract { get; set; }
        }
    }
}
