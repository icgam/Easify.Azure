namespace Easify.Azure.Serilog
{
    public sealed class AzureLogAnalyticsOptions
    {
        public string WorkspaceId { get; set; }
        public string AuthenticationId { get; set; }
        public string LogName { get; set; }
        public int? LogBufferSize { get; set; }
        public int? BatchSize { get; set; }
    }
}