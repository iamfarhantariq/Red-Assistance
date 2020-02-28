using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Messaging;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BloodBanks : ContentPage
    {
        public BloodBanks()
        {
            InitializeComponent();
            GetBloodBanks();
        }

        private async void GetBloodBanks()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("No internet");
            }
            else
            {
                try
                {
                    WaitingLoader.IsRunning = true;
                    WaitingLoader.IsVisible = true;

                    var httpClient = new System.Net.Http.HttpClient();
                    var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/BloodBanksApi");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        if (result == "[]")
                        {
                            DependencyService.Get<IMessage>().LongAlert("No record found");
                            await Navigation.PopAsync(SendBackButtonPressed());
                        }
                        else
                        {
                            var name = JsonConvert.DeserializeObject<List<BloodBankClass>>(result);
                            LvBloodBanks.ItemsSource = name;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                    DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                    await Navigation.PopAsync();
                }
                finally
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                }
            }

        }

        private void LvBloodBanks_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            string phonenumber = ((BloodBankClass)LvBloodBanks.SelectedItem).PhoneNumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }
    }
}