// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using CoreWCF.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    internal class BindingFactory : IBindingFactory
    {
        public Binding Create(string bindingType)
        {
            if (string.IsNullOrEmpty(bindingType))
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(bindingType));
            }

            // with reflection?
            switch (bindingType)
            {
                case "basicHttpBinding":
                    return new System.ServiceModel.BasicHttpBinding();
                case "netTcpBinding":
                    return new System.ServiceModel.NetTcpBinding();
                case "wsHttpBinding":
                    return new System.ServiceModel.WSHttpBinding();
                case "netHttpBinding":
                    return new System.ServiceModel.NetHttpBinding();
                case "customBinding":
                    return new CustomBinding();
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperWarning(new BindingNotFoundException());
            }
        }
    }
}
