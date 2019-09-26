using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services.Common.Caching.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Services.Common.Caching.Test
{
	public class CacheTests
    {
        private readonly ITestOutputHelper _output;

        public CacheTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ThisCallsControllerInTestProjectThatWorks()
        {
            using (var tc = new TestContext(_output))
            {
                tc.AdditionalServices = services => { services.AddMemoryCache(); };

                var r = await tc.HttpClient.GetAsync("http://localhost/test/addcache/bob/ted");
                Assert.Equal(HttpStatusCode.OK, r.StatusCode);
                
                r = await tc.HttpClient.GetAsync("http://localhost/test/cacheitem/bob");
                Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            }
        }

        [Fact]
        public async Task ThisCallsControllerInMainProjectThatDoesntWork()
        {
            using (var tc = new TestContext(_output))
            {
                var r = await tc.HttpClient.DeleteAsync("http://localhost/cache");
                Assert.Equal(HttpStatusCode.NoContent, r.StatusCode);
            }
        }

        [Fact]
        public async Task ThisCallsControllerInMainProjectUsingAddPartsThatWorks()
        {
            using (var tc = new TestContext(_output))
            {
                tc.AdditionalServices = services =>
                {
#if NETCOREAPP3_0
                    services.AddControllers().AddApplicationPart(typeof(CachingController).Assembly);
#endif
                };

                var r = await tc.HttpClient.DeleteAsync("http://localhost/cache");
                Assert.Equal(HttpStatusCode.NoContent, r.StatusCode);
            }
        }
    }
}

