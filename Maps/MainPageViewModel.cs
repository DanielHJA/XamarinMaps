﻿using System;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace Maps
{
    public class MainPageViewModel: BindableObject, INotifyPropertyChanged
    {

        Map map;
        Plugin.Geolocator.Abstractions.Position userPosition = new Plugin.Geolocator.Abstractions.Position();
        public ObservableCollection<CarouselItem> CarouselItems { get; private set; }
        public MapType CurrentMapType { get; private set; }
        public ObservableCollection<Location> Locations { get; private set; }
        private int currentMapTypeIndex;
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
            ConfigureCarouselItems();
        }

        private void ConfigureCarouselItems() {
            ObservableCollection<CarouselItem> Items = new ObservableCollection<CarouselItem>(new CarouselItem[] {
                new CarouselItem("Monkey", "https://ichef.bbci.co.uk/news/410/cpsprodpb/E9DF/production/_96317895_gettyimages-164067218.jpg"),
                new CarouselItem("Fox", "https://www.thatsfarming.com/uploads/news/resizeExact_1200_800/11685-fox-3053706-1280.jpg"),
                new CarouselItem("Koala", "https://upload.wikimedia.org/wikipedia/commons/4/49/Koala_climbing_tree.jpg"),
                new CarouselItem("Seal", "https://s.abcnews.com/images/International/grey-seal-stock-gty-jef-190621_hpMain_16x9_992.jpg")
             });
            CarouselItems = Items;
            OnPropertyChanged("CarouselItems");
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

                    Locations = new ObservableCollection<Location>() { Location1, Location2, Location3 };

                    OnPropertyChanged("Locations");
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
            OnPropertyChanged("CurrentMapType");
        }

        public void CarouselItemDidChange(int Position)
        {
    
        }
    }
}
