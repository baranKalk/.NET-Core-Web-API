using Authentication.Core.Configuration;
using Authentication.Core.DTOs;
using Authentication.Core.Model;
using Authentication.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Service.Services
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<UserApp> _userManager;
		private readonly CustomTokenOptions _tokenOptions;
		public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions>options)
		{
			_userManager = userManager;
			_tokenOptions = options.Value;
		}
		private string CreateRefreshToken()
		{
			var numberByte = new Byte[32];
			using var rnd = RandomNumberGenerator.Create();
			rnd.GetBytes(numberByte);
			return Convert.ToBase64String(numberByte);
		}
		// üyelik sistemi payloadı
		private async Task<IEnumerable<Claim>>GetClaims(UserApp userApp,List<string>audience)
		{
			var userRoles = await _userManager.GetRolesAsync(userApp);


			var userList = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier,userApp.Id),
				new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
				new Claim(ClaimTypes.Name,userApp.UserName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new Claim("city",userApp.City)
			};
			userList.AddRange(audience.Select(x =>new Claim(JwtRegisteredClaimNames.Aud,x)));
			userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
			return userList;
		}
		//üyelik sistemi gerektirmeyen Token
		private IEnumerable<Claim>GetClaimsByClient(Client client)
		{
			var claims = new List<Claim>();

			claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

			return claims;
		}


		public TokenDto CreateToken(UserApp userApp)
		{
			//tokenin ömrü 
			var accessTokenExpiration=DateTime.Now.AddMinutes(_tokenOptions.AccesTokenExpiration);
			//-------------
			var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
			//imza anahtarı
			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);
			//imzalama kısmı
			SigningCredentials signingCredentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
			
			JwtSecurityToken jwtSecuritToken = new JwtSecurityToken(
				issuer: _tokenOptions.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaims(userApp,_tokenOptions.Audience).Result,
				signingCredentials:signingCredentials);

			var handler = new JwtSecurityTokenHandler();
			//token burada oluşturuluyor
			var token =handler.WriteToken(jwtSecuritToken);
			
			var tokenDto = new TokenDto
			{
				AccessToken = token,
				RefreshToken = CreateRefreshToken(),
				AccessTokenExpiration = accessTokenExpiration,
				RefreshTokenExpiration = refreshTokenExpiration
			};
			return tokenDto;
		}
		public ClientTokenDto CreateTokenByClient(Client client)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccesTokenExpiration);
			//imza anahtarı
			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);
			//imzalama kısmı
			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecuritToken = new JwtSecurityToken(
				issuer: _tokenOptions.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaimsByClient(client),
				signingCredentials: signingCredentials);

			var handler = new JwtSecurityTokenHandler();
			//token burada oluşturuluyor
			var token = handler.WriteToken(jwtSecuritToken);

			var tokenDto = new ClientTokenDto
			{
				AccessToken = token,
				AccessTokenExpiration = accessTokenExpiration,
			};
			return tokenDto;
		}
	}
}
