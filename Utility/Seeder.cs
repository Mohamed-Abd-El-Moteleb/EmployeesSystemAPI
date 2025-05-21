using EmployeesSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeesSystem.Utility
{
	public static class Seeder
	{
		public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			string adminEmail = "Mohamed@gmail.com";
			string adminUserName = "Mohamed";
			string adminPassword = "Mohamed@123";
			string adminRole = "Admin";

			// Create role if it doesn't exist
			if (!await roleManager.RoleExistsAsync(adminRole))
			{
				await roleManager.CreateAsync(new IdentityRole(adminRole));
			}

			var adminUser = await userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				var newAdmin = new ApplicationUser
				{
					UserName = adminUserName,
					Email = adminEmail,
					EmailConfirmed = true
				};

				var result = await userManager.CreateAsync(newAdmin, adminPassword);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(newAdmin, adminRole);
				}
				else
				{
					throw new Exception("Failed to create admin user: " +
						string.Join(", ", result.Errors.Select(e => e.Description)));
				}

			}
			else
			{
				// Ensure user is in Admin role
				if (!await userManager.IsInRoleAsync(adminUser, adminRole))
				{
					await userManager.AddToRoleAsync(adminUser, adminRole);
				}
			}
		}
	}
}
