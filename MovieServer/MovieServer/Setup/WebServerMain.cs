using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace MovieServer.Setup
{
    class WebServerMain
    {
        DataStore ds;
        WebServer ws;

        Random random = new Random();

        public WebServerMain(DataStore ds)
        {
            this.ds = ds;
            ws = new WebServer(SendResponse, "http://localhost:11111/test/");
        }

        public void Run()
        {
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
        
        public string SendResponse(HttpListenerRequest request)
        {
            if (request.HttpMethod.Equals("GET"))
            {
                return JsonConvert.SerializeObject(ds);
            }
            else if (request.HttpMethod.Equals("POST"))
            {
                int nresults;

                Int32.TryParse(request.QueryString["nresults"], out nresults);
                if (nresults <= 0)
                {
                    nresults = 5;
                }

                var features = JsonConvert.DeserializeObject<List<double>>(request.QueryString["features"]);
                var weights = JsonConvert.DeserializeObject<List<double>>(request.QueryString["results"]);

                var hs = new HashSet<int>();
                while (hs.Count() < nresults)
                {
                    hs.Add(random.Next(0, ds.Movies.Count()));
                }

                var movies = new List<Movie>();
                foreach (var h in hs)
                {
                    movies.Add(ds.Movies[h]);
                }

                return JsonConvert.SerializeObject(movies);
            }
            return "response";
        }
    }
}
