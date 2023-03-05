using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{
    public class GroupTraining
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TrainingType { get; set; }
        public string FitnessCenterId { get; set; }
        public int TrainingLength { get; set; }
        public DateTime TrainingDate { get; set; }
        public int MaxVisitors { get; set; }
        public List<Visitor> Visitors { get; set; } = new List<Visitor>();
        public bool IsDeleted { get; set; } = false;
        public string TrainerUsername { get; set; }

        public GroupTraining()
        {
            
        }
    }
}