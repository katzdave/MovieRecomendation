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
            //ds.GenerateRandomFeatures(10);

            var wsm = new WebServerMain(ds);
            wsm.Run();
        }

        public static DataStore LoadDataStore()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("./ImdbDataStore.bin",
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            DataStore obj = (DataStore)formatter.Deserialize(stream);
            stream.Close();

            var nonull = new List<Movie>();

            foreach (var movie in obj.Movies)
            {
                if (movie.Title != null && movie.Poster != null && movie.Poster != "" && movie.Poster != "N/A")
                {
                    nonull.Add(movie);
                }
            }

            obj.Movies = nonull;
            obj.FeatureNames = new List<string>();

            var lines = System.IO.File.ReadAllLines("features.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                var split = lines[i].Split('~');
                if (i == 0)
                {
                    foreach (var feat in split)
                    {
                        obj.FeatureNames.Add(feat);
                    }
                }
                else
                {
                    foreach (var movie in obj.Movies)
                    {
                        if (movie.Id == Int32.Parse(split[0]))
                        {
                            movie.Features = new List<double>();
                            for (int j = 1; j < split.Length; j++)
                            {
                                movie.Features.Add(Double.Parse(split[j]));
                            }
                        }
                    }
                }
            }
            

            return obj;
        }
    }
}
