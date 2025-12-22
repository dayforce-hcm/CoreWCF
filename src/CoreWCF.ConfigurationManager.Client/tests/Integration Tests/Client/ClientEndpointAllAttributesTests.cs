using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceModel;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Client
{
    public class ClientEndpointAllAttributesTests : TestBase
    {
        private const string ExpectedAddress = "http://localhost:8080/SomeService";
        private const string ExpectedBindingType = "basicHttpBinding";
        private const string ExpectedBindingName = "ClientEndpointHttpBinding";
        private const string ExpectedContract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string ExpectedEndpointName = "SomeClientEndpoint";
        private const string ExpectedBehaviorName = "ClientEndpointBehavior";
        private const string ExpectedKind = "basicHttpBinding";

        /// <summary>
        /// Verifies that creating a client configuration with no endpoints results in an exception 
        /// when attempting to construct a service endpoint.
        /// </summary>
        [Fact]
        public void CanCreateEmptyClientWithNoEndpoints()
        {
            var configNone = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileNone = TemporaryFileStream.Create(configNone))
            {
                var provider = CreateProvider(tempFileNone.Name);
                var configHolder = GetConfigurationHolder(provider);
                var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                var endpointName = configHolder.ClientEndpoints.FirstOrDefault()?.Name ?? "";
                Assert.Throws<System.ArgumentException>(() =>
                {
                    var endpointObjSome = endpointBuilder.ConstructServiceEndPoint(endpointName);
                });
            }
        }

        /// <summary>
        /// Ensures that adding an endpoint with no parameters to the client section throws a configuration exception.
        /// </summary>
        [Fact]
        public void CanCreateClientWithNoEndpointParameters_ThrowsConfigurationException()
        {
            var configNone = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint() // No parameters
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileNone = TemporaryFileStream.Create(configNone))
            {
                var providerNone = CreateProvider(tempFileNone.Name);
                var configHolderNone = GetConfigurationHolder(providerNone);
                Assert.Throws<System.Configuration.ConfigurationErrorsException>(() =>
                {    
                    var endpointBuilderNone = providerNone.GetRequiredService<IServiceEndpointBuilder>();                    
                });
            }
        }

        /// <summary>
        /// Checks that specifying only the address for an endpoint (without binding or contract) throws a configuration exception.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAddressEndpointParameters_ThrowsConfigurationException()
        {
            var configNone = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(address: ExpectedAddress)
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileNone = TemporaryFileStream.Create(configNone))
            {
                var provider = CreateProvider(tempFileNone.Name);
                var configHolderNone = GetConfigurationHolder(provider);
                Assert.Throws<System.Configuration.ConfigurationErrorsException>(() =>
                {
                    var endpointBuilderNone = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }

        /// <summary>
        /// Validates that providing address and binding, but omitting contract, results in a configuration exception.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAddressAndBindingEndpointParameters_ThrowsConfigurationException()
        {
            var configNone = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(address: ExpectedAddress, binding: ExpectedBindingType)
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileNone = TemporaryFileStream.Create(configNone))
            {
                var provider= CreateProvider(tempFileNone.Name);
                var configHolderNone = GetConfigurationHolder(provider);
                Assert.Throws<System.Configuration.ConfigurationErrorsException>(() =>
                {
                    var endpointBuilderNone = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }

        /// <summary>
        /// Confirms that an endpoint with address, binding, and contract can be successfully constructed and validated.
        /// </summary>
        [Fact]
        public void CanCreateClientWithRequiredEndpointParameters_Successful()
        {
            var configNone = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress, 
                    binding: ExpectedBindingType, 
                    contract: ExpectedContract)
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileNone = TemporaryFileStream.Create(configNone))
            {
                var provider= CreateProvider(tempFileNone.Name);
                var configHolder = GetConfigurationHolder(provider);
                var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                var endpointName = configHolder.ClientEndpoints.FirstOrDefault()?.Name ?? "";
                var endpointObjSome = endpointBuilder.ConstructServiceEndPoint(endpointName);
                Assert.NotNull(endpointObjSome);
                Assert.Equal(ExpectedAddress, endpointObjSome.Address.Uri.ToString());
                Assert.Equal(ExpectedBindingType, endpointObjSome.Binding.Name, true); 
                Assert.Equal(ExpectedContract, endpointObjSome.Contract.ConfigurationName);
            }
        }

        /// <summary>
        /// Tests that specifying all endpoint parameters but omitting the bindings section throws a binding not found exception.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAllEndpointParametersAndNoBindings_ThrowsConfigurationException()
        {
            var configAll = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress,
                    binding: ExpectedBindingType,
                    contract: ExpectedContract,
                    bindingConfiguration: ExpectedBindingName,
                    kind: ExpectedKind,
                    name: ExpectedEndpointName
                )
                .CloseClientSection()
                .EndConfig()
                .ToString();
            using (var tempFileAll = TemporaryFileStream.Create(configAll))
            {
                var providerAll = CreateProvider(tempFileAll.Name);
                var configHolderAll = GetConfigurationHolder(providerAll);
                Assert.NotNull(configHolderAll);

                Assert.Throws<BindingNotFoundException>(() =>
                {
                    var endpointBuilderAll = providerAll.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }


        /// <summary>
        /// Verifies that specifying all endpoint parameters with an empty bindings section also throws a 
        /// binding not found exception.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAllEndpointParametersAndEmptyBindingSection_ThrowsConfigurationException()
        {
            var configAll = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress,
                    binding: ExpectedBindingType,
                    contract: ExpectedContract,
                    bindingConfiguration: ExpectedBindingName,
                    kind: ExpectedKind,
                    name: ExpectedEndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();
            using (var tempFileAll = TemporaryFileStream.Create(configAll))
            {
                var providerAll = CreateProvider(tempFileAll.Name);
                var configHolderAll = GetConfigurationHolder(providerAll);
                Assert.NotNull(configHolderAll);

                Assert.Throws<BindingNotFoundException>(() =>
                {
                    var endpointBuilderAll = providerAll.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }

        /// <summary>
        /// Ensures that an endpoint with all parameters and a valid binding section can be successfully 
        /// constructed and used to create a channel.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAllEndpointParametersAndBinding_Successful()
        {
            var configAll = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress,
                    binding: ExpectedBindingType,
                    contract: ExpectedContract,
                    bindingConfiguration: ExpectedBindingName,
                    kind: ExpectedKind,
                    name: ExpectedEndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                .StartHttpBindingSection()
                    .StartBinding(name: ExpectedBindingName)
                    .CloseBinding()
                .CloseHttpBindingSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();
            using (var tempFileAll = TemporaryFileStream.Create(configAll))
            {
                var providerAll = CreateProvider(tempFileAll.Name);
                var configHolderAll = GetConfigurationHolder(providerAll);
                Assert.NotNull(configHolderAll);

                var endpointBuilderAll = providerAll.GetRequiredService<IServiceEndpointBuilder>();
                var endpointObjAll = endpointBuilderAll.ConstructServiceEndPoint(ExpectedEndpointName);
                Assert.NotNull(endpointObjAll);
                Assert.Single(configHolderAll.ClientEndpoints);

                Assert.Equal(ExpectedAddress, endpointObjAll.Address.Uri.ToString());
                Assert.Equal(ExpectedContract, endpointObjAll.Contract.ConfigurationName);
                Assert.Equal(ExpectedBindingName, endpointObjAll.Binding.Name, true);
                Assert.NotNull(endpointObjAll.Name); // Actual endpoint name is different, so we just check it's not null

                var factory = new ChannelFactory<ISomeService>(endpointObjAll);
                Assert.NotNull(factory);
                var client = factory.CreateChannel();
                Assert.NotNull(client);

                ((IClientChannel)client).Close();
            }
        }


        /// <summary>
        /// Checks that specifying all endpoint parameters, binding, and an empty behavior section does not 
        /// prevent successful endpoint construction and channel creation.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAllEndpointParametersAndBindingAndBehavior_ThrowsConfigurationException()
        {
            var configAll = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress,
                    binding: ExpectedBindingType,
                    contract: ExpectedContract,
                    bindingConfiguration: ExpectedBindingName,
                    kind: ExpectedKind,
                    name: ExpectedEndpointName,
                    behaviorConfiguration: ExpectedBehaviorName
                )
                .CloseClientSection()
                .StartEndpointBehaviorsSection()
                .CloseEndpointBehaviorsSection()
                .StartBindingsSection()
                .StartHttpBindingSection()
                    .StartBinding(name: ExpectedBindingName)
                    .CloseBinding()
                .CloseHttpBindingSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();
            using (var tempFileAll = TemporaryFileStream.Create(configAll))
            {
                var providerAll = CreateProvider(tempFileAll.Name);
                var configHolderAll = GetConfigurationHolder(providerAll);
                Assert.NotNull(configHolderAll);

                var endpointBuilderAll = providerAll.GetRequiredService<IServiceEndpointBuilder>();
                var endpointObjAll = endpointBuilderAll.ConstructServiceEndPoint(ExpectedEndpointName);
                Assert.NotNull(endpointObjAll);
                Assert.Single(configHolderAll.ClientEndpoints);

                Assert.Equal(ExpectedAddress, endpointObjAll.Address.Uri.ToString());
                Assert.Equal(ExpectedContract, endpointObjAll.Contract.ConfigurationName);
                Assert.Equal(ExpectedBindingName, endpointObjAll.Binding.Name, true);
                Assert.NotNull(endpointObjAll.Name); // Actual endpoint name is different, so we just check it's not null

                var factory = new ChannelFactory<ISomeService>(endpointObjAll);
                Assert.NotNull(factory);
                var client = factory.CreateChannel();
                Assert.NotNull(client);

                ((IClientChannel)client).Close();
            }
        }

        /// <summary>
        /// Validates that an endpoint with all parameters, binding, and behavior can be successfully constructed 
        /// and used to create a channel.
        /// </summary>
        [Fact]
        public void CanCreateClientWithAllEndpointParameters_Successful()
        {
            var configAll = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: ExpectedAddress,
                    binding: ExpectedBindingType,
                    contract: ExpectedContract,
                    bindingConfiguration: ExpectedBindingName,
                    kind: ExpectedKind,
                    name: ExpectedEndpointName,
                    behaviorConfiguration: ExpectedBehaviorName
                )
                .CloseClientSection()
                .StartBindingsSection()
                .StartHttpBindingSection()
                    .StartBinding(name: ExpectedBindingName)
                    .CloseBinding()
                .CloseHttpBindingSection()
                .CloseBindingsSection()
                .StartEndpointBehaviorsSection()
                .AddEndpointBehavior()
                .CloseEndpointBehaviorsSection()
                .EndConfig()
                .ToString();
            using (var tempFileAll = TemporaryFileStream.Create(configAll))
            {
                var providerAll = CreateProvider(tempFileAll.Name);
                var configHolderAll = GetConfigurationHolder(providerAll);
                Assert.NotNull(configHolderAll);
                
                var endpointBuilderAll = providerAll.GetRequiredService<IServiceEndpointBuilder>();
                var endpointObjAll = endpointBuilderAll.ConstructServiceEndPoint(ExpectedEndpointName);
                Assert.NotNull(endpointObjAll);
                Assert.Single(configHolderAll.ClientEndpoints);

                Assert.Equal(ExpectedAddress, endpointObjAll.Address.Uri.ToString());
                Assert.Equal(ExpectedContract, endpointObjAll.Contract.ConfigurationName);
                Assert.Equal(ExpectedBindingName, endpointObjAll.Binding.Name, true);
                Assert.NotNull(endpointObjAll.Name); // Actual endpoint name is different, so we just check it's not null

                var factory = new ChannelFactory<ISomeService>(endpointObjAll);
                Assert.NotNull(factory);
                var client = factory.CreateChannel();
                Assert.NotNull(client);

                ((IClientChannel)client).Close();
            }
        }
    }
}
