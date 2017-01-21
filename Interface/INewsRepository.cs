using NewsDb;
using Razredi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface INewsRepository
    {
        List<News> GetCategory(Category category);
        List<Category> GetAllCategory();
        void RemoveNews(Guid NId, Guid userId);
        void AddNews( Guid userId, string Title, string Text,Category kategorija);
        void GiveUpvote(Guid NId, string name, Guid userId);
        void GiveDownvote(Guid NId,string name,Guid userId);
        void AddAuthor(Guid userId,Guid adminId);
        void Comment(Guid Nid, Guid userId, string Text);
        List<News> GetTop();
        bool AddAdministrator(Guid userId);
        News GetNewsData(Guid id);
        void GiveComment(Guid id, Guid userId,string name, string Text);
        List<Komentar> pronadiKomentare(Guid id);
        bool provjeriOvlasti(Guid userId);
        bool jeAdmin(Guid userId);
        bool jeAutor(Guid userId);
        bool korisnikUBazi(Guid userId,string name);
        void zahtjevZaAutora(Guid userId);
        void potvrdiZahtjev(Guid userId, Korisnik user);
        List<Korisnik> pregledajZahtjeve(Guid userId);
        Korisnik pronadiKorisnikaPoId(Guid Id);
        void UpdateNews(Guid NId, Guid UserId,string naslov, string text);
        News pronadiVijestPoId(Guid NId);
        string pronadiImePoId(Guid Id);
         
       
         
    }

    public class NewsRepository : INewsRepository
    {
        //private const string ConnectionString =
           // "Server=(localdb)\\mssqllocaldb;Database=NewsPortalDb101;Trusted_Connection=True;MultipleActiveResultSets=true";
          // "Server=tcp:vjestinacsharp.database.windows.net,1433;Initial Catalog=vijestiportal;Persist Security Info=False;User ID={miatomaic};Password={Aim1ludi#};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private NewsDbContext  _context; // relacijska baza podataka

        public NewsRepository(NewsDbContext context) // konstruktor repozitorija
        {
            _context = context;
        }

        public List<News> GetCategory(Category category)
        {
           // gets news from a specified category
           // using (_context=new NewsDbContext(ConnectionString))
            //{
                var lista = new List<News>();
                if (_context.News.Count()>0)
                {
                     lista = _context.News.Where(s => s.Kategorija == category).Select(s => s).ToList();
                }
               
                return lista;
            }
        //}

        //samo jedan admin za sada
        
        public bool AddAdministrator(Guid userId)
        {
            //using (_context = new NewsDbContext(ConnectionString))
           // {
               
                //var postojiAdmin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
               // if (postojiAdmin == null)
                //{
                _context.Administrator.Add(new Administrator(userId));
                _context.SaveChanges();
                    return true;
                //}
                //return false;
            }
       // }
        
        public List<Category> GetAllCategory()
        {
          //  using (_context = new NewsDbContext(ConnectionString))
            //{
                var lista = _context.News.Select(s=>s.Kategorija);
                var kategorije = lista.ToList();
                var values = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
                return values;
            }
       // }

        public News GetNewsData(Guid id)
        {
            if (id==null)
            {
                throw new ArgumentNullException();
            }
          //  using (_context = new NewsDbContext(ConnectionString))
            //{
                var vijesti = _context.News.Where(s => s.NId == id).Select(s => s);
                var vijest = vijesti.FirstOrDefault();
                return vijest;
            }
       // }


        public void AddNews( Guid userId, string Title,string Text, Category kategorija)
        {
            //author or administrator can add new news
            if ( userId==null)
            {
                throw new AccessDeniedException("Access denied!");
            }

            //using (_context = new NewsDbContext(ConnectionString))
            //{
                    //administrator ima proširene ovlasti u odnosu na autora
                    var currentAdmin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                

                if (currentAdmin == null)// ako nije administrator, provjera je li autor
                {
                    var currentAuthor = _context.Author.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    if (currentAuthor == null) //nije ni autor
                    {
                        throw new AccessDeniedException("Access denied!");

                    }
                    else
                    {
                        News nova = new News(userId, Title, Text, kategorija);
                       // nova.komentari = new List<Komentar>();
                       // nova.Vote = new List<string>();
                            _context.News.Add(nova);
                           // var admin = _context.Author.Where(s => s.Id == userId).Select(s => s).First();
                           // currentAuthor.vijesti.Add(new News(userId, Title, Text, kategorija));
                            _context.SaveChanges();
                        
                    }
                }
                else
                {

                        var novaVijest = new News(userId, Title, Text, kategorija);
                       // novaVijest.Vote = new List<string>();
                       // novaVijest.komentari = new List<Komentar>();
                        _context.News.Add(novaVijest);
                        _context.SaveChanges();
                       //currentAdmin.vijesti.Add(novaVijest);
                       
                    
                }
            }
        //}

        public void RemoveNews(Guid NId, Guid userId)
        {
            //author or administrator can remove news
            if (NId == null || userId == null)
            {
                throw new AccessDeniedException("Access denied!");
            }

            //using (_context = new NewsDbContext(ConnectionString))
           // {
                //administrator ima proširene ovlasti u odnosu na autora
                var currentAdmin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();

                if (currentAdmin == null)// ako nije administrator, provjera je li autor
                {
                    var currentAuthor = _context.Author.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    if (currentAuthor == null) //nije ni autor
                    {
                        throw new AccessDeniedException("Access denied!");

                    }
                    else
                    {
                      // autor briše vijesti kojima je autor
                        var odabrana = _context.News.Where(s=>s.NId==NId && s.AuthorId==currentAuthor.Id).Select(s=>s).FirstOrDefault();
                        var komentari = _context.Komentar.Where(s => s.NId == NId).Select(s => s).ToList();
                        _context.News.Remove(odabrana);
                        foreach (var item in komentari)
                        {
                          _context.Komentar.Remove(item);
                        }
                           // var admin = _context.Author.Where(s => s.Id == userId).Select(s => s).First();
                            //admin.vijesti.Remove(odabrana);
                         _context.SaveChanges();
                        

                    }
                }
                else
                {
                    //admin sve može brisati
                    var odabrana = _context.News.Where(s=>s.NId==NId).Select(s=>s).FirstOrDefault();
                    if (odabrana == null) // ne postoji već 
                    {
                        throw new AccessDeniedException("Access denied!");
                    }
                    else
                    {
                      var komentari = _context.Komentar.Where(s => s.NId == NId).Select(s => s).ToList();
                       _context.News.Remove(odabrana);
                      foreach (var item in komentari)
                      {
                        _context.Komentar.Remove(item);
                      }
                      _context.SaveChanges();
                    }
                }
           // }
        }

        public void GiveUpvote(Guid NId, string name, Guid userId)
        {
            if (NId==null || userId==null)
            {
                throw new ArgumentNullException();
            }
           // using (_context=new NewsDbContext(ConnectionString))
            //{
                //provjera je li korisnik već glasao
                var vijest = _context.News.Where(s=>s.NId==NId).Select(s => s.Vote.Where(h => h.Id == userId)).FirstOrDefault();
                var korisnik = vijest.FirstOrDefault();
                if (korisnik == null)
                {
                    var odabranaVijest = _context.News.Where(s => s.NId == NId).Select(s => s).First();
                    if (odabranaVijest.Vote==null)
                    {
                        odabranaVijest.Vote = new List<Korisnik>();
                    }
                    var korisnikUBazi = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    if (korisnikUBazi==null)
                    {
                        korisnikUBazi = new Korisnik(userId,name);
                        _context.Korisnik.Add(korisnikUBazi);
                    }
                    odabranaVijest.Vote.Add(korisnikUBazi);
                    odabranaVijest.NumUpvote += 1;
                    _context.SaveChanges();
                }
                else
                {
                    //throw new AccessDeniedException("Access denied");
                    //ukoliko ponovo glasa poništava si glas
                    var odabranaVijest = _context.News.Where(s => s.NId == NId).Select(s => s).First();
                    var korisnikUBazi = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    odabranaVijest.Vote.Remove(korisnikUBazi);
                    odabranaVijest.NumUpvote -= 1;
                    _context.SaveChanges();
                }
            //}
        }

        public void GiveDownvote(Guid NId, string name, Guid userId)
        {
            if (NId == null || userId == null)
            {
                throw new ArgumentNullException();
            }
           // using (_context = new NewsDbContext(ConnectionString))
            //{
                //provjera je li korisnik već glasao
                var vijest = _context.News.Where(s=>s.NId==NId).Select(s => s.Vote.Where(h => h.Id == userId).FirstOrDefault());
                var korisnik = vijest.FirstOrDefault();
                if (korisnik == null)
                {
                    var odabranaVijest = _context.News.Where(s => s.NId == NId).Select(s => s).First();
                    if (odabranaVijest.Vote == null)
                    {
                        odabranaVijest.Vote = new List<Korisnik>();
                    }
                    var korisnikUBazi = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    if (korisnikUBazi == null)
                    {
                        korisnikUBazi = new Korisnik(userId,name);
                        _context.Korisnik.Add(korisnikUBazi);
                    }
                    odabranaVijest.Vote.Add(korisnikUBazi);
                    odabranaVijest.NumDownvote += 1;
                    _context.SaveChanges();
                }
                else
                {
                //throw new AccessDeniedException("Access denied");
                //ukoliko ponovo glasa poništava si glas
                var odabranaVijest = _context.News.Where(s => s.NId == NId).Select(s => s).First();
                var korisnikUBazi = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                odabranaVijest.Vote.Remove(korisnikUBazi);
                odabranaVijest.NumDownvote -= 1;
                _context.SaveChanges();
            }
           // }
        }

        public void AddAuthor(Guid userId, Guid adminId)
        {
            if (userId==null || adminId==null)
            {
                throw new ArgumentNullException();
            }
           // using (_context=new NewsDbContext(ConnectionString))
            //{
                var admin = _context.Administrator.Where(s => s.Id == adminId).Select(s => s).FirstOrDefault();
                if (admin==null)
                {
                    throw new AccessDeniedException("Access denied!");
                }
                else
                {
                    //var noviAutor = admin.zahtjevi.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    //_context.Author.Add(new Author(noviAutor.Id)); // dodajemo ga u relaciju autor 
                   // _context.Korisnik.Remove(noviAutor); // mičemo ga iz relacije korisnik
                    //_context.SaveChanges();
                }
            //}

        }

        public void Comment(Guid Nid, Guid userId, string Text)
        {
            if (Nid==null || userId==null || Text==null)
            {
                throw new ArgumentNullException();
            }
           // using (_context=new NewsDbContext(ConnectionString))
            //{
                _context.Komentar.Add(new Komentar(Text,userId,Nid));
                _context.SaveChanges();
           // }
        }

        public List<News> GetTop()
        {
            List<News> lista = null;
           // using (_context=new NewsDbContext(ConnectionString))
            //{
                if (_context.News.Count()>0)
                { 
                    var values = Enum.GetValues(typeof(Category)).Cast<Category>();
                    foreach (var value in values)
                    {
                        
                        lista = _context.News.Where(s => (s.NumUpvote + s.NumDownvote)==_context.News.Max(h=>h.NumUpvote+h.NumDownvote)).OrderByDescending(s=>s.Kategorija).Select(s => s).ToList();
                        
                    }
                //}
            }
            return lista;
        }

        public void GiveComment(Guid id, Guid userId, string name, string Text)
        {
            if (id==null || userId==null || Text==null)
            {
                throw new ArgumentNullException();
            }
            //using (_context = new NewsDbContext(ConnectionString))
            //{
                //korisnik može komentirati više puta
               
                var korisnikUBazi = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                if (korisnikUBazi == null)
                {
                    korisnikUBazi = new Korisnik(userId, name);
                    _context.Korisnik.Add(korisnikUBazi);
                }
                var komentar = new Komentar(Text, userId, id);
                _context.Komentar.Add(komentar);
                _context.SaveChanges();
            //}

        }

         public  List<Komentar> pronadiKomentare(Guid id)
        {
            if (id==null)
            {
                throw new ArgumentNullException();
            }
            //using (_context = new NewsDbContext(ConnectionString))
            //{
                var lista = _context.Komentar.Where(s => s.NId == id).Select(s => s).ToList();
                return lista;
            //}
        }

        public bool provjeriOvlasti(Guid userId)
        {
            if (userId == null)
            {
                return false;
            }
           // using (_context = new NewsDbContext(ConnectionString))
            //{
                var currentAdmin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();

                if (currentAdmin == null)// ako nije administrator, provjera je li autor
                {
                    var currentAuthor = _context.Author.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                    if (currentAuthor == null)
                    {
                        return false;
                    }

                }
                return true;
            //}
        }
       public bool korisnikUBazi(Guid userId,string name)
        {
            if (userId==null || name==string.Empty || userId==Guid.Empty)
            {
                return false;
            }
           // using (_context = new NewsDbContext(ConnectionString))
            //{
                var korisnik = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                if (korisnik == null)
                {
                    //dodajemo prijavljenog korisnika u bazu
                    _context.Korisnik.Add(new Korisnik(userId,name));
                    _context.SaveChanges();
                }
                      
                return true;
            //}
        }

        public void zahtjevZaAutora(Guid userId)
        {
            //samo prijavljeni korisnik može slati zahtjev
            if (userId==null)
            {
                throw new ArgumentNullException();
            }
           // using (_context = new NewsDbContext(ConnectionString))
            //{
                var korisnik = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                if (korisnik!=null)
                {
                    korisnik.zeliBitiAutor = true;
                    _context.SaveChanges();
                }
                  
           // }
        }

        public List<Korisnik> pregledajZahtjeve(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }
            //using (_context = new NewsDbContext(ConnectionString))
            //{
                List<Korisnik> lista = null;
                //provjera je li user admin
                var admin = _context.Administrator.Where(s=>s.Id==userId).Select(s => s).FirstOrDefault();
                if (admin != null)
                {
                    lista = _context.Korisnik.Where(s => s.zeliBitiAutor == true).Select(s => s).ToList();               
                }
                return lista;
            //}

        }

        public void potvrdiZahtjev(Guid userId, Korisnik user)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }
           // using (_context = new NewsDbContext(ConnectionString))
            //{
                //samo admin može dodati novog autora
                var admin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                if (admin!=null)
                {
                    _context.Author.Add(new Author(user.Id));
                    user.zeliBitiAutor = false;
                    _context.SaveChanges();
                }

            //}

        }

        public Korisnik pronadiKorisnikaPoId(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException();
            }
            //using (_context = new NewsDbContext(ConnectionString))
           // {
                var korisnik = _context.Korisnik.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
                return korisnik;
           // }
        }

        public bool jeAdmin(Guid userId)
        {
            if (userId==null)
            {
                return false;
            }
            var admin = _context.Administrator.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
            if (admin != null)
            {
                return true;
            }
            return false;
        }

        public void UpdateNews(Guid NId, Guid UserId, string naslov, string text)
        {
            if (NId==null || UserId == null)
            {
                throw new ArgumentNullException();
            }
            //provjera je li korisnik admin
            var currentAdmin = _context.Administrator.Where(s => s.Id == UserId).Select(s => s).FirstOrDefault();
            if (currentAdmin==null)
            {
                //provjera je li autor
                var currentAuthor = _context.Author.Where(s => s.Id == UserId).Select(s => s).FirstOrDefault();
                if (currentAuthor==null)
                {
                    throw new AccessDeniedException("Access denied!");
                }
                else
                {
                    var vijest = _context.News.Where(s => s.NId == NId && s.AuthorId == currentAuthor.Id).Select(s => s).FirstOrDefault();
                    if (vijest != null)
                    {
                        vijest.Title = naslov;
                        vijest.Text = text;
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                var vijest = _context.News.Where(s => s.NId == NId).Select(s => s).FirstOrDefault();
                if (vijest!=null)
                {
                    vijest.Title = naslov;
                    vijest.Text = text;
                    _context.SaveChanges();
                }
            }
                        
        }

        public News pronadiVijestPoId(Guid NId)
        {
            if (NId == null)
            {
                throw new ArgumentNullException();
            }
            var trazenaVijest = _context.News.Where(n => n.NId == NId).Select(n => n).FirstOrDefault();
            return trazenaVijest;
        }

        public string pronadiImePoId(Guid Id)
        {
            if (Id == null)
            {
                throw new ArgumentNullException();
            }
            var name = _context.Korisnik.Where(s => s.Id == Id).Select(s => s.name).FirstOrDefault();
            return name;
        }

        public bool jeAutor(Guid userId)
        {
            if (userId==null)
            {
                return false;
            }
            var autor = _context.Author.Where(s => s.Id == userId).Select(s => s).FirstOrDefault();
            if (autor!=null)
            {
                return true;
            }
            return false;
        }

    }

    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message)
        {

        }
    }

    public class DuplicateNewsException : Exception
    {
        public DuplicateNewsException(string message) : base(message)
        {

        }
    }

}
