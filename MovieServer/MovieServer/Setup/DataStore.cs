using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
