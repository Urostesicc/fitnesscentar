using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{
    public class Visitor : User
    {
        public List<string> TrainingIds { get; set; } = new List<string>();
        
        public Visitor()
        {

        }
    }
}