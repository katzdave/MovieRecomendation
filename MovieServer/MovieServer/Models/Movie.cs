using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieServer
{
    [Serializable]
    class Movie
    {
        public string Title;
        public string Year;
        public string Poster;
        public string Plot;
        public string Released;
        public string imdbID;
        public string Genre;
        public string Director;

        public int Id;
        public int Popularity;
        public List<double> Features;
    }
}
