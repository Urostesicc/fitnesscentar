using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBProject.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string VisitorId { get; set; }
        public string FitnessCenterId { get; set; }
        public string CommentText { get; set; }
        public int Grade { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Comment()
        {

        }
    }
}