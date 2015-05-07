using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;
using MovieServer.Models;

namespace MovieServer.Setup
{
    [Serializable]
    class DataStore
    {
        public List<string> FeatureNames;
        public List<Movie> Movies;

        public void GenerateRandomFeatures(int nFeatures)
        {
            FeatureNames = new List<string>();
            for (int i = 1; i <= nFeatures; i++)
            {
                FeatureNames.Add("Feature" + i);
            }

            Random random = new Random();

            foreach (var movie in Movies)
            {
                movie.Features = new List<double>();
                for (int i = 0; i < nFeatures; i++)
                {
                    movie.Features.Add(random.NextDouble());
                }
            }
        }

        public List<Movie> GetKnn(int k, List<double> features, List<double> weights)
        {
            var heap = new OrderedSet<QueryMovie>();
            var querymovies = new List<QueryMovie>();

            foreach (var movie in Movies)
            {
                querymovies.Add(new QueryMovie(features, weights, movie));
            }

            querymovies.Sort();

            var res = new List<Movie>();
            foreach (var movie in querymovies.Take(k))
            {
                res.Add(movie.movie);
            }

            return res;
        }
    }
}
