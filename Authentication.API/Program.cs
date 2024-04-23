using Authentication.Core.Model;
using Authentication.Core.Repositories;
using Authentication.Core.Services;
using Authentication.Core.UnitOfWork;
using Authentication.Data;
using Authentication.Data.Repositories;
using Authentication.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;
using SharedLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
	{
		sqlOptions.MigrationsAssembly("Authentication.Data");
	});
 
});
builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
{
	Opt.User.RequireUniqueEmail = true;
	Opt.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Configuration.GetSection("TokenOption");
builder.Configuration.GetSection("Clients");

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
	var tokenOption = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
	opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
	{
		//geçerlilik adýmlarýndan bir tanesi
		ValidIssuer = tokenOption.Issuer,
		ValidAudience = tokenOption.Audience[0],

		//imza verme iþlemi
		IssuerSigningKey=SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),


		//DOÐRULAMA ÝÞLEMÝ
		ValidateIssuerSigningKey=true,
		ValidateAudience=true,
		ValidateIssuer=true,

		//ömrünü kontrol etme
		ValidateLifetime=true,

		//default olarak 5 dakikdalýk ömür ekler
		ClockSkew=TimeSpan.Zero
	};
});
builder.Services.AddControllers().AddFluentValidation(optiponts =>
{
	optiponts.RegisterValidatorsFromAssemblyContaining<Program>();
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication.API v1"));

}

else
{
	app.UseCustomException();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
