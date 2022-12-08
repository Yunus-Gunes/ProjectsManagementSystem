using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsManagementSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Manager { get; set; }
        public int ManagerId { get; set; }
        public List<User> Users{ get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public DateTime? EstimatedStartTime { get; set; }
        public DateTime? EstimatedFinishTime { get; set; }
        public ProjectAndTaskState ProjectState { get; set; }
        public List<ProjectDocument> ProjectDocuments { get; set; }

        [ForeignKey("MoneyId")]
        public Money Money { get; set; }
        public int MoneyId { get; set; }
        public ProjectType ProjectType { get; set; }
        public List<Milestone> Milestones { get; set; }
        public string GetProjectNo()
        {
            return $"PRJ-{Id}";
        }
    }
}
