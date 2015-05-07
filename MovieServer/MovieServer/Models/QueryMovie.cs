using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieServer.Models
{
    class QueryMovie : IComparable<QueryMovie>
    {
        public double distance;
        public Movie movie;

        public QueryMovie(List<double> query, List<double> weights, Movie mov)
        {
            movie = mov;
            distance = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                distance += ((query[i] - mov.Features[i]) * weights[i] * weights.Count) * ((query[i] - mov.Features[i]) * weights[i] * weights.Count);
            }
        }

        public int CompareTo(QueryMovie other)
        {
            if (this.distance < other.distance)
                return -1;
            if (this.distance > other.distance)
                return 1;
            return 0;
        }
    }
}
