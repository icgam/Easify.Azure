// This software is part of the LittleBlocks framework
// Copyright (C) 2019 LittleBlocks
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
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.DependencyInjection;

namespace LittleBlocks.Azure.AspNetCore.AppInsights
{
    public sealed class TelemetryConfigure : ITelemetryConfigure
    {
        private readonly IServiceCollection _services;

        public TelemetryConfigure(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }
        
        public ITelemetryConfigure DisableSqlInstrumentation()
        {
            _services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation = false;
            });

            return this;
        }
    }
}