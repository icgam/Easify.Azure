using System;
using Easify.Configurations;
using FluentAssertions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Environment = Easify.Configurations.Environment;

namespace Easify.Azure.AspNetCore.AppInsights.UnitTests
{
    public class ApplicationInfoTelemetryInitializerTests
    {
        [Fact]
        public void Should_Initialize_ConfigureTheTelemetryCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTelemetry();

            services.AddOptions<Application>().Configure(a =>
            {
                a.Environment = new Environment {Name = "Development"};
                a.Name = "Easify Azure";
            });

            var sp = services.BuildServiceProvider();

            var telemetry = new MockTelemetry();
            var sut = sp.GetRequiredService<ITelemetryInitializer>();
            
            //Act
            sut.Initialize(telemetry);

            //Assert
            telemetry.Context.Cloud.RoleName.Should().Be("Easify Azure-Development");
            telemetry.Context.Cloud.RoleInstance.Should().Be("Easify Azure-Development-Instance");
        }

        public class MockTelemetry : ITelemetry
        {
            public void Sanitize()
            {
                throw new NotImplementedException();
            }

            public ITelemetry DeepClone()
            {
                throw new NotImplementedException();
            }

            public void SerializeData(ISerializationWriter serializationWriter)
            {
                throw new NotImplementedException();
            }

            public DateTimeOffset Timestamp { get; set; }
            public TelemetryContext Context { get; } = new TelemetryContext();
            public IExtension Extension { get; set; }
            public string Sequence { get; set; }
        }
    }
}