using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; //for self join

namespace BeltExam2.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Description {get; set; }

        [InverseProperty("User")]
        public List<Invitation> Invitations { get; set; }

        [InverseProperty("User")]
        public List<NetworkRelationship> NetworkRelationships { get; set; }
        public User()
        {
            Invitations = new List<Invitation>();
            NetworkRelationships = new List<NetworkRelationship>();
        }
    }
}