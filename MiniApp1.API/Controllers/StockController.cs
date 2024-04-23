﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
	[Authorize(Roles ="admin.manager")]
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetStock()
		{
			var userName = HttpContext.User.Identity.Name;

			var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

			return Ok($"UserName:{userName}-UserId:{userIdClaim.Value}");
		}
	}
}
