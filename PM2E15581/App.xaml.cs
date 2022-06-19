using PM2E15581.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E15581
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new PagePrincipal();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
