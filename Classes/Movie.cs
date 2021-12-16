using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieSearch.Classes;

namespace MovieSearch
{
    class Movie
    {
        public static HttpClient client = new HttpClient();
        public List<Genre> genres { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
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
            Console.WriteLine("Movie ID: "+id);
            Menu.Padder();
            Console.WriteLine("Title: "+title);

                if (genres !=null)
                {
                    Console.Write("\nGenre(s): ");
                    foreach (var item in genres)
                    {
                    if (genres.IndexOf(item)!=genres.Count-1)
                    {
                        Console.Write(item.name + ", ");
                    }
                        else
                    {
                        Console.Write(item.name);
                    }
                    }
                    Console.WriteLine();
                }

            Console.WriteLine("\nOverview: "+overview);
            Console.WriteLine("\nAverage score: {0}/10 by {1} voters", vote_average,vote_count);
            switch(runtime)
            {
                case 0: GetMissingInfo().Wait();
                    break;
                default: Console.WriteLine("\nRuntime: {0} minutes", runtime);
                         Console.WriteLine("\nHomepage: {0}", homepage);
                    break;
            } 
                
            switch(original_language)
            {
                case "en": Console.WriteLine("\nOriginal language: English");
                    break;
                case "sv": Console.WriteLine("\nOriginal language: Swedish");
                    break;
                case "de": Console.WriteLine("\nOriginal language: German");
                    break;
                case "fr": Console.WriteLine("\nOriginal language: French");
                    break;
                case "es": Console.WriteLine("\nOriginal language: Spahish");
                    break;
                default: Console.WriteLine("\nOriginal language: {0}",original_language);
                    break;
            }
            
            Console.WriteLine("\nRelease Date: {0}",release_date);
            
            Console.WriteLine("\nPoster: {0}",PosterPathSource+poster_path);
        }



        //Gets runtime, genres and homepage by utilizing ID-search
        public async Task GetMissingInfo()
        {
                
        DotNetEnv.Env.TraversePath().Load();
            string key = Environment.GetEnvironmentVariable("API_KEY");
            string uriID = $"https://api.themoviedb.org/3/movie/{id}?api_key={key}";


            var response = await client.GetAsync(uriID);
            try
            {
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Movie movie = JsonConvert.DeserializeObject<Movie>(responseContent);
                Console.WriteLine("\nRuntime: {0} minutes",movie.runtime);
                Console.WriteLine("\nHomepage: {0}", movie.homepage);
                Console.Write("\nGenre(s): ");
                foreach (var item in movie.genres)
                {
                    if (movie.genres.IndexOf(item) != movie.genres.Count - 1)
                    {
                        Console.Write(item.name + ", ");
                    }
                    else
                    {
                        Console.Write(item.name);
                    }
                }
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("Unable to find any match");
            }

        }

        public Movie()
        {

        }

        public class Genre
        {
            public int id { get; set; }
            public string name { get; set; }
        }

    }
}
