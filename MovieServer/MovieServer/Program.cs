using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using MovieServer.Setup;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MovieServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //SetupMain.main();

            DataStore ds = LoadDataStore();
            ds.GenerateRandomFeatures(10);

            var query = new List<double> { .1, .2, .3, .4, .5, .6, .7, .8, .9, 1 };
            var weights = new List<double> { .1, .1, .1, .1, .1, .1, .1, .1, .1, .1 };

            ds.GetKnn(5, query, weights);

            var wsm = new WebServerMain(ds);
            wsm.Run();
        }

        public static DataStore LoadDataStore()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("ImdbDataStore.bin",
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            DataStore obj = (DataStore)formatter.Deserialize(stream);
            stream.Close();

            var nonull = new List<Movie>();

            foreach (var movie in obj.Movies)
            {
                if (movie.Title != null)
                {
                    nonull.Add(movie);
                }
            }

            obj.Movies = nonull;

            return obj;
        }
    }
}
