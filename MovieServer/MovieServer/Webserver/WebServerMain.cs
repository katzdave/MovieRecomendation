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
            //ws = new WebServer(SendResponse, "http://169.254.231.73:11111/test/");
            ws = new WebServer(SendResponse, "http://localhost:11111/test/");
            //ws = new WebServer(new string[] { "http://199.98.20.55:11111/test/", "http://localhost:11111/test/" }, SendResponse);
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
                if (request.QueryString["nofeat"] != null)
                {
                    return JsonConvert.SerializeObject(ds.Movies);
                }
                return JsonConvert.SerializeObject(ds);
            }
            else if (request.HttpMethod.Equals("POST"))
            {
                int nresults;

                Int32.TryParse(request.QueryString["nresults"], out nresults);
                if (nresults <= 0) nresults = 5;
                if (nresults > 50) nresults = 50;

                var features = request.QueryString["features"].Split(',');
                var featuresNorm = new List<double>();
                foreach (var feature in features) {
                    featuresNorm.Add(Double.Parse(feature)/100);
                }

                var weights = request.QueryString["weights"].Split(',');
                var weightsNorm = new List<double>();
                double weightSum = 0;
                foreach (var weight in weights)
                {
                    weightSum += Double.Parse(weight);
                }
                //if (weightSum == 0) return null;
                foreach (var weight in weights)
                {
                    if (weightSum != 0)
                        weightsNorm.Add(Double.Parse(weight) / weightSum);
                    else
                        weightsNorm.Add(1.0 / weights.Count());
                }

                var knn = ds.GetKnn(nresults, featuresNorm, weightsNorm);

                return JsonConvert.SerializeObject(knn);
            }
            return "response";
        }
    }
}
