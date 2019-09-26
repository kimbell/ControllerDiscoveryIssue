using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Services.Common.Caching.Test
{
	[Route("test")]
    public class TestController : Controller
    {
	    private readonly IMemoryCache _cache;

	    public TestController(IMemoryCache cache)
	    {
		    _cache = cache;
	    }

		[HttpGet]
		[Route("addcache/{key}/{value}")]
	    public IActionResult AddKey(string key, string value)
        {
            var x = _cache.Set(key, value);
            return Ok(x);
		}

	    [HttpGet]
	    [Route("cacheitem/{key}")]
	    public IActionResult GetValue(string key)
        {
            var x = _cache.Get<string>(key);
            return Ok(x);
	    }
	}
}
