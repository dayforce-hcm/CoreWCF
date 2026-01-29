// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Security;
using CoreWCF.Configuration;
using SMBasicHttpMessageSecurity = System.ServiceModel.BasicHttpMessageSecurity;
using SMBasicHttpMessageCredentialType = System.ServiceModel.BasicHttpMessageCredentialType;

namespace CoreWCF.ConfigurationManager.Client
{
    public class BasicHttpMessageSecurityElement : ServiceModelConfigurationElement
    {
        internal const SMBasicHttpMessageCredentialType DefaultClientCredentialType = SMBasicHttpMessageCredentialType.UserName;

        [ConfigurationProperty(ConfigurationStrings.ClientCredentialType, DefaultValue = DefaultClientCredentialType)]
        public SMBasicHttpMessageCredentialType ClientCredentialType
        {
            get { return (SMBasicHttpMessageCredentialType)base[ConfigurationStrings.ClientCredentialType]; }
            set { base[ConfigurationStrings.ClientCredentialType] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.AlgorithmSuite, DefaultValue = ConfigurationStrings.Default)]
        [TypeConverter(typeof(SecurityAlgorithmSuiteConverter))]
        public SecurityAlgorithmSuite AlgorithmSuite
        {
            get { return (SecurityAlgorithmSuite)base[ConfigurationStrings.AlgorithmSuite]; }
            set { base[ConfigurationStrings.AlgorithmSuite] = value; }
        }

        internal void ApplyConfiguration(SMBasicHttpMessageSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(security));
            }

            security.ClientCredentialType = ClientCredentialType;

            if (PropertyValueOrigin.Default != ElementInformation.Properties[ConfigurationStrings.AlgorithmSuite].ValueOrigin)
            {
                security.AlgorithmSuite = AlgorithmSuite;
            }
        }
    }
}
