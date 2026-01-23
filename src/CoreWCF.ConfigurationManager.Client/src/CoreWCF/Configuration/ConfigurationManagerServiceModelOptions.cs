// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreWCF.ConfigurationManager.Client
{
    internal class ConfigurationManagerServiceModelOptions : IConfigureNamedOptions<ClientModelOptions>
    {
        private readonly Lazy<ServiceModelSectionGroup> _section;

        private readonly IConfigurationHolder _holder;

        public ConfigurationManagerServiceModelOptions(IServiceProvider builder, string path)
        {
            _holder = builder.GetRequiredService<IConfigurationHolder>();

            _section = new Lazy<ServiceModelSectionGroup>(() =>
            {
                var assembly = Assembly.GetEntryAssembly();
                var basePath = string.IsNullOrEmpty(assembly?.Location) ? AppContext.BaseDirectory : Path.GetDirectoryName(assembly.Location);

                // hack - in .net core 2.1 on linux directory not correct(on unit tests),
                // for example:
                // /home/vsts/.nuget/packages/microsoft.testplatform.testhost/16.7.1/lib/netcoreapp2.1/
                var isNetCore21 = RuntimeInformation.FrameworkDescription.Contains(".NET Core 4.6");
                if (isNetCore21 && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    basePath = AppContext.BaseDirectory;
                }

                var configMap = new ExeConfigurationFileMap(Path.Combine(basePath, "CoreWCF.ConfigurationManager.Client.config"))
                {
                    ExeConfigFilename = path
                };
                System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                var section = ServiceModelSectionGroup.GetSectionGroup(configuration);

                if (section is null)
                {
                    throw new ServiceModelConfigurationException("Section not found");
                }

                return section;
            }, true);
        }

        public void Configure(string name, ClientModelOptions options)
        {
            Configure(options);
        }

        public void Configure(ClientModelOptions options)
        {
            var configHolder = ParseConfig();
            foreach (var clientEndPoint in configHolder.ClientEndpoints)
            {
                IXmlConfigClientEndpoint configEndpoint = configHolder.GetXmlConfigClientEndpoint(clientEndPoint);
                options.ConfigureClientEndpoint(configEndpoint.Contract, endpointConfig =>
                {
                    endpointConfig.AddClientEndpoint(configEndpoint.Contract, configEndpoint.Binding, configEndpoint.Address);
                });
            }
        }

        private void ReadConfigSection(ServiceModelSectionGroup group)
        {
            if (group is null)
            {
                return;
            }

            AddBinding(group.Bindings?.BasicHttpBinding.Bindings);
            //AddBinding(group.Bindings?.NetTcpBinding.Bindings);
            //AddBinding(group.Bindings?.NetHttpBinding.Bindings);
            //AddBinding(group.Bindings?.wsHttpBinding.Bindings);
            AddBinding(group.Bindings?.WebHttpBinding.Bindings);
            AddBinding(group.Bindings?.CustomBinding.Bindings);
            AddEndpoint(group.Client?.ClientEndpoints);
        }

        private void AddEndpoint(IEnumerable endpoints)
        {
            foreach (ClientEndpointElement bindingElement in endpoints.OfType<ClientEndpointElement>())
            {
                _holder.AddClientEndpoint(
                    bindingElement.Name,
                    bindingElement.Address,
                    bindingElement.Contract,
                    bindingElement.Binding,
                    bindingElement.BindingConfiguration);
            }
        }

        private void AddBinding(IEnumerable bindings)
        {
            foreach (IStandardBindingElement bindingElement in bindings.OfType<IStandardBindingElement>())
            {
                Binding binding = bindingElement.CreateBinding();
                _holder.AddBinding(binding);
            }
        }

        private IConfigurationHolder ParseConfig()
        {
            ReadConfigSection(_section.Value);
            return _holder;
        }
    }

}
