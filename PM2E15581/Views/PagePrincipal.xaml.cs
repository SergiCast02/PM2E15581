using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PM2E15581.Views
{
    public partial class PagePrincipal : ContentPage
    {
        public PagePrincipal()
        {
            InitializeComponent();
        }

        private async void btnlistasitios_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageSitios());
        }
    }
}
