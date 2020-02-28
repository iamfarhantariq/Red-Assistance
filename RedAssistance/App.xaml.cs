using Newtonsoft.Json;
using RedAssistance.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;
using System.Linq;
using System.Net;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RedAssistance
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            //string data;
            //{
            //    data = EncryptDecryptClass.Crypt("ABC4116301+/");
            //}
            //{
            //    var result = EncryptDecryptClass.Decrypt(data);
            //}
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
