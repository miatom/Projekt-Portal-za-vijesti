using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razredi;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalVijesti.Models
{
    public class VijestZaPrikaz 
    {
        public List<Komentar> komentari { get; set; }
        public News vijest { get; set; }
        public string username { get; set; }
        public bool isKorisnik { get; set; }
        public bool IsAdmin { get; set; }
        public Guid userid { get; set; }
        public List<Comment> comments { get; set; }
       

        public VijestZaPrikaz(List<Komentar> komentariVijesti, News vijest, string userName, bool korisnik,bool jeAdmin, Guid userId,List<Comment> comments)
        {
            komentari = komentariVijesti;
            this.vijest = vijest;
            username = userName;
            isKorisnik = korisnik;
            IsAdmin = jeAdmin;
            userid = userId;
            this.comments = comments;
        }
    }

    public class Comment
    {
        public string text { get; set; }
        public string username { get; set; }

        public Comment(string text, string username)
        {
            this.text = text;
            this.username = username;
        }
    }

   
}
