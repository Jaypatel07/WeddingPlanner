using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Models;


namespace Wedding_Planner.Controllers {
    public class UserController : Controller {
        private Context dbContext;
        public UserController (Context context) {
            dbContext = context;
        }

        [HttpGet]
        [Route ("")]
        public ActionResult Index () {
            return View ("Index");
        }

        [HttpPost]
        [Route ("register")]
        public IActionResult register (User user) {
            if (!ModelState.IsValid) {
                return RedirectToAction ("Index");
            }
            if (ModelState.IsValid) {
                if (dbContext.Users.Any (u => u.Email == user.Email)) {
                    ModelState.AddModelError ("Email", "Email already in use!");
                    return RedirectToAction ("");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                user.Password = Hasher.HashPassword (user, user.Password);
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;

                dbContext.Users.Add (user);
                dbContext.SaveChanges ();
                HttpContext.Session.SetInt32 ("UserId", user.UserId);
                return RedirectToAction ("dashboard", "Wedding");
            }
            return RedirectToAction ("Index");
        }

        [HttpGet]
        [Route ("login")]

        public ActionResult login () {
            return View ("Login");
        }

        [HttpPost]
        [Route ("login")]
        public IActionResult Login (LoginUser userSubmission) {
            if (ModelState.IsValid) {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault (u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if (userInDb == null) {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError ("Email", "Invalid Email/Password");
                    return View ("Login");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser> ();

                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword (userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0) {
                    ModelState.AddModelError ("Password", "Invalid Email/Password");
                    return View ("Login");
                }
                HttpContext.Session.SetInt32 ("UserId", userInDb.UserId);

                return RedirectToAction ("dashboard", "Wedding");

            }
            return View ("Login");
        }

        [HttpGet]
        [Route ("/logout")]

        public IActionResult Logout () {
            HttpContext.Session.Clear ();
        
            
            return RedirectToAction ("login");

        }

    }
}