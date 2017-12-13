using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingContext _context;
    
        public HomeController(WeddingContext context)
        {
            _context = context;
        }
    
        [HttpGet]
        [Route("")]
        public IActionResult LoginPage()
        {
            // List<User> Allusers = _context.users.ToList();
            // foreach(var user in Allusers)
            // {
            //     System.Console.WriteLine(user.FirstName);
            // }
            // Other code
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserViewModel UserInput)
        {
            if(ModelState.IsValid)
            {
                List<User> CheckEmail = _context.users.Where(user => user.Email==UserInput.Email).ToList();
                if(CheckEmail.Count==0)
                {
                    PasswordHasher<UserViewModel> Hasher = new PasswordHasher<UserViewModel>();
                    UserInput.Password = Hasher.HashPassword(UserInput, UserInput.Password);
                    User NewUser = new User
                    {
                        FirstName = UserInput.FirstName,
                        LastName = UserInput.LastName,
                        Email = UserInput.Email,
                        Password = UserInput.Password
                    };
                    _context.Add(NewUser);
                    _context.SaveChanges();
                    User SaveUser = _context.users.SingleOrDefault(user => user.Email == NewUser.Email);
                    HttpContext.Session.SetInt32("CurId", NewUser.UserId);
                    HttpContext.Session.SetString("CurUser",NewUser.FirstName);
                    HttpContext.Session.SetString("CurEmail",NewUser.Email);
                    return RedirectToAction("Dashboard");
                }
                ViewBag.error="Email already exisits in database";
                return View("LoginPage");   
            }
            return View("LoginPage", UserInput);
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("CurUser")==null)
            {
                return RedirectToAction("LoginPage");
            }
            List<Wedding> AvailableWeddings = _context.weddings.Include(wed => wed.GuestList).ThenInclude(list=>list.User).ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("CurId");
            return View(AvailableWeddings);
        }
        
        
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string LogEmail, string LogPass)
        {
            List<User> CheckUser = _context.users.Where(user => user.Email == LogEmail).ToList();
            if(CheckUser.Count==0){
                ViewBag.error="Email does not exist in database";
                return View("LoginPage");
            }
            var Hasher = new PasswordHasher<User>();
            if(Hasher.VerifyHashedPassword(CheckUser[0], CheckUser[0].Password, LogPass)==0){
                ViewBag.error="password is incorrect";
                return View("LoginPage");
            }
            HttpContext.Session.SetInt32("CurId", CheckUser[0].UserId);
            HttpContext.Session.SetString("CurUser",CheckUser[0].FirstName);
            HttpContext.Session.SetString("CurEmail",CheckUser[0].Email);
            return RedirectToAction("Dashboard");
        }


        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

        [Route("AddWedding")]
        public IActionResult AddWedding()
        {
            if(HttpContext.Session.GetString("CurUser")==null)
            {
                return RedirectToAction("LoginPage");
            }
            return View();
        }

        [HttpPost]
        [Route("CreateWedding")]
        public IActionResult CreateWedding(WeddingViewModel WeddingInfo)
        {
            if(ModelState.IsValid)
            {   
                Wedding NewWedding = new Wedding
                {
                    NameOne = WeddingInfo.NameOne,
                    NameTwo = WeddingInfo.NameTwo,
                    Date = WeddingInfo.Date,
                    Address = WeddingInfo.Address,
                    CreatorId = (int)HttpContext.Session.GetInt32("CurId"),
                };
                _context.Add(NewWedding);
                
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("AddWedding", WeddingInfo);
        }

        [Route("ShowWedding/{Id}")]
        public IActionResult ShowWedding(int Id)
        {
            Wedding MyWedding = _context.weddings.Include(wed=>wed.GuestList).ThenInclude(list=>list.User).SingleOrDefault(wed => wed.WeddingId==Id);
            System.Console.WriteLine(Id);
            return View(MyWedding);
        }
        

        [Route("NewRSVP/{Id}")]
        public IActionResult NewRSVP(int Id)
        {
            RSVPList MyRSVP = new RSVPList
            {
                RSVPStatus=true,
                UserId=(int)HttpContext.Session.GetInt32("CurId"),
                WeddingId=Id,
            }; 
            _context.rsvplist.Add(MyRSVP);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [Route("UNRSVP/{Id}")]
        public IActionResult UNRSVP(int Id)
        {
            RSVPList MyRSVP = _context.rsvplist.SingleOrDefault(rsvp=>rsvp.RSVPListId==Id);
            _context.rsvplist.Remove(MyRSVP);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [Route("DeleteWedding/{Id}")]
        public IActionResult DeleteWedding(int Id)
        {
            Wedding MyWedding = _context.weddings.SingleOrDefault(wedding=>wedding.WeddingId==Id);
            _context.weddings.Remove(MyWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
