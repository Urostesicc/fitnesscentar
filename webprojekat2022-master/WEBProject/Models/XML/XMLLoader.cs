using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WEBProject.Models.XML
{
    public static class XMLLoader
    {
        
        public static List<Comment> GetComments()
        {
            List<Comment> comments = new List<Comment>();
            var serializer = new XmlSerializer(typeof(List<Comment>));
            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/comments.xml")))
            {
                comments = (List<Comment>)serializer.Deserialize(reader);
            }
       
            return comments;

        }

        public static List<FitnessCenter> GetFitnessCenters()
        {
            List<FitnessCenter> fitnessCenters = new List<FitnessCenter>();
            var serializer = new XmlSerializer(typeof(List<FitnessCenter>));
            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/fitness_centers.xml")))
            {
                fitnessCenters = (List<FitnessCenter>)serializer.Deserialize(reader);
            }
           
            return fitnessCenters;
        }

        public static List<GroupTraining> GetGroupTrainings()
        {
            List<GroupTraining> groupTrainings = new List<GroupTraining>();
            var serializer = new XmlSerializer(typeof(List<GroupTraining>));
            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/group_trainings.xml")))
            {
                groupTrainings = (List<GroupTraining>)serializer.Deserialize(reader);
            }

           
            return groupTrainings;
        }

        public static List<User> GetUsers()
        {
            var serializer = new XmlSerializer(typeof(List<User>));
            List<User> users = new List<User>();
            using(var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/users.xml"))){
                users = (List<User>)serializer.Deserialize(reader);
            }
            return users;

        }
    }
}
