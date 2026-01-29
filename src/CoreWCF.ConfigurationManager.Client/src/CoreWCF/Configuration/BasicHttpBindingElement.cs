// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SMTransferMode = System.ServiceModel.TransferMode;
using SMWSMessageEncoding = System.ServiceModel.WSMessageEncoding;

namespace CoreWCF.ConfigurationManager.Client
{
    public class BasicHttpBindingElement : HttpBindingBaseElement
    {
        public BasicHttpBindingElement(string name)
            : base(name)
        {
        }

        public BasicHttpBindingElement()
            : this(null)
        {
        }

        [ConfigurationProperty(ConfigurationStrings.Security)]
        public BasicHttpSecurityElement Security
        {
            get { return (BasicHttpSecurityElement)base[ConfigurationStrings.Security]; }
        }

        public override Binding CreateBinding()
        {
            var binding = new System.ServiceModel.BasicHttpBinding(Security.Mode)
            {
                Name = Name,
                MaxReceivedMessageSize = MaxReceivedMessageSize,
                MaxBufferSize = MaxBufferSize,
                ReceiveTimeout = ReceiveTimeout,
                CloseTimeout = CloseTimeout,
                OpenTimeout = OpenTimeout,
                SendTimeout = SendTimeout,
                TransferMode = (SMTransferMode)TransferMode,
                TextEncoding = TextEncoding,
                ReaderQuotas = ReaderQuotas.Clone(),

                //BypassProxyOnLocal = BypassProxyOnLocal,              // Not supported in CoreWCF
                //HostNameComparisonMode = HostNameComparisonMode,      // Not supported in netstandard2.0
                MaxBufferPoolSize = MaxBufferPoolSize,
                ProxyAddress = ProxyAddress,
                UseDefaultWebProxy = UseDefaultWebProxy
            };
            if (!string.IsNullOrEmpty(MessageEncoding))
            {
                binding.MessageEncoding = (SMWSMessageEncoding)System.Enum.Parse(typeof(SMWSMessageEncoding), MessageEncoding);
            }
            Security.ApplyConfiguration(binding.Security);
            return binding;
        }
    }
}
