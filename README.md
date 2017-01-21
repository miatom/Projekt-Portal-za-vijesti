# Projekt-Portal-za-vijesti
#link na stranicu: http://portalsvijestima.azurewebsites.net

- omogućen je pregled članaka po kategoriji
- pregled svih vijesti unutar pojedine kategorije
- za autora i administratora omogućeno je dodavanje nove vijesti
- administrator sve članke može brisati ili uređivati,
 autor ima ovlasti samo nad člancima koje je osobno kreirao
- autor, admin i korisnik mogu dati recenziju (upvote i downvote)
 te komentirati pojedini članak
- mogu vidjeti komentare drugih korisnika
- ponovnim odabirom iste recenzije glas se poništava
- korisnik može poslati zahtjev kako bi postao autor
- administrator ima mogućnost pregleda zahtjeva te promoviranja korisnika
   u klasu autor
- neprijavljen korisnik vidi samo početnu stranicu na kojoj se nalaze top N
  članaka (s najvećim brojem recenzija), članke ne može čitati bez prethodne prijave
- omogućena je prijava s lokalnog računa
- prijava s Google računom postoji samo kao mogućnost (zasada), ali nažalost ne radi
  (postoji neka greška koju je potrebno ispraviti)
- što se tiče testiranja aplikacije svaka nova prijava (local account) stvara korisnika koji ima prethodno
  opisane ovlasti, ukoliko se želi vidjeti "autorska" strana logirati se možete pomoću idućeg 
  računa email: autor1@mail.com te password: Autor1!
