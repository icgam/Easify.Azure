using Easify.Azure.AspNetCore.KeyVaults;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Easify.Azure.AspNetCore.UnitTests.KeyVault
{
    public class KeyVaultExtensionsTests
    {
        [Fact]
        public void Should_AddAzureKeyVault_SkipAddingKeyVaultConfiguration_WhenNoNameHasBennProvidedForKeyVaults()
        {
            // Arrange
            var builder = new ConfigurationBuilder();

            //Act
            builder.AddAzureKeyVault(m =>
            {
                m.KeyVaultNames = new string[]{};
                m.AzureAdApplicationId = "AppID";
                m.AzureAdApplicationCertThumbprint = "CertificateThumbprint";
                m.AzureAdTenantId = "TenantID";
            });

            //Assert
            builder.Sources.Count.Should().Be(0);
        }
        
        [Fact]
        public void Should_AddAzureKeyVault_SkipAddingKeyVaultConfiguration_WhenTheConditionalRegistrationIsFalse()
        {
            // Arrange
            var builder = new ConfigurationBuilder();


            //Act
            builder.AddAzureKeyVault(() => false, m =>
            {
                m.KeyVaultNames = new string[]{};
                m.AzureAdApplicationId = "AppID";
                m.AzureAdApplicationCertThumbprint = "CertificateThumbprint";
                m.AzureAdTenantId = "TenantID";
            });

            //Assert
            builder.Sources.Count.Should().Be(0);
        }
    }
}