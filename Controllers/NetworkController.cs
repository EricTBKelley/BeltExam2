using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Http;

namespace BeltExam2.Controllers
{
    public class NetworkController : Controller
    {
        private BeltExam2Context _context;
        public NetworkController(BeltExam2Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Network/Dashboard")]

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LoggedUser = _context.Users.Single(user => user.UserID == HttpContext.Session.GetInt32("UserID"));
            ViewBag.NetworkBuddies = NetworkBuddies();
            ViewBag.Invitations = UsersInvitedMe();
            return View();
        }

//_____________show all other users_________________________________________________________________________________________________________________

        public List<User> UsersIInvited()
        {
            List<Invitation> MyInvitations = _context.Invitations.Include(i => i.User).Where(i => i.InviterID == HttpContext.Session.GetInt32("UserID")).ToList();
            List<User> UsersIInvited = new List<User>();
            foreach(Invitation Invitation in MyInvitations)
            {
                UsersIInvited.Add(Invitation.User);
            }
            return UsersIInvited;
        }

        public List<User> UsersInvitedMe()
        {
            List<Invitation> InvitationForMe = _context.Invitations.Include(i => i.Inviter).Where(i => i.UserID == HttpContext.Session.GetInt32("UserID")).ToList();
            List<User> UsersInvitedMe = new List<User>();
            foreach(Invitation Invitation in InvitationForMe)
            {
                UsersInvitedMe.Add(Invitation.Inviter);
            }
            return UsersInvitedMe;
        }

        public List<User> NetworkBuddies()
        {
            List<NetworkRelationship> NetworkRelationships = _context.NetworkRelationships.Include(nw => nw.NetworkUser).Where(nw => nw.UserID == HttpContext.Session.GetInt32("UserID")).ToList();
            List<User> NetworkBuddies = new List<User>();
            foreach(NetworkRelationship relationship in NetworkRelationships)
            {
                NetworkBuddies.Add(relationship.NetworkUser);
            }
            return NetworkBuddies;
        }
        public List<User> OtherUsers()
        {
            return _context.Users.Where(user => user.UserID != (int)HttpContext.Session.GetInt32("UserID")).Except(UsersIInvited()).Except(UsersInvitedMe()).Except(NetworkBuddies()).ToList();
        }

        [HttpGet]
        [Route("Network/Users")]
        public IActionResult Users()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.OtherUsers = OtherUsers();
            return View();
        }
        //_____________show a user_______________________________________________________________________________________________________________________________________________________________________________________
        [HttpGet]
        [Route("Network/Show/{UserID}")]

        public IActionResult ShowUser(int UserID)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowUser = _context.Users.Single(user => user.UserID == UserID);
            return View("Show");
        }

        //_____________invite a user_______________________________________________________________________________________________________________________________________________________________________________________

        [HttpPost]
        [Route("Network/InviteUser/{UserID}")]

        public IActionResult InviteUser(int UserID)
        {
            Invitation NewInvitation = new Invitation();
            NewInvitation.InviterID = (int)HttpContext.Session.GetInt32("UserID");
            NewInvitation.UserID = UserID;
            _context.Invitations.Add(NewInvitation);
            _context.SaveChanges();
            return RedirectToAction("Users", "Network");

        }

        //____________Accept invite___________________________________________________________________________________________________

        [HttpPost]
        [Route("Network/AcceptInvite/{UserID}")]

        public IActionResult AcceptInvite(int UserID)
        {
            Invitation AcceptedInvite = _context.Invitations.Where(i => i.UserID == HttpContext.Session.GetInt32("UserID")).Single(i=> i.InviterID == UserID);
            NetworkRelationship NewNW = new NetworkRelationship();
            NewNW.UserID = AcceptedInvite.UserID;
            NewNW.NetworkUserID = AcceptedInvite.InviterID;
            NetworkRelationship OtherNewNW = new NetworkRelationship();
            OtherNewNW.UserID = AcceptedInvite.InviterID;
            OtherNewNW.NetworkUserID = AcceptedInvite.UserID;

            _context.NetworkRelationships.Add(NewNW);
            _context.NetworkRelationships.Add(OtherNewNW);
            _context.Invitations.Remove(AcceptedInvite);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Network");
        }

        [HttpPost]
        [Route("NetWork/IgnoreInvite/{UserID}")]

        public IActionResult IgnoreInvite(int UserID)
        {
            Invitation IgnoredInvite = _context.Invitations.Where(i => i.UserID == HttpContext.Session.GetInt32("UserID")).Single(i => i.InviterID == UserID);
            _context.Invitations.Remove(IgnoredInvite);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Network");
        }
    }
}