﻿using Authentication.Core.DTOs;
using Authentication.Core.Model;
using Authentication.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : CustomBaseController
	{
		private readonly IServiceGeneric<Product, ProductDto> _productService;


		public ProductController(IServiceGeneric<Product, ProductDto> productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			return ActionResultInstance(await _productService.GetAllAsync());
		}
		
		[HttpPost]
		public async Task<IActionResult> SaveProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.AddAsync(productDto));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.Update(productDto, productDto.Id));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			return ActionResultInstance(await _productService.Remove(id));
		}

	}
}
