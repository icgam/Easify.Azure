using System.Security.Cryptography.X509Certificates;

namespace LittleBlocks.Azure.AspNetCore.KeyVaults
{
    public class KeyVaultOptions
    {
        public string AzureAdTenantId { get; set; }
        public string AzureAdApplicationId { get; set; }
        public string AzureAdApplicationCertThumbprint { get; set; }
        public StoreLocation LocalCertificateStore { get; set; } = StoreLocation.CurrentUser;
        public string[] KeyVaultNames { get; set; } = { };
    }
}