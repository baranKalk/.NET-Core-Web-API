using Authentication.Core.DTOs;
using Authentication.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using SharedLibrary.Exceptions;

namespace Authentication.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : CustomBaseController
	{
		private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
             _userService = userService;
        }


        [HttpPost]
		public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
		{
			throw new CustomException("Veri Tabanı ile ilgili bir hata meydana geldi");
			return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
		}


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUser()
		{
			return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
		}

		[HttpGet("CreateUserRoles/{userName}")]
		public async  Task<IActionResult> CreateUserRoles(string userName)
		{
			return ActionResultInstance(await _userService.CreateUserRoles(userName));
		}

	}
}
