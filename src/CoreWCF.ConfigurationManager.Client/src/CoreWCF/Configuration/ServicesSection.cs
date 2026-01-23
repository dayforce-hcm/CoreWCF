using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    public  class ClientSection : ConfigurationSection
    {
        [ConfigurationProperty(ConfigurationStrings.DefaultCollectionName, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ClientEndpointElementCollection ClientEndpoints
        {
            get { return (ClientEndpointElementCollection)this[ConfigurationStrings.DefaultCollectionName]; }
        }
    }
    
}
