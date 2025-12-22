using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IO;

namespace CoreWCF.ConfigurationManager.Client
{
    public static class ConfigurationManagerExtensions
    {
        public static IServiceCollection AddClientModelConfigurationManagerFile(this IServiceCollection services, string path)
        {
            if (!File.Exists(path)) { throw new FileNotFoundException(SR.Format(SR.FileNotFound, path)); }
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (!fs.CanRead) { throw new IOException(SR.Format(SR.CannotAccessFile, path)); }
            }

            services.TryAddSingleton<IConfigurationHolder, ConfigurationHolder>();
            services.TryAddSingleton<IBindingFactory, BindingFactory>();
            services.TryAddSingleton<IServiceEndpointBuilder, ServiceEndpointBuilder>();
            services.AddSingleton<IConfigureOptions<ClientModelOptions>>(ctx => new ConfigurationManagerServiceModelOptions(ctx, path));

            return services;
        }

        public static IServiceCollection AddClientModelServices( this IServiceCollection services)
        {
            services.AddOptions();
            //services.AddSingleton<ClientEndpointBuilder>();
            //services.AddSingleton<IClientEndpointBuilder>(provider => provider.GetRequiredService<ClientEndpointBuilder>());
            //services.AddSingleton(typeof(ClientModelOptions));
            return services;
        }
    }
}
