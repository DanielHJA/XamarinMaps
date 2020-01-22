using System;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace Maps
{
    public class MainPageViewModel: BindableObject
    {
        Map map;
        Plugin.Geolocator.Abstractions.Position userPosition = new Plugin.Geolocator.Abstractions.Position();

        private ObservableCollection<Location> _Locations = new ObservableCollection<Location>();
        public ObservableCollection<Location> Locations
        {
            get
            {
                return _Locations;
            }
            set
            {
                if (value != _Locations)
                {
                    _Locations = value;
                    OnPropertyChanged();
                }
            }
        }

        private int currentMapTypeIndex;
        private MapType _CurrentMapType { get; set; }
        public MapType CurrentMapType
        {
            get
            {
                return _CurrentMapType;
            }
            set
            {
                if (value != _CurrentMapType)
                {
                    _CurrentMapType = value;
                    OnPropertyChanged();
                }
            }
        }

        readonly private List<MapType> mapTypes = new List<MapType>(new MapType[] { MapType.Hybrid, MapType.Satellite, MapType.Street });

        public MainPageViewModel(Map map)
        {
            this.map = map;
            currentMapTypeIndex = 0;
            CurrentMapType = mapTypes[currentMapTypeIndex];
            _ = StartListening();
            ConfigureLocations();
            addPolygon();
            addPolyLine();
        }

        private void addPolygon() {
            Polygon polygon = new Polygon
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#1BA1E2"),
                FillColor = Color.FromHex("#881BA1E2"),
                Geopath =
                        {
                            new Xamarin.Forms.Maps.Position(58.76022203314249, 16.973271758053183),
                            new Xamarin.Forms.Maps.Position(58.76841156568028, 17.03506985375631),
                            new Xamarin.Forms.Maps.Position(58.7453513190088, 17.05996075341451),
                            new Xamarin.Forms.Maps.Position(58.73712294427616, 17.053459078762412),
                            new Xamarin.Forms.Maps.Position(58.74457198943936, 17.029104620907187),
                            new Xamarin.Forms.Maps.Position(58.74129861452169, 16.998935133907676)
     
                        }
            };

            // add the polygon to the map's MapElements collection
            map.MapElements.Add(polygon);
        }

        private void addPolyLine() {
            Polyline polyline = new Polyline
            {
                StrokeColor = Color.Orange,
                StrokeWidth = 5,
                Geopath =
                    {
                    new Xamarin.Forms.Maps.Position(58.751996,17.006982),
                    new Xamarin.Forms.Maps.Position(58.752446,17.006564),
                    new Xamarin.Forms.Maps.Position(58.752131,17.005252),
                    new Xamarin.Forms.Maps.Position(58.759823,16.998335)
        }
    };


            map.MapElements.Add(polyline);
}

private void ConfigureLocations() {
            Location Location1 = new Location();
            Location1.Position = new Xamarin.Forms.Maps.Position(58.753158, 17.002392);
            Location1.Address = "Locationsvägen 10, 51122, Nyköping";
            Location1.Description = "Location 1";

            Location Location2 = new Location();
            Location2.Position = new Xamarin.Forms.Maps.Position(58.74631443881254, 17.012184120110856);
            Location2.Address = "Locationsvägen 20, 51122, Nyköping";
            Location2.Description = "Location 2";

            Location Location3 = new Location();
            Location3.Position = new Xamarin.Forms.Maps.Position(58.74339490122441, 16.99936701669513);
            Location3.Address = "Locationsvägen 30, 51122, Nyköping";
            Location3.Description = "Location 3";

            Locations.Add(Location1);
            Locations.Add(Location2);
            Locations.Add(Location3);
        }

        private async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {

            //If updating the UI, ensure you invoke on main thread
            var position = new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude);
            setMapRegion(position);
            _ = StopListening();
            reverseGeocode(position);
        }

        private async void reverseGeocode(Xamarin.Forms.Maps.Position position) {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();
            System.Console.WriteLine(address);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            System.Console.WriteLine(e.Error);
        }

        private async Task StopListening()
        {
            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }

        public void setMapRegion(Xamarin.Forms.Maps.Position position) {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(2)));
        }

        public void ChangeMapType() {
            if (currentMapTypeIndex >= mapTypes.Count - 1)
            {
                currentMapTypeIndex = 0;
            }
            else
            {
                currentMapTypeIndex += 1;
            }

            CurrentMapType = mapTypes[currentMapTypeIndex];
        }
    }
}
