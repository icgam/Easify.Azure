using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Easify.Azure.AspNetCore.KeyVaults
{
    public static class KeyVaultExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder builder, Action<KeyVaultOptions> configure = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var config = builder.Build();
            var options = new KeyVaultOptions();
            config.GetSection(nameof(KeyVaultOptions)).Bind(options);
            
            configure?.Invoke(options);

            if (!options.KeyVaultNames.Any())
                return builder;
            
            using var store = new X509Store(options.LocalCertificateStore);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(
                X509FindType.FindByThumbprint,
                config[options.AzureAdApplicationCertThumbprint], false);

            foreach (var keyVaultName in options.KeyVaultNames)
            {
                builder.AddAzureKeyVault(new Uri($"https://{keyVaultName}.vault.azure.net/"),
                    new ClientCertificateCredential(options.AzureAdTenantId, options.AzureAdApplicationId, certs.OfType<X509Certificate2>().Single()),
                    new KeyVaultSecretManager());
            }
            store.Close();

            return builder;
        }

        public static IConfigurationBuilder AddAzureKeyVault(this ConfigurationBuilder builder, Func<bool> condition,
            Action<KeyVaultOptions> configure = null)
        {
            return condition() ? builder.AddAzureKeyVault(configure) : builder;
        }
    }
}