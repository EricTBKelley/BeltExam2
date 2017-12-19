using Microsoft.EntityFrameworkCore;

namespace BeltExam2.Models
{
    public class BeltExam2Context : DbContext
    {        // base() calls the parent class' constructor passing the "options" parameter along        
        public BeltExam2Context(DbContextOptions<BeltExam2Context> options) : base(options) { }
        public DbSet<User> Users {get;set;} 
        public DbSet<Invitation> Invitations {get;set;}
        public DbSet<NetworkRelationship> NetworkRelationships {get;set;}
    }
}