using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Razredi
{
    public class News
    {
        public Guid NId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime DateCreated { get; set; }
        public double NumUpvote { get; set; }
        public double NumDownvote { get; set; }
        public Category Kategorija { get; set; }
        public List<Korisnik> Vote { get; set; }
        

        public News(Guid AId,string Title,string Text,Category kategorija)
        {
            this.NId = Guid.NewGuid();
            this.Title = Title;
            this.Text = Text;
            AuthorId = AId;
            DateCreated = DateTime.Now;
            NumUpvote = 0;
            NumDownvote = 0;
            this.Kategorija = kategorija;
            //Vote = new List<string>();
            //komentari = new List<Komentar>();

        }

        public News() { }
    }

    public enum Category
    {
        WorldNews,Showbizz,Sport,Weather
    }

    public class Korisnik
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        
        public bool zeliBitiAutor { get; set; }

        public Korisnik(Guid id, string name)
        {
            this.Id = id;
            this.name = name;
            zeliBitiAutor = false;
            
        }

        public Korisnik() { }
    }

    
    public class Author
    {
        public Guid Id { get; set; }
        

        public Author(Guid id)
        {
            Id = id;
            //vijesti = new List<News>();
        }
        public Author() { }
    }

    public class Administrator 
    {
        public List<Korisnik> zahtjevi { get; set; }
        //public List<News> vijesti { get; set; }
        public Guid Id { get; set; }

        public Administrator(Guid id) 
        {   
            Id = id;
            zahtjevi = new List<Korisnik>();
           // vijesti = new List<News>();

        }
        public Administrator() { }
    }

    public class Komentar
    {
        public string Text { get; set; }
        public Guid userId { get; set; }
        public Guid NId { get; set; }
        public Guid KId { get; set; }


        public Komentar(string Text, Guid id, Guid newsId)
        {
            KId = Guid.NewGuid();
            this.Text = Text;
            userId = id;
            NId = newsId;
                
        }
        public Komentar() { }
    }

   
    


}
