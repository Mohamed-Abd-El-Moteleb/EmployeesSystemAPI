using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeesSystem.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Position> Positions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Department>()
				.HasOne(d => d.Manager)
				.WithMany()
				.HasForeignKey(d => d.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Employee>()
				.HasOne(e => e.Department)
				.WithMany(d => d.Employees)
				.HasForeignKey(e => e.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Employee>()
				.HasOne(e => e.Position)
				.WithMany(p => p.Employees)
				.HasForeignKey(e => e.PositionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Salary)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Department>().HasData(
				new Department
				{
					Id = 1,
					Name = "Not Selected Yet",
					ManagerId = null
				});

			modelBuilder.Entity<Position>().HasData(
				new Position { Id = 1, Name = "Unassigned" },
				new Position { Id = 2, Name = "Manager" }
			);

		}
	}
}
