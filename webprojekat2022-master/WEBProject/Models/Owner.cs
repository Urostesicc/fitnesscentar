using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{
    public class Owner : User
    {
        public List<string> OwnedCentersIds { get; set; }
    }
}