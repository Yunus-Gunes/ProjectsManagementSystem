
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsManagementSystem.Models
{
    public class Milestone
    {
        public int Id { get; set; }
        public string Name { get; set; }//
        public List<Task> Tasks { get; set; }
        public ProjectAndTaskState MilestoneState { get; set; } = ProjectAndTaskState.NotStarted;//

        [ForeignKey("AssignedUserId")]
        public User AssignedTo { get; set; }//
        public int AssignedUserId { get; set; }//
        public DateTime? StartTime { get; set; }//
        public DateTime? FinishTime { get; set; }//

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }//
        public int ProjectId { get; set; }//

    }
}
