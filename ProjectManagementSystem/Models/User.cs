using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace ProjectsManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Employee;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public List<Milestone> Milestones { get; set; }
        public List<Project> WorksProjects { get; set; }
        public List<Project> ManagedProjects { get; set; }

    }
}
