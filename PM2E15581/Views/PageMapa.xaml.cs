using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using ImageFromXamarinUI;

namespace PM2E15581.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMapa : ContentPage
    {
        double _latitud, _longitud;
        String _descripcion;
        public PageMapa(double latitud, double longitud, String descripcion)
        {
            InitializeComponent();
            _latitud = latitud;
            _longitud = longitud;
            _descripcion = descripcion;
        }

        private async void btncompartirimgubicacion_Clicked(object sender, EventArgs e)
        {
            try
            {
                var screenshot = await Screenshot.CaptureAsync();
                //var stream = await screenshot.OpenReadAsync();

                var stream = await mapa.CaptureImageAsync();

                //var Image = ImageSource.FromStream(() => stream);

                //Aquí debería guardar la imagen
                //var dest = System.IO.File.OpenWrite(path);
                //await stream.CopyToAsync(dest);

                /*new Plugin.Media.Abstractions.MediaFile
                {
                    Directory = "MisFotos",
                    Name = "test.jpg",
                    path = "/"
                };*/

                /*var image = await MediaPicker.PickPhotoAsync();
                if (image == null)
                {
                    return;
                }*/
                byte[] screenshotarray;
                using (System.IO.MemoryStream memory = new MemoryStream())
                {
                    stream.CopyTo(memory);
                    screenshotarray = memory.ToArray();
                }

                var tempFilename = "Test.png";
                String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), tempFilename);
                File.WriteAllBytes(filePath, screenshotarray);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Mi Ubicacion",
                    File = new ShareFile(filePath)
                });
            }
            catch
            {

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //await DisplayAlert("Aviso", "Si se leyó la ubicacion: "+location.Latitude +", "+location.Longitude, "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "El GPS está desactivado o no puede reconocerse", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Aviso", "Este dispositivo no soporta la versión de GPS utilizada", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                //await DisplayAlert("Aviso", "Handle not enabled on device exception: "+fneEx, "OK");
                await DisplayAlert("Aviso", "La ubicación está desactivada", "OK");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Aviso", "La aplicación no puede acceder a su ubicación.\n\n" +
                    "Habilite los permisos de ubicación en los ajustes del dispositivo", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Aviso", "No se ha podido obtener la localización (error de gps)", "OK");
            }

            Pin pin = new Pin();
            pin.Label = _descripcion;
            pin.Type = PinType.Place;
            pin.Position = new Position(_latitud, _longitud);
            mapa.Pins.Add(pin);
            mapa.IsShowingUser = true; //muestra la ubicacion del usuario en donde se encuentra
            mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(_latitud, _longitud), Distance.FromMeters(500.0)));





        }
    }
}