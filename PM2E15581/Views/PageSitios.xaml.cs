using PM2E15581.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E15581.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSitios : ContentPage
    {
        ItemTappedEventArgs elemento = null;
        public PageSitios()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ListaSitios.ItemsSource = await App.DBase.obtenerListaSitios();
            elemento = null;
        }

        private void ListaSitios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            elemento = e;
        }

        private async void btneliminarcasa_Clicked(object sender, EventArgs e)
        {
            try
            {
                var sitio = (Sitios)elemento.Item;
                //await DisplayAlert("Aviso","Elemento Seleccionado " + sitio.descripcion, "OK");

                bool answer = await DisplayAlert("Aviso", "Está a punto de eliminar el siguiente sitio: \n\n" + sitio.descripcion, "CONFIRMAR", "Volver");
                //Debug.WriteLine("Answer: " + answer);
                if (answer)
                {
                    var result = await App.DBase.SitioDelete(sitio);

                    if (result > 0)
                    {
                        await DisplayAlert("Aviso", "Sitio eliminado con éxito", "OK");
                        ListaSitios.ItemsSource = await App.DBase.obtenerListaSitios(); // Se vuelve a llenar la lista una vez borrado el sitio

                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Ha ocurrido un error", "OK");
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Aviso", "Escoja un elemento", "OK");
            }

        }

        private async void btnvermapa_Clicked(object sender, EventArgs e)
        {
            try
            {
                var sitio = (Sitios)elemento.Item;
                await Navigation.PushAsync(new PageMapa(Double.Parse(sitio.latitud), Double.Parse(sitio.longitud), sitio.descripcion)); // Abre el Page de Mapa
            }
            catch (Exception)
            {
                await DisplayAlert("Aviso", "Escoja un elemento", "OK");
            }
            
        }
    }
}