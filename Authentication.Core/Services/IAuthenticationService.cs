using Authentication.Core.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Services
{
	public interface IAuthenticationService
	{
		Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
		Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
		Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
		Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto); 

	}
}
