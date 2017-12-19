using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; //for self join

namespace BeltExam2.Models
{
    public class Invitation
    {
        public int InvitationID {get; set;}
        public int UserID {get; set;}

        public User User {get; set;}

        public int InviterID {get; set;}

        public User Inviter {get;set;}
    }
}