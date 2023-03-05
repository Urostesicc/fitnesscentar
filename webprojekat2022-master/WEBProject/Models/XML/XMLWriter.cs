using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WEBProject.Models.XML
{
    public static class XMLWriter
    {
        public static void WriteUsers(List<User> users)
        {
            var serializer = new XmlSerializer(typeof(List<User>));
            using(var writer = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/users.xml")))
            {
                serializer.Serialize(writer, users);
            }
        }

        public static void WriteFitnessCenters(List<FitnessCenter> fitnessCenters)
        {
            var serializer = new XmlSerializer(typeof(List<FitnessCenter>));
            using (var writer = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/fitness_centers.xml")))
            {
                serializer.Serialize(writer, fitnessCenters);
            }
        }

        public static void WriteGroupTrainings(List<GroupTraining> groupTrainings)
        {
            var serializer = new XmlSerializer(typeof(List<GroupTraining>));
            using (var writer = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/group_trainings.xml")))
            {
                serializer.Serialize(writer, groupTrainings);
            }
        }

        public static void WriteComments(List<Comment> comments)
        {
            var serializer = new XmlSerializer(typeof(List<Comment>));
            using (var writer = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/comments.xml")))
            {
                serializer.Serialize(writer, comments);
            }
        }

       public static void AddData()
       {

            FitnessCenter fc1 = new FitnessCenter { Id="fc1", Address ="Random Address 123 Novi Sad", Name = "Best fitness center", FoundedIn = 2012,  OwnerId = "owner", GroupSessionFee = 50, SingleSessionFee = 10, SingleSessionTrainerFee = 15, YearlyFee = 200, MonthlyFee = 60, IsDeleted = false};
            FitnessCenter fc2 = new FitnessCenter { Id = "fc2", Address = "Bulevar Oslobodjenja 222 Novi Sad", Name = "Not the best fitness center", FoundedIn = 2018, OwnerId = "owner1", GroupSessionFee = 50, SingleSessionFee = 10, SingleSessionTrainerFee = 15, YearlyFee = 200, MonthlyFee = 60, IsDeleted = false };
            Owner owner = new Owner { Username = "owner", Name = "Uros", Lastname = "Tesic", Password = "owner123", UserType = "Owner", Gender = "Male", IsDeleted = false, CanLogin = true };
            Owner owner1 = new Owner { Username = "owner1", Name = "Uros", Lastname = "Tesic", Password = "owner123", UserType = "Owner", Gender = "Male", IsDeleted = false, CanLogin = true };

            Trainer trainer = new Trainer { Username = "trainer1-fc1", Name = "Uros", Lastname = "Tesic", Password = "trainer123", UserType = "Trainer", Gender = "Male", IsDeleted = false, CanLogin = true, FitnessCenterId = "fc1", TrainingIds = new List<string> { "gt1-fc1", "gt2-fc1" } };
            Trainer trainer1 = new Trainer { Username = "trainer2-fc1", Name = "Uros", Lastname = "Tesic", Password = "trainer123", UserType = "Trainer", Gender = "Male", IsDeleted = false, CanLogin = true, FitnessCenterId = "fc1", TrainingIds = new List<string> { "gt3-fc1", "gt4-fc1" } };
            Trainer trainer2 = new Trainer { Username = "trainer1-fc2", Name = "Uros", Lastname = "Tesic", Password = "trainer123", UserType = "Trainer", Gender = "Male", IsDeleted = false, CanLogin = true, FitnessCenterId = "fc2", TrainingIds = new List<string> {"gt2-fc1", "gt2-fc2" } };
            Trainer trainer3 = new Trainer { Username = "trainer2-fc2", Name = "Uros", Lastname = "Tesic", Password = "trainer123", UserType = "Trainer", Gender = "Male", IsDeleted = false, CanLogin = true, FitnessCenterId = "fc2", TrainingIds = new List<string> {"gt2-fc3", "gt2-fc4" } };

            string date = "2022-05-05 13:30";
            string date1 = "2022-07-09 13:30";


            Visitor visitor = new Visitor { Username = "visitor", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt1-fc1", "gt2-fc1", "gt2-fc4" } };
            Visitor visitor1 = new Visitor { Username = "visitor1", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt1-fc1", "gt2-fc1", "gt2-fc1", "gt2-fc1" } };
            Visitor visitor2 = new Visitor { Username = "visitor2", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt2-fc3", "gt3-fc1" } };
            Visitor visitor3 = new Visitor { Username = "visitor3", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt3-fc1", "gt4-fc1" } };
            Visitor visitor4 = new Visitor { Username = "visitor4", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt4-fc1", "gt2-fc3" } };
            Visitor visitor5 = new Visitor { Username = "visitor5", Name = "Uros", Lastname = "Tesic", Password = "visitor123", UserType = "Visitor", Gender = "Male", IsDeleted = false, CanLogin = true, TrainingIds = new List<string> { "gt2-fc1", "gt2-fc2", "gt2-fc4" } };





            GroupTraining gc = new GroupTraining { Id = "gt1-fc1", FitnessCenterId = "fc1", MaxVisitors = 3, TrainerUsername = "trainer1-fc1", IsDeleted = false, Name = "training1", TrainingType = "Cardio", TrainingDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor, visitor1 } };
            GroupTraining gc1 = new GroupTraining { Id = "gt2-fc1", FitnessCenterId = "fc1", MaxVisitors = 2, TrainerUsername = "trainer1-fc1", IsDeleted = false, Name = "training2", TrainingType = "Weights", TrainingDate = DateTime.ParseExact(date1, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor, visitor1 } };
            GroupTraining gc2 = new GroupTraining { Id = "gt3-fc1", FitnessCenterId = "fc1", MaxVisitors = 5, TrainerUsername = "trainer2-fc1", IsDeleted = false, Name = "training3", TrainingType = "Cardio", TrainingDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor2, visitor3 } };
            GroupTraining gc3 = new GroupTraining { Id = "gt4-fc1", FitnessCenterId = "fc1", MaxVisitors = 4, TrainerUsername = "trainer2-fc1", IsDeleted = false, Name = "training4", TrainingType = "Weights", TrainingDate = DateTime.ParseExact(date1, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor3, visitor4 } };


            GroupTraining gc4 = new GroupTraining { Id = "gt2-fc1", FitnessCenterId = "fc2", MaxVisitors = 12, TrainerUsername = "trainer1-fc2", IsDeleted = false, Name = "training5", TrainingType = "Cardio", TrainingDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor1, visitor5 } };
            GroupTraining gc5 = new GroupTraining { Id = "gt2-fc2", FitnessCenterId = "fc2", MaxVisitors = 3, TrainerUsername = "trainer1-fc2", IsDeleted = false, Name = "training6", TrainingType = "Weights", TrainingDate = DateTime.ParseExact(date1, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor1, visitor5 } };
            GroupTraining gc6 = new GroupTraining { Id = "gt2-fc3", FitnessCenterId = "fc2", MaxVisitors = 6, TrainerUsername = "trainer2-fc2", IsDeleted = false, Name = "training7", TrainingType = "Cardio", TrainingDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor2, visitor4 } };
            GroupTraining gc7 = new GroupTraining { Id = "gt2-fc4", FitnessCenterId = "fc2", MaxVisitors = 2, TrainerUsername = "trainer2-fc2", IsDeleted = false, Name = "training8", TrainingType = "Weights", TrainingDate = DateTime.ParseExact(date1, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), TrainingLength = 60, Visitors = new List<Visitor> { visitor, visitor5 } };







            List<FitnessCenter> fitnessCenters = new List<FitnessCenter> { fc1, fc2};




            List<User> users = new List<User> { trainer, trainer1, trainer2, trainer3, owner, owner1, visitor, visitor1, visitor2, visitor3, visitor4, visitor5 };


         
            List<GroupTraining> groupTrainings = new List<GroupTraining> { gc, gc1, gc2, gc3, gc4, gc5, gc6, gc7 };


            WriteUsers(users);
            WriteGroupTrainings(groupTrainings);
            WriteFitnessCenters(fitnessCenters);
            

            Comment comment1 = new Comment { Id = "com1", CommentText = "very good", Grade = 5, IsDeleted = false, VisitorId = "visitor", FitnessCenterId = "fc1", IsApproved = false};
            Comment comment2 = new Comment { Id = "com2", CommentText = "badd", Grade = 1, IsDeleted = false, VisitorId = "visitor1", FitnessCenterId = "fc1", IsApproved = false};
            Comment comment3 = new Comment { Id = "com3", CommentText = "eh", Grade = 3, IsDeleted = false, VisitorId = "visitor1", FitnessCenterId = "fc1", IsApproved = false};
            Comment comment4 = new Comment { Id = "com4", CommentText = "decent", Grade = 4, IsDeleted = false, VisitorId = "visitor", FitnessCenterId = "fc1", IsApproved = true};


            WriteComments(new List<Comment> { comment1, comment2, comment3, comment4 });

        }
    }
}