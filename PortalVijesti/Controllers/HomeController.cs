using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razredi;
using Interface;
using Microsoft.AspNet.Identity;
using PortalVijesti.Models;

namespace PortalVijesti.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsRepository _repository;

        public HomeController(INewsRepository repository)
        {
            _repository = repository;
        }

        public Guid createGuidFromUserId(string user)
        {
            string padded = user.ToString().PadLeft(32, '0');
            var userId = new Guid(padded);
            return userId;
        }
        public IActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var username = User.Identity.GetUserName();
            Guid userId = Guid.Empty;
            if (user!=null)
            {
                userId = createGuidFromUserId(user);
            }
            bool korisnik = _repository.korisnikUBazi(userId,username);
            bool jeAdmin = _repository.jeAdmin(userId);
            bool jeAutor = _repository.jeAutor(userId);
            var lista = _repository.GetTop();
            var model = new HomePage(korisnik,jeAdmin,jeAutor,lista,userId);
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
