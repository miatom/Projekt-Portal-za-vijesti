using Razredi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsDb
{
    public class NewsDbContext : System.Data.Entity.DbContext
    {
        public IDbSet<News> News { get; set; }
        public IDbSet<Korisnik> Korisnik { get; set; }
        public IDbSet<Author> Author { get; set; }
        public IDbSet<Administrator> Administrator { get; set; }
        public IDbSet<Komentar> Komentar { get; set; }
       

        public NewsDbContext(string connectionString) : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<News>().HasKey(s => s.NId);
            modelBuilder.Entity<News>().Property(s => s.Title).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.Text).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.AuthorId).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.DateCreated).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.NumUpvote).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.NumDownvote).IsRequired();
            modelBuilder.Entity<News>().Property(s => s.Kategorija).IsRequired();
            modelBuilder.Entity<News>().HasMany(s=>s.Vote);
            //modelBuilder.Entity<News>().HasMany(s=>s.komentari);

            modelBuilder.Entity<Korisnik>().HasKey(s => s.Id);
            modelBuilder.Entity<Korisnik>().Property(s => s.name);
            modelBuilder.Entity<Korisnik>().Property(s => s.zeliBitiAutor);
            
            modelBuilder.Entity<Author>().HasKey(s => s.Id);
            //modelBuilder.Entity<Author>().HasMany(s => s.vijesti);

            modelBuilder.Entity<Administrator>().HasKey(s => s.Id);
            modelBuilder.Entity<Administrator>().HasMany(s => s.zahtjevi);
            //modelBuilder.Entity<Administrator>().HasMany(s => s.vijesti);

            modelBuilder.Entity<Komentar>().HasKey(s => s.KId);
            modelBuilder.Entity<Komentar>().Property(s=>s.NId);
            modelBuilder.Entity<Komentar>().Property(s => s.userId).IsRequired();
            modelBuilder.Entity<Komentar>().Property(s => s.Text).IsRequired();

            
        }
    }
}
