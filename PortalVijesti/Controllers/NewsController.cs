using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Interface;
using Microsoft.AspNet.Identity;
using Razredi;
using PortalVijesti.Models;

namespace PortalVijesti.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _repository;

        public NewsController(INewsRepository repository)
        {
            _repository = repository;
        }



        public Guid createGuidFromUserId(string user)
        {
            string padded = user.ToString().PadLeft(32, '0');
            var userId = new Guid(padded);
            return userId;
        }

        
        public IActionResult AddAdmin()
        {
            var user = User.Identity.GetUserId();
            if (user == null)
            {
                return RedirectToAction("Sorry");
            }
            var userId = createGuidFromUserId(user);

            _repository.AddAdministrator(userId);
                return RedirectToAction("Index2");
            
            
        }
        
        public IActionResult AddNews(Category category,string Naslov, string Text)
        {
            var user = User.Identity.GetUserId();
            if (user == null)
            {
                return RedirectToAction("Sorry");
            }
            var userId = createGuidFromUserId(user);
            _repository.AddNews(userId,Naslov,Text,category);
            return RedirectToAction("NewsList",new { category=category });

        }

        public IActionResult ReadNews(Guid id)
        {
            var userName = User.Identity.GetUserName();
            var user = User.Identity.GetUserId();
            bool korisnik = false;
            bool jeAdmin = false;
            Guid userId = Guid.Empty;
            List<Comment> comments = new List<Comment>();
            if (user != null)
            {
                userId = createGuidFromUserId(user);
                korisnik = _repository.korisnikUBazi(userId, userName);
                jeAdmin = _repository.jeAdmin(userId);
            }
            
            var vijest = _repository.GetNewsData(id);
            var komentari = _repository.pronadiKomentare(id);
            foreach (var item in komentari)
            {
                var ime = _repository.pronadiImePoId(item.userId);
                var tekst = item.Text;
                comments.Add(new Comment(tekst,ime));
            }
           
            var prikaz = new VijestZaPrikaz(komentari,vijest,userName,korisnik,jeAdmin,userId,comments);
            return View(prikaz);
        }

        public IActionResult Index2()
        {
            var user = User.Identity.GetUserId();
            if (user==null)
            {
                return RedirectToAction("Sorry");
            }
            var userId = createGuidFromUserId(user);
            var userName = User.Identity.GetUserName();
            List<Category> listaKategorija = _repository.GetAllCategory();
            bool korisnik = _repository.korisnikUBazi(userId,userName);
            bool ovlasti = _repository.provjeriOvlasti(userId);
            bool jeAdmin = _repository.jeAdmin(userId);
            var model = new StranicaKategorija(listaKategorija,korisnik,userId,ovlasti,jeAdmin);

            return View(model);
        }

        //ukoliko se korisnik nije prijavio
        public IActionResult Sorry()
        {
            return View();
        }

        //prijavljeni korisnici ovo vide
        public IActionResult NewsList(Category category)
        {
            var user = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();
            var userId = createGuidFromUserId(user);
            bool ovlasti = _repository.provjeriOvlasti(userId);
            bool admin = _repository.jeAdmin(userId);
            bool korisnik = _repository.korisnikUBazi(userId,userName);
            var vijestiIzKategorija = _repository.GetCategory(category);
            var model = new NewsAndCategory(vijestiIzKategorija,category,ovlasti,admin,korisnik,userId);
            return View(model);
        }

        public IActionResult GiveUpvote(Guid id)
        {
            try
            {
                var user = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();
                var userId = createGuidFromUserId(user);
                _repository.GiveUpvote(id,userName, userId);
                return RedirectToAction("ReadNews", new { id = id });
            }
            catch (AccessDeniedException)
            {
                return RedirectToAction("ReadNews", new { id = id });
            }

        }

        public IActionResult GiveDownVote(Guid id)
        {
            try
            {
                var user = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();
                var userId = createGuidFromUserId(user);
                _repository.GiveDownvote(id,userName, userId);
                return RedirectToAction("ReadNews", new { id = id });
            }
            catch (AccessDeniedException)
            {
                return RedirectToAction("ReadNews", new { id = id });
            }
        }

        public IActionResult AddComment(Guid id,string Text)
        {
            
                var user = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();
                var userId = createGuidFromUserId(user);
                _repository.GiveComment(id,userId,userName,Text);
                return RedirectToAction("ReadNews", new { id = id });

        }

        public IActionResult AutorZahtjev(Guid Id,Category category)
        {
            _repository.zahtjevZaAutora(Id);

            return RedirectToAction("Index2");
        }

        public IActionResult pogledajZahtjeve()
        {
            var user = User.Identity.GetUserId();
            var userId = createGuidFromUserId(user);
            var zahtjevi = _repository.pregledajZahtjeve(userId);
            return View(zahtjevi);
        }

        public IActionResult odobriZahtjev(Guid Id)
        {
            var user = User.Identity.GetUserId();
            var userId = createGuidFromUserId(user);
            var korisnik = _repository.pronadiKorisnikaPoId(Id);
            _repository.potvrdiZahtjev(userId,korisnik);
            return RedirectToAction("pogledajZahtjeve");
        }

        public IActionResult DeleteNews(Guid id,Category category)
        {
            var user = User.Identity.GetUserId();
            var userId = createGuidFromUserId(user);
            _repository.RemoveNews(id, userId);
            return RedirectToAction("NewsList", new { category = category });
        }

        public IActionResult UpdateNews(Guid Nid)
        {
            var user = User.Identity.GetUserId();
            var userId = createGuidFromUserId(user);
            News vijest = _repository.pronadiVijestPoId(Nid);
            return View(vijest);
        }

        public IActionResult ChangeNews(Guid NId,string naslov,string text)
        {
            var user = User.Identity.GetUserId();
            var userId = createGuidFromUserId(user);
            _repository.UpdateNews(NId,userId,naslov,text);
            return RedirectToAction("ReadNews", new { id = NId });
        }

        
    }
}