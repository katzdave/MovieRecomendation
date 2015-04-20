using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MovieServer.Setup
{
    class SetupMain
    {
        public static void main()
        {
            var movies = new List<Movie>();
            var lines = System.IO.File.ReadAllLines(@"movies.dat");
            foreach (var line in lines)
            {
                var spl = line.Split('\t');
                var title = spl[1].Substring(0,spl[1].Length-7);
                try
                {
                    Movie m = queryMovie(title);
                    if (m != null)
                    {
                        m.Id = Int32.Parse(spl[0]);
                        m.Popularity = Int32.Parse(spl[2]);
                        movies.Add(m);
                    }
                    Console.WriteLine(movies.Count());
                }
                catch { }
            }

            var ds = new DataStore();
            ds.Movies = movies;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("ImdbDataStore.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, ds);
            stream.Close();
        }

        public static Movie queryMovie(string title)
        {
            string url = "http://www.omdbapi.com/?t=" + title + "&y=&plot=short&r=json";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader resReader = new StreamReader(resStream);
            string sLine = resReader.ReadLine();

            return JsonConvert.DeserializeObject<Movie>(sLine);
        }
    }
}
