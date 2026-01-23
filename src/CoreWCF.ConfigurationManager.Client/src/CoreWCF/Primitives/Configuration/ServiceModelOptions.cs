using System;
using System.Collections.Generic;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ClientModelOptions
    {
        private readonly Dictionary<Type, ClientEndpointConfigurationBuilder> _configBuilders = new Dictionary<Type, ClientEndpointConfigurationBuilder>();
        
        public void ConfigureClientEndpoint(Type endpointType, Action<ClientEndpointConfigurationBuilder> configure)
        {
            if (!_configBuilders.TryGetValue(endpointType, out ClientEndpointConfigurationBuilder configBuilder))
            {
                configBuilder = new ClientEndpointConfigurationBuilder(endpointType);
                _configBuilders[endpointType] = configBuilder;
            }
            configBuilder.Configure(configure);
        }        
    }
}
