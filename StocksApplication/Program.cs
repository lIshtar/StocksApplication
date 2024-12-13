using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StocksApplication.Data;
using StocksApplication.Interfaces;
using StocksApplication.Models;
using StocksApplication.Repository;
using StocksApplication.Service;

namespace StocksApplication
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddSwaggerGen();

			builder.Services
				.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
				.AddTransient<IStockRepository, StockRepository>()
				.AddTransient<ICommentRepository, CommentRepository>()
				.AddScoped<ITokenService, TokenService>()
				.AddTransient<IPortfolioRepository, PortfolioRepository>();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
				{
					options.Password.RequireDigit = true;
					options.Password.RequireNonAlphanumeric = true;
					options.Password.RequireLowercase = true;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme =
				options.DefaultChallengeScheme =
				options.DefaultForbidScheme =
				options.DefaultScheme =
				options.DefaultSignInScheme =
				options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
                };
			});

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
            });

			var a = new AppUser();
            var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}



            app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllers();

            app.Run();
		}
	}
}
