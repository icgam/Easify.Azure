using System;
using System.Diagnostics.CodeAnalysis;
using Easify.Logging.SeriLog;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Easify.Azure.Serilog
{
    [ExcludeFromCodeCoverage]
    public static class SinkBuilderContext
    {
        public static ISinkBuilderContext UseAzureTableStorage(this ISinkBuilderContext sinkBuilderContext, IConfiguration configuration, Action<AzureTableStorageOptions> configure = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var options = new AzureTableStorageOptions();
            configuration.GetSection(nameof(AzureTableStorageOptions)).Bind(options);
            configure?.Invoke(options);

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new ArgumentException($"{nameof(options.ConnectionString)} is missing in AzureTableStorageOptions");
            
            var loggerConfiguration = sinkBuilderContext.LoggerConfiguration.WriteTo.AzureTableStorage(options.ConnectionString);

            return sinkBuilderContext.Clone(loggerConfiguration);
        }
        
        public static ISinkBuilderContext UseAzureLogAnalytics(this ISinkBuilderContext sinkBuilderContext, IConfiguration configuration, Action<AzureLogAnalyticsOptions> configure = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var options = new AzureLogAnalyticsOptions();
            configuration.GetSection(nameof(AzureLogAnalyticsOptions)).Bind(options);
            configure?.Invoke(options);

            if (string.IsNullOrWhiteSpace(options.WorkspaceId))
                throw new ArgumentException($"{nameof(options.WorkspaceId)} is missing in AzureLogAnalyticsOptions");
            
            if (string.IsNullOrWhiteSpace(options.AuthenticationId))
                throw new ArgumentException($"{nameof(options.AuthenticationId)} is missing in AzureLogAnalyticsOptions");
            
            var loggerConfiguration = sinkBuilderContext
                .LoggerConfiguration
                .WriteTo
                .AzureAnalytics(options.WorkspaceId,options.AuthenticationId);

            return sinkBuilderContext.Clone(loggerConfiguration);
        }
    }
}