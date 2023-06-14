using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;

namespace App2P2023.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMap : ContentPage
    {
        public PageMap()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var conectividad = Connectivity.NetworkAccess;
            var locl = CrossGeolocator.Current;

            if (conectividad == NetworkAccess.Internet)
            {
                if (locl != null)
                {
                    locl.PositionChanged += Locl_PositionChanged;

                    if (!locl.IsListening)
                    {
                        await locl.StartListeningAsync(TimeSpan.FromSeconds(10), 100);
                    }

                    var posicion = await locl.GetPositionAsync();
                    var mapcenter = new Xamarin.Forms.Maps.Position(posicion.Latitude, posicion.Longitude);
                    mapa.MoveToRegion(new MapSpan(mapcenter, 1, 1));
                }
            }
            else 
            {
                var posicion = await locl.GetPositionAsync();
                var mapcenter = new Xamarin.Forms.Maps.Position(posicion.Latitude, posicion.Longitude);
                mapa.MoveToRegion(new MapSpan(mapcenter, 1, 1));
            }

            /*
            var location = await Geolocation.GetLocationAsync();
            if (location != null )
            {
                var pin = new Pin()
                {
                    Position = new Position(location.Latitude, location.Longitude), Label = "Ubicacion actual"
                };
                mapa.Pins.Add(pin);
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(100)));             
            }
            */
        }

        private void Locl_PositionChanged(object sender, PositionEventArgs e)
        {
            var mapcenter = new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude);
            mapa.MoveToRegion(new MapSpan(mapcenter, 1, 1));
        }
    }
}