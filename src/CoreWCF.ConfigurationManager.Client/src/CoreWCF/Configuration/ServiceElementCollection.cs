using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    [ConfigurationCollection(typeof(ClientElement), AddItemName = ConfigurationStrings.Endpoint)]
    public class ClientElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClientElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(element));
            }

            return ((ClientElement)element);
        }
    }
}
