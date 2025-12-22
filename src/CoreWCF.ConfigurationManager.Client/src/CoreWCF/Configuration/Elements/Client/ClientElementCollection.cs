using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

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
                throw new ArgumentNullException(nameof(element));
            }

            return ((ClientElement)element);
        }
    }
}
