// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.ConfigurationManager.Client;
using CoreWCF.ConfigurationManager.Client.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using System.ServiceModel.Description;
using Xunit;
using Xunit.Abstractions;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests
{
    public class IntegrationOfDependencyInjectionTests : TestBase
    {
        private readonly ITestOutputHelper _output;

        public IntegrationOfDependencyInjectionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AddOneEndpointInContainer()
        {
            AddOneEndpointInContainerCore("http://localhost:6687/SomeService.svc", "SomeEndpoint"); 
        }

        private void AddOneEndpointInContainerCore(string expectedBaseUri, string endpointName)
        {
            IWebHost host = ServiceHelper.CreateWebHostBuilder<Startup>(_output ).Build();
            host.Start();
            var resolver = new DependencyResolverHelper(host);

            var configHolder = resolver.GetService<IConfigurationHolder>();
            Assert.NotNull(configHolder);

            var bindingFactory = resolver.GetService<IBindingFactory>();
            Assert.NotNull(bindingFactory);

            var endpointBuilder = resolver.GetService<IServiceEndpointBuilder>();
            Assert.NotNull(endpointBuilder);

            // Verify single instance of IConfigurationHolder
            var configHolder2 = resolver.GetService<IConfigurationHolder>();
            Assert.Same(configHolder, configHolder2);

            // Verify single instance of IBindingFactory
            var bindingFactory2 = resolver.GetService<IBindingFactory>();
            Assert.Same(bindingFactory, bindingFactory2);

            // Verify single instance of IServiceEndpointBuilder
            var endpointBuilder2 = resolver.GetService<IServiceEndpointBuilder>();
            Assert.Same(endpointBuilder, endpointBuilder2);

            System.ServiceModel.Description.ServiceEndpoint endpoint = endpointBuilder.ConstructServiceEndPoint(endpointName);
            Assert.NotNull(endpoint);

            Assert.Equal(endpoint.Address.Uri.ToString(), expectedBaseUri);

            host.Dispose();
        }
    }
}
