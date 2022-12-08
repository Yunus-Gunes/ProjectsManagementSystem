using DevExpress.XtraReports.Templates;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectsManagementSystem.Models;
using System.Collections.Generic;

namespace ProjectManagementSystem.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        /*
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        */
        public DbSet<Project> Projects { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Money> Moneys { get; set; }
        public DbSet<ProjectDocument> ProjectDocuments { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=ProjectManagement;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SEED veritabana iki admin eklendi
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id= 1,
                    UserRole = UserRole.Admin,
                    FirstName = "Ergün Deniz",
                    LastName = "Buyruk",
                    Email = "ergunndenizbuyruk@gmail.com",
                    Password = "123qwe",
                    Address = "İzmir",
                    PhoneNumber = "5522341427",
                    FullName = "Ergün Deniz Buyruk"
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    UserRole = UserRole.Admin,
                    FirstName = "Yunus",
                    LastName = "Güneş",
                    FullName = "Yunus_Güneş",
                    Email = "ygunes6606@gmail.com",
                    Password = "123qwe",
                    Address = "İzmir",
                    PhoneNumber = "1234567891"
                    
                    
                }
            );


            modelBuilder.Entity<Project>().HasOne(project => project.Manager)
                           .WithMany(user => user.ManagedProjects).HasForeignKey(p => p.ManagerId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>().HasMany(p => p.Users).WithMany(u => u.WorksProjects).UsingEntity(j => j.ToTable("Project_User"));
        }
    }
}
