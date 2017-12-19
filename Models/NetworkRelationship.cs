using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; //for self join

namespace BeltExam2.Models
{
    public class NetworkRelationship
    {
        public int NetworkRelationshipID {get;set;}
        public int UserID {get;set;}

        public User User {get;set;}

        public int NetworkUserID {get;set;}

        public User NetworkUser {get;set;}
    }
}