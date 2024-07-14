using Microsoft.EntityFrameworkCore;
using StocksApplication.Models;

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
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

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

			app.Run();
		}
	}
}
