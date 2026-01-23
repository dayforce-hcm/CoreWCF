// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.ConfigurationManager.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            string pathToXml = GetXmlConfigFilePath();
            services.AddOptions();
            services.AddClientModelServices();
            services.AddClientModelConfigurationManagerFile(pathToXml);
        }

        public void Configure(IApplicationBuilder app)
        {
        }

        private string GetXmlConfigFilePath()
        {
            string xml = $@"
<configuration> 
    <system.serviceModel>
        <bindings>            
            <basicHttpBinding>
                <binding name=""testHttpBinding""/>
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint   address=""http://localhost:6687/SomeService.svc""
                        name=""SomeEndpoint""
                        binding=""basicHttpBinding""
                        bindingConfiguration=""testHttpBinding""
                        contract=""{typeof(ISomeService).FullName}"" />
        </client>
    </system.serviceModel>
</configuration>";
            
            var fs = TemporaryFileStream.Create(xml);

            return fs.Name;
        }
    }
}
