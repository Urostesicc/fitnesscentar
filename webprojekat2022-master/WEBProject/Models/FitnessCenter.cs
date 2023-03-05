using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{ 
    public class FitnessCenter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int FoundedIn { get; set; }
        public string OwnerId { get; set; }
        public int MonthlyFee { get; set; }
        public int YearlyFee { get; set; }
        public int SingleSessionFee { get; set; }
        public int GroupSessionFee { get; set; }
        public int SingleSessionTrainerFee { get; set; }
        public bool IsDeleted { get; set; } = false;

        public FitnessCenter()
        {

        }
    }
}