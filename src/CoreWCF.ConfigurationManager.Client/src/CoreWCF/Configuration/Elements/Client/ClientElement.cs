using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ClientElement : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationStrings.DefaultCollectionName, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ClientEndpointElementCollection Endpoints
        {
            get { return (ClientEndpointElementCollection)base[ConfigurationStrings.DefaultCollectionName]; }
        }
    }
}
