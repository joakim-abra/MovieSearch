using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieSearch.Classes;
using Newtonsoft.Json;

namespace MovieSearch.Classes
{

    class Menu
    {
        public static HttpClient client = new HttpClient();



        public async Task SearchID()
        {
            DotNetEnv.Env.TraversePath().Load();
            string key = Environment.GetEnvironmentVariable("API_KEY");
            Console.Write("Enter ID:");
            int id = Convert.ToInt32(Console.ReadLine());
            string uriID = $"https://api.themoviedb.org/3/movie/{id}?api_key={key}";


            var response = await client.GetAsync(uriID);
            try
            {
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Movie movie = JsonConvert.DeserializeObject<Movie>(responseContent);
                movie.PrintInfo();
            }
            catch (Exception)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("Hittar ej matchande film på angivet ID");
            } 
            
        }
        
        public async Task SearchTitle()
        {

            DotNetEnv.Env.TraversePath().Load();
            string key = Environment.GetEnvironmentVariable("API_KEY");
            Console.Write("Skriv in sökord: ");
            string search = Console.ReadLine().ToLower();

            string uriSearch = $"https://api.themoviedb.org/3/search/movie?api_key={key}&language=en-US&query={search}&page=1&include_adult=false";
            var searchResponse = await client.GetAsync(uriSearch);

            try
            {
                searchResponse.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                Console.WriteLine(searchResponse);
            }

            string search1 = await searchResponse.Content.ReadAsStringAsync();

            Search searchResults = JsonConvert.DeserializeObject<Search>(search1);
            int i = 1;
            Console.WriteLine("{0} resultat hittades på {1} sidor:", searchResults.total_results, searchResults.total_pages);
            foreach (var item in searchResults.results)
            {
                Console.WriteLine(i + ") " + item.original_title);
                i++;
            }
            while (true)
            {
                Console.Write("Välj träff: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice <= searchResults.results.Count && choice >= 0)
                {
                    if (choice != 0)
                    {
                        searchResults.results[choice - 1].PrintInfo();
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val");
                }
            }
        }
        
        public string RunMenu()
        {

                Console.WriteLine("1) Sök film med ID");
                Console.WriteLine("2) Sök film efter titel");
                Console.WriteLine("3) Avsluta");

                string choice = Console.ReadLine();
            return choice;
        }
        public void MainMenu()
        {
            while (true)
            {
                string choice = RunMenu();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        SearchID().Wait();
                        Helper();
                        break;
                    case "2":
                        Console.Clear();
                        SearchTitle().Wait();
                        Helper();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Programmet avslutas.");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Ogiltigt val!\n");
                        break;

                }
            }
        }

        private static void Helper()
        {
            Console.WriteLine("_".PadLeft(50,'_'));
            Console.WriteLine("\nTryck på valfri tangent för att återgå till huvudmeny...");
            Console.ReadKey();
            Console.Clear();
        }
        public Menu()
        {

        }


    }
}
