using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configurations;
using SharedLibrary.Services;

namespace SharedLibrary.Extensions
{
	public static class CustomTokenAuth
	{
		public static void AddCustomTokenAuth(this IServiceCollection services,CustomTokenOptions tokenOption)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
			{
				
				opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					//geçerlilik adımlarından bir tanesi
					ValidIssuer = tokenOption.Issuer,
					ValidAudience = tokenOption.Audience[0],

					//imza verme işlemi
					IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),


					//DOĞRULAMA İŞLEMİ
					ValidateIssuerSigningKey = true,
					ValidateAudience = true,
					ValidateIssuer = true,

					//ömrünü kontrol etme
					ValidateLifetime = true,

					//default olarak 5 dakikdalık ömür ekler
					ClockSkew = TimeSpan.Zero
				};
			});
		}
	}
}
