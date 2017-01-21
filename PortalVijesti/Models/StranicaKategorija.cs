using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razredi;

namespace PortalVijesti.Models
{
    public class StranicaKategorija 
    {
       public List<Category> sveKategorije { get; set; }
        public bool isKorisnik { get; set; }
        public Guid userId { get; set; }
        public bool isAuthorOrAdmin { get; set; }
        public bool IsAdmin { get; set; }

        public StranicaKategorija(List<Category> kat, bool isKorisnik, Guid userId,bool ovlasti,bool jeAdmin)
        {
            sveKategorije = kat;
            this.isKorisnik = isKorisnik;
            this.userId = userId;
            isAuthorOrAdmin = ovlasti;
            IsAdmin = jeAdmin;
        }

    }
}
