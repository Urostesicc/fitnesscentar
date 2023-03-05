using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WEBProject.Models
{
    [XmlInclude(typeof(Trainer))]
    [XmlInclude(typeof(Visitor))]
    [XmlInclude(typeof(Owner))]
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool CanLogin { get; set; } = true;
        public string UserType { get; set; }
    }
}