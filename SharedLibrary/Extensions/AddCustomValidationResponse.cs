using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
	public static class AddCustomValidationResponse
	{
	   public static void UseCustomValidationResponse(this IServiceCollection service)
		{
			service.Configure<ApiBehaviorOptions>(opt =>
			{
				opt.InvalidModelStateResponseFactory = context =>
				{
					var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
					var errorDto = new ErrorDto(errors.ToList(), true);
					var response = Response<NoContentResult>.Fail(errorDto, 400);

                    return new BadRequestObjectResult(response);
				};
			});
		}
	}
}
