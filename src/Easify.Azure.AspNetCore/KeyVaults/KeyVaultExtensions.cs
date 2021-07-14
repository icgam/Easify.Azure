using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Easify.Azure.AspNetCore.KeyVaults
{
    public static class KeyVaultExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var config = builder.Build();
            var options = new KeyVaultOptions();
            config.GetSection(nameof(KeyVaultOptions)).Bind(options);

            if (!options.KeyVaultNames.Any())
                return builder;
            
            using var store = new X509Store(StoreLocation.LocalMachine);
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

        public static IConfigurationBuilder AddAzureKeyVault(this ConfigurationBuilder builder, Func<bool> condition)
        {
            return condition() ? builder.AddAzureKeyVault() : builder;
        }
    }
}