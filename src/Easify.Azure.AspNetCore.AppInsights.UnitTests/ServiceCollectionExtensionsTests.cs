using FluentAssertions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Easify.Azure.AspNetCore.AppInsights.UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Should_ApplicationInsight_IsResolvableFromContainer_WhenTheExtensionIsUsed()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTelemetry();

            //Act
            var sp = services.BuildServiceProvider();

            //Assert
            sp.GetRequiredService<ITelemetryModule>().Should().NotBeNull();
            sp.GetRequiredService<ITelemetryInitializer>().Should().NotBeNull().And
                .BeAssignableTo(typeof(ApplicationInfoTelemetryInitializer));
        }
    }
}