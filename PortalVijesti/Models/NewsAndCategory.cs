using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razredi;

namespace PortalVijesti.Models
{
    public class NewsAndCategory 
    {
        public List<News> CNews { get; set; }
        public Category category { get; set; }
        public bool isAuthorOrAdmin { get; set; }
        public bool IsAdmin { get; set; }
        public bool isKorisnik { get; set; }
        public Guid userid { get; set; }

        public NewsAndCategory(List<News> vijesti, Category kategorija, bool ovlasti,bool admin,bool korisnik,Guid id)
        {
            CNews = vijesti;
            category = kategorija;
            isAuthorOrAdmin = ovlasti;
            IsAdmin = admin;
            isKorisnik = korisnik;
            userid = id;
        }

    }
}
