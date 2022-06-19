using Plugin.Media;
using PM2E15581.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace PM2E15581.Views
{
    public partial class PagePrincipal : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile FileFoto = null;
        public PagePrincipal()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            getLocation();
        }

        private async void btnlistasitios_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSitios());
        }

        async void imgsitiovisitado_Tapped(object sender, EventArgs args)
        {
            // await DisplayAlert("Hola", "mensaje xd si diste clic aqui pa", "OK");

            FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MisFotos",
                Name = "test.jpg",
                SaveToAlbum = true
            });

            // await DisplayAlert("Path directorio", FileFoto.Path, "OK");

            if (FileFoto != null)
            {
                imgsitiovisitado.Source = ImageSource.FromStream(() =>
                {
                    return FileFoto.GetStream();
                });
            }
        }

        #region Métodos
        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (System.IO.MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
            return null;
        }

        private void cleanLocation()
        {
            latitud.Text = null;
            longitud.Text = null;
        }

        public async void getLocation() {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    latitud.Text = ""+location.Latitude;
                    longitud.Text = "" + location.Longitude;
                    //await DisplayAlert("Aviso", "Si se leyó la ubicacion: "+location.Latitude +", "+location.Longitude, "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "El GPS está desactivado o no puede reconocerse", "OK");
                    cleanLocation();
                }
                
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Aviso", "Este dispositivo no soporta la versión de GPS utilizada", "OK");
                cleanLocation();
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                //await DisplayAlert("Aviso", "Handle not enabled on device exception: "+fneEx, "OK");
                await DisplayAlert("Aviso", "La ubicación está desactivada", "OK");
                cleanLocation();

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Aviso", "La aplicación no puede acceder a su ubicación.\n\n" +
                    "Habilite los permisos de ubicación en los ajustes del dispositivo", "OK");
                cleanLocation();
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Aviso", "No se ha podido obtener la localización (error de gps)", "OK");
                cleanLocation();
            }
        }
        #endregion

        private async void btnagregarsitio_Clicked(object sender, EventArgs e)
        {
            bool isCampoVacio = false;
            try
            {
                var _imgsitiovisitado = ConvertImageToByteArray();
                var _latitud = latitud.Text;
                var _longitud = longitud.Text;
                var _descripcion = descripcion.Text;

                if(_imgsitiovisitado == null || _latitud == null || _longitud == null || _descripcion == null)
                {
                    await DisplayAlert("Aviso", "Ha dejado un campo vacío, revise que haya tomado su foto y llenado los campos.", "OK");
                }
                else
                {
                    var sitio = new Sitios
                    {
                        imagen = _imgsitiovisitado,
                        latitud = _latitud,
                        longitud = _longitud,
                        descripcion = _descripcion
                    };

                    var result = await App.DBase.SitioSave(sitio);
                    if (result > 0)
                    {
                        await DisplayAlert("Aviso", "Sitio adicionado con éxito", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Ha ocurrido un error", "OK");
                    }

                    bool answer = await DisplayAlert("Aviso", "¿Desea vaciar el formulario?", "Si", "No");
                    //Debug.WriteLine("Answer: " + answer);
                    if (answer)
                    {
                        imgsitiovisitado.Source = null;
                        descripcion.Text = null;
                    }
                }
            }
            catch (Exception)
            {
                isCampoVacio = true;
            }
            finally
            {
                if (isCampoVacio)
                {
                    await DisplayAlert("Aviso", "Ha dejado un campo vacío, revise que haya tomado su foto y llenado los campos.", "OK");
                }
            }

            
        }


        private void btnsalirapp_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
