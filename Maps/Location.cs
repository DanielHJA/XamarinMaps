using System;
using Xamarin.Forms.Maps;

namespace Maps
{
    public class Location
    {

        public Position Position { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public Location()
        {
        }
    }
}
