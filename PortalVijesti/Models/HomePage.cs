using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razredi;

namespace PortalVijesti.Models
{
    public class HomePage 
    {
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAuthor { get; set; }
        public List<News> lista { get; set; }
        public Guid userid { get; set; }

        public HomePage(bool user, bool admin,bool author, List<News> vijesti, Guid userId)
        {
            IsUser = user;
            IsAdmin = admin;
            IsAuthor = author;
            lista = vijesti;
            userid = userId;
        }
    }
}
