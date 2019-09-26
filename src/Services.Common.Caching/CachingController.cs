using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Services.Common.Caching
{
	/// <summary>
	/// Provides access to the memory cache
	/// </summary>
	[Route("cache")]
    [PublicAPI]
    [ApiController]
	public class CachingController : Controller
	{
		private readonly ILogger<CachingController> _logging;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logging"></param>
		public CachingController(ILogger<CachingController> logging)
		{
			_logging = logging;
        }

	    /// <summary>
		/// Deletes all items from the cache
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult Flush()
		{
			_logging.LogWarning("Removing all entries from the memory cache");
            return NoContent();
		}
    }
}
