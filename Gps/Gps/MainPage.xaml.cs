using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;

namespace Gps
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Latitud.Text = "";
            Longitud.Text = "";
            inp1.Text = "";
            inp2.Text = "";
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                GestorGPS obj = new GestorGPS();
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                TimeSpan ts = TimeSpan.FromTicks(10000);
                var position = await locator.GetPositionAsync(ts);
                double latitud = position.Latitude;
                double longitud = position.Longitude;
                Latitud.Text = position.Latitude.ToString();
                Longitud.Text = position.Longitude.ToString();
                obj.mandarCoordenadas(latitud, longitud);
            }
            catch
            {
               
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if ((inp1.ToString() != "")&&(inp2.ToString() != "")) {
                GestorGPS obj = new GestorGPS();
                decimal x, y;
                x = Convert.ToDecimal(inp1.Text);
                y = Convert.ToDecimal(inp2.Text);
                labRes.Text = obj.consultarDistancia(x, y).ToString();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}
