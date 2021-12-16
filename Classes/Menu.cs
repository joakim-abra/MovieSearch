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
            int id;
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter (0) to return to main menu");
                    Padder();
                    Console.Write("Enter ID: ");
                    id = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid ID; ID can only contain numbers and must not be empty!\n");
                }
            }
            if (id == 0)
            {
                return;
            }
            string uriID = $"https://api.themoviedb.org/3/movie/{id}?api_key={key}";


            var response = await client.GetAsync(uriID);
            try
            {
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                Movie movie = JsonConvert.DeserializeObject<Movie>(responseContent);
                movie.PrintInfo();
                Helper();
            }
            catch (Exception)
            {
                Console.WriteLine("Error {1}: {0}", response.StatusCode, (int)response.StatusCode);
                Exception((int)response.StatusCode);
            }

        }


        public async Task SearchTitle()
        {

            DotNetEnv.Env.TraversePath().Load();
            string key = Environment.GetEnvironmentVariable("API_KEY");
            string search;
            while (true)
            {
                Console.Write("Enter search term: ");
                search = Console.ReadLine().ToLower();
                if (search != "")
                {

                    break;
                }
                else
                {
                    Console.WriteLine("Error: Invalid search term!");
                }
            }
            int page = 1;
            while (true)
            {
                string uriSearch = $"https://api.themoviedb.org/3/search/movie?api_key={key}&language=en-US&query={search}&page={page}&include_adult=false";
                var searchResponse = await client.GetAsync(uriSearch);

                try
                {
                    searchResponse.EnsureSuccessStatusCode();
                }
                catch (Exception)
                {
                    Console.WriteLine("\nError {1}: {0}", searchResponse.StatusCode, (int)searchResponse.StatusCode);
                    Exception((int)searchResponse.StatusCode);
                    return;
                }

                string search1 = await searchResponse.Content.ReadAsStringAsync();

                Search searchResults = JsonConvert.DeserializeObject<Search>(search1);
                Console.WriteLine("{0} results found on {1} total page(s):\n", searchResults.total_results, searchResults.total_pages);
                Console.WriteLine("Page: {0} of {1}", page, searchResults.total_pages);
                Padder();
                foreach (var item in searchResults.results)
                {
                    Console.WriteLine(searchResults.results.IndexOf(item) + 1 + ") " + item.title);
                }
                if (searchResults.total_pages > 1)
                {
                    if (searchResults.total_pages != page)
                    {
                        Padder();
                        Console.WriteLine(searchResults.results.Count + 1 + ") View next page");
                        if (searchResults.total_pages != page && page != 1)
                        {
                            Console.WriteLine(searchResults.results.Count + 2 + ") View previous page");
                        }

                    }
                    else
                    {
                        Padder();
                        Console.WriteLine(searchResults.results.Count + 1 + ") View previous page");
                    }
                }
                else
                {
                    Padder();
                }
                int choice = -1;

                Console.WriteLine("0) Return to main menu");
                Padder();
                Console.Write("Selection: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                }
                catch (Exception)
                {
                    //Console.WriteLine("Invalid choice!");
                }

                if (choice <= searchResults.results.Count && choice >= 0)
                {

                    if (choice != 0)
                    {
                        searchResults.results[choice - 1].PrintInfo();
                        Helper();
                    }
                    else
                    {
                        Console.Clear();
                        return;
                    }
                }
                else if (choice > searchResults.results.Count && searchResults.total_pages > 1)
                {
                    if (choice == searchResults.results.Count + 1 && page != searchResults.total_pages)
                    {
                        page++;
                    }
                    else if (choice == searchResults.results.Count + 2 && page != searchResults.total_pages && page != 1)
                    {
                        page--;
                    }
                    else if (choice == searchResults.results.Count + 1 && page == searchResults.total_pages)
                    {
                        page--;
                    }
                    else
                    {
                        Console.WriteLine("Error: invalid selection!");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Invalid selection!");

                }
            }
        }

        public string RunMenu()
        {
            Console.WriteLine("MovieSearch".PadRight(18, '-').PadLeft(25, '-'));
            Console.WriteLine("1) Find movie by ID");
            Console.WriteLine("2) Search movie by title");
            Console.WriteLine("3) Exit\n".PadRight(33, '-'));
            Console.Write("Selection: ");
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
                        break;
                    case "2":
                        Console.Clear();
                        SearchTitle().Wait();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Exiting program.\n");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error: Invalid selection!\n");
                        break;

                }
            }
        }

        private static void Helper()
        {
            Padder();
            Console.Write("\nPress any key to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void Padder()
        {
            Console.WriteLine("-".PadLeft(50, '-'));
        }

        public void Exception(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    Console.WriteLine("No match found for that ID\n");
                    break;
                case 503:
                    Console.WriteLine("Server unavailable\n");
                    break;
                case 500:
                    Console.WriteLine("Server error\n");
                    break;
                case 401:
                    Console.WriteLine("Unauthorized request, check API key\n");
                    break;
                default:
                    Console.WriteLine("Unable to complete request");
                    break;
            }
            Helper();
        }

        public Menu()
        {

        }


    }
}
