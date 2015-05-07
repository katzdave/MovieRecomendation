using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using MovieServer.Setup;


namespace MovieServer
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
                if (nresults <= 0) nresults = 5;
                if (nresults > 50) nresults = 50; 

                var features = JsonConvert.DeserializeObject<List<double>>(request.QueryString["features"]);
                var weights = JsonConvert.DeserializeObject<List<double>>(request.QueryString["weights"]);

                var knn = ds.GetKnn(nresults, features, weights);

                return JsonConvert.SerializeObject(knn);
            }
            return "response";
        }
    }
}
