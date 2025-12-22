// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    public class ClientEndpointElement : ConfigurationElement
    {
        [ConfigurationProperty(ConfigurationStrings.Address, DefaultValue = "", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        public string Address
        {
            get { return (string)base[ConfigurationStrings.Address]; }
            set { base[ConfigurationStrings.Address] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.BehaviorConfiguration, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string BehaviorConfiguration
        {
            get { return (string)base[ConfigurationStrings.BehaviorConfiguration]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.BehaviorConfiguration] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.Binding, Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        [StringValidator(MinLength = 0)]
        public string Binding
        {
            get { return (string)base[ConfigurationStrings.Binding]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.Binding] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.BindingConfiguration, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string BindingConfiguration
        {
            get { return (string)base[ConfigurationStrings.BindingConfiguration]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.BindingConfiguration] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.Contract, DefaultValue = "", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        [StringValidator(MinLength = 0)]
        public string Contract
        {
            get { return (string)base[ConfigurationStrings.Contract]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.Contract] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.EndpointConfiguration, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string EndpointConfiguration
        {
            get { return (string)base[ConfigurationStrings.EndpointConfiguration]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.EndpointConfiguration] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.Kind, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string Kind
        {
            get { return (string)base[ConfigurationStrings.Kind]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.Kind] = value;
            }
        }


        [ConfigurationProperty(ConfigurationStrings.Name, DefaultValue = "", Options = ConfigurationPropertyOptions.IsKey)]
        [StringValidator(MinLength = 0)]
        public string Name
        {
            get { return (string)base[ConfigurationStrings.Name]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }
                base[ConfigurationStrings.Name] = value;
            }
        }

        internal ClientEndpoint CreateClientEndpoint()
        {          
            var endpoint = new ClientEndpoint()
            {
                Name = Name,
                Address = Address,
                Binding = Binding,
                BindingConfiguration = BindingConfiguration,
                Contract = Contract
            };

            return endpoint;
        }
    }
}
