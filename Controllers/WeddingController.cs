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
    public class WeddingController : Controller {
        private Context dbContext;
        public WeddingController(Context context) {
            dbContext = context;
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            List<Wedding> AllWeddings = dbContext.Weddings
                                        .Include(wedding => wedding.Guests)
                                            .ThenInclude(guest => guest.User)
                                        .ToList();
            List<Rsvp> UserRsvps = dbContext.Rsvps.Where(rsvp => rsvp.User.Equals(CurrentUser)).ToList();
            
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.AllWeddings = AllWeddings;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.UserRsvps = UserRsvps;
            return View();
        }

        [HttpGet]
        [Route("NewWedding")]
        public IActionResult NewWedding() {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [HttpPost]
        [Route("AddWedding")]
        public IActionResult AddWedding(Wedding model) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            if(model.WeddingDate < DateTime.Now) {
                ModelState.AddModelError("WeddingDate", "Wedding must be in the future");
            }
            if(ModelState.IsValid) {
                dbContext.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else {
                ViewBag.errors = ModelState.Values;
                return View("NewWedding");
            }
        }

        [HttpGet]
        [Route("Wedding/{WeddingId}")]
        public IActionResult Wedding(int WeddingId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            Wedding CurrentWedding = dbContext.Weddings
                                        .Include(wedding => wedding.Guests)
                                            .ThenInclude(guest => guest.User)
                                        .SingleOrDefault(wedding => wedding.WeddingId == WeddingId);
            ViewBag.CurrentWedding = CurrentWedding;
            return View("Wedding");
        }

        [HttpGet]
        [Route("RSVP/{WeddingId}")]
        public IActionResult RSVP(int WeddingId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            Wedding CurrentWedding = dbContext.Weddings
                                            .Include(wedding => wedding.Guests)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(wedding => wedding.WeddingId == WeddingId);
            Rsvp NewRsvp = new Rsvp {
                UserId = CurrentUser.UserId,
                User = CurrentUser,
                WeddingId = CurrentWedding.WeddingId,
                Wedding = CurrentWedding
            };
            CurrentWedding.Guests.Add(NewRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Decline/{WeddingId}")]
        public IActionResult Decline(int WeddingId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            Rsvp CurrentRsvp = dbContext.Rsvps.SingleOrDefault(rsvp => rsvp.UserId == HttpContext.Session.GetInt32("UserId") && rsvp.WeddingId == WeddingId);
            Wedding CurrentWedding = dbContext.Weddings
                                            .Include(wedding => wedding.Guests)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(wedding => wedding.WeddingId == WeddingId);
            CurrentWedding.Guests.Remove(CurrentRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Delete/{WeddingId}")]
        public IActionResult Delete(int WeddingId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            Wedding CurrentWedding = dbContext.Weddings
                                            .SingleOrDefault(wedding => wedding.WeddingId == WeddingId);
            dbContext.Weddings.Remove(CurrentWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}