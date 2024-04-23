using Microsoft.AspNetCore.Mvc;

namespace MiniApp3.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductValController : ControllerBase
	{
		public IActionResult GetProductVal()
		{
			var productName = HttpContext.User.Identity.Name;

			return Ok($"Invoice işlemleri = ProductName:{productName}");
		}
	}
}
