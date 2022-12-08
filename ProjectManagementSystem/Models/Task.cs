using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsManagementSystem.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Subject { get; set; }//
        public ProjectAndTaskState Status { get; set; } = ProjectAndTaskState.NotStarted;
        public DateTime? StartTime { get; set; }//
        public DateTime? FinishTime { get; set; }//

        public int MilestoneId { get; set; }//

        [ForeignKey("MilestoneId")]
        public Milestone Milestone { get; set; }//
    }
}
