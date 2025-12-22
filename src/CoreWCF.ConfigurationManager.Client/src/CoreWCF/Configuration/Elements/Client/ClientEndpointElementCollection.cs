// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    [ConfigurationCollection(typeof(ClientEndpointElement), AddItemName = ConfigurationStrings.Endpoint)]
    public class ClientEndpointElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClientEndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return ((ClientEndpointElement)element).Name;
        }
    }
}
