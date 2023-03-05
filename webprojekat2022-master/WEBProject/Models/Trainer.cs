using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{
    public class Trainer : User
    {
        public string FitnessCenterId { get; set; }
        public List<string> TrainingIds { get; set; } = new List<string>();

        public Trainer()
        {

        }
    }
}