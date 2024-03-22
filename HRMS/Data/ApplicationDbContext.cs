using HRMS.DTO;
using HRMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HRMS.Models.Employees;
using Microsoft.Extensions.Hosting;

namespace HRMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<SickLeaveReplacedEmployee> SickLeaveReplacedEmployees { get; set; }
        public DbSet<SkillType> Skills { get; set; }
        public DbSet<EmpSkill> EmpSkills { get; set; }
        public DbSet<ProjSkill> ProjSkills { get; set; }
        public DbSet<TaskSingle> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAttachement> CommentAttachements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Project>().HasMany(e => e.AssignedEmployees).WithMany(e => e.AssignedProjects);
            modelBuilder.Entity<EmployeeRegisterDTO>().HasNoKey();
            modelBuilder.Entity<Employee>().HasMany(x => x.LeaveDetails).WithOne(x => x.EmployeeApplied);
            modelBuilder.Entity<SickLeaveReplacedEmployee>().HasOne(x => x.EmployeeReplaced).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SickLeaveReplacedEmployee>()
                    .HasOne(m => m.EmployeeOnleave)
                    .WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskSingle>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TaskSingle>().HasIndex(e => e.CreatedUserId).IsUnique(false);     //Not including CreatedBy in Tasks
            modelBuilder.Entity<Comment>().HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasIndex(e => e.UserId).IsUnique(false);
            modelBuilder.Entity<Department>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<Project>().HasQueryFilter(p => p.IsActive);

        }
        //public DbSet<EmployeeRegisterDTO> EmployeeRegisterDTO { get; set; } = default!;
    }
}
