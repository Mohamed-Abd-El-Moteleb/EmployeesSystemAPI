
using EmployeesSystem.Models;
using Microsoft.EntityFrameworkCore;
using EmployeesSystem.Utility;
using Microsoft.AspNetCore.Identity;

namespace EmployeesSystem
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
            });

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	        .AddEntityFrameworkStores<ApplicationDbContext>()
	        .AddDefaultTokenProviders();

			builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(opttions =>
            {
                opttions.AddPolicy("MyPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });

            });

			var app = builder.Build();


			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				await Seeder.SeedAdminAsync(services);

			}


			if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
