using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Services.Common.Caching.Test.Infrastructure
{
	/// <summary>
	/// Context used for unit testing
	/// </summary>
	public class TestContext : WebApplicationFactory<TestContext.Startup>
    {
        private readonly TestOutputLoggerProvider _loggerProvider;

        // actual setup is handled in ConfigureWebHost
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }
            public void Configure(IApplicationBuilder app)
            {
            }
        }

        /// <summary>
        /// test specific configuration for services
        /// </summary>
        public Action<IServiceCollection> AdditionalServices { get; set; }

        private HttpClient _httpClient;

        public HttpClient HttpClient
        {
            get
            {
                if (_httpClient != null)
                {
                    return _httpClient;
                }

                _httpClient = CreateClient();
                return _httpClient;
            }
        }

        public TestContext(ITestOutputHelper output)
        {
            _loggerProvider = new TestOutputLoggerProvider(output);
        }

        protected override void ConfigureWebHost(IWebHostBuilder webhostBuilder)
        {
            webhostBuilder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseDefaultServiceProvider(p => p.ValidateScopes = true)
                .ConfigureServices((context, services) =>
                {
#if NETCOREAPP3_0
                    services.AddControllers();
#else
                    services.AddMvc();
#endif
                    AdditionalServices?.Invoke(services);
                })
                .ConfigureLogging(l =>
                {
                    l.SetMinimumLevel(LogLevel.Debug);
                    l.AddFilter("Microsoft", LogLevel.Warning);
                    l.AddProvider(_loggerProvider);
                })
                .Configure(appBuilder =>
                {
#if NETCOREAPP3_0
                    appBuilder.UseRouting();
                    appBuilder.UseEndpoints(config =>
                    {
                        config.MapControllers();
                    });
#else
                    appBuilder.UseMvc();
#endif
                });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder<Startup>(null);
        }
    }
}
