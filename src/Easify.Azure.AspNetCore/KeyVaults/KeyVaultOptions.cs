namespace Easify.Azure.AspNetCore.KeyVaults
{
    public class KeyVaultOptions
    {
        public string AzureAdTenantId { get; set; }
        public string AzureAdApplicationId { get; set; }
        public string AzureAdApplicationCertThumbprint { get; set; }
        public string[] KeyVaultNames { get; set; } = { };
    }
}