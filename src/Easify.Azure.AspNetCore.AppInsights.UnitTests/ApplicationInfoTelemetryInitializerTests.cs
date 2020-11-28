// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
