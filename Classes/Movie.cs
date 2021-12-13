using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch
{
    class Movie
    {
        public bool adult { get; set; }
        public int[] genre_ids { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
        public int runtime { get; set; }
        public string homepage { get; set; }
        public string PosterPathSource = "https://image.tmdb.org/t/p/w500";
        public void PrintInfo()
        {
            Console.Clear();
            Console.WriteLine("-".PadLeft(50, '-'));
            Console.WriteLine("Titel: "+title);
            Console.WriteLine("\nBeskrivning: "+overview);
            Console.WriteLine("\nGenomsnittligt betyg: {0} av {1} röster", vote_average,vote_count);
            Console.WriteLine("\nSpeltid: {0}", runtime);
            Console.WriteLine("\nUrsprångsspråk: {0}",original_language);
            Console.WriteLine("\nPremiärdatum: {0}",release_date);
            Console.WriteLine("\nHemsida: {0}",homepage);
            Console.WriteLine("\nPosterlänk: {0}",PosterPathSource+poster_path);
        }

        public Movie()
        {

        }
        /*
        public class Rootobject
        {

            public Genre[] genres { get; set; }

        }
        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        */



    }
}
