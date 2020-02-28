using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Messaging;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        private readonly int _listviewtwo;
        private string cell;
        private List<LocalRequestsDB> data;

        public RequestsPage()
        {
            InitializeComponent();
            GetData();
        }

        public RequestsPage(List<LocalRequestsDB> data, string cell)
        {
            this.cell = cell;
            this.data = data;
            InitializeComponent();
            _listviewtwo = 1;
            LvRequests.ItemsSource = data;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HandleDB dB = new HandleDB();
            if (_listviewtwo == 1)
            {
                LvRequests.ItemsSource = dB.GetRequestByCell(cell).ToList();
            }
            else
            {
                LvRequests.ItemsSource = data;
            }
        }

        private async void GetData()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
                ShowOfflineData();
            }
            else
            {
                try
                {
                    WaitingLoader.IsRunning = true;
                    WaitingLoader.IsVisible = true;

                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        if (result == "[]")
                        {
                            DependencyService.Get<IMessage>().LongAlert("No Data Found");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            var name = JsonConvert.DeserializeObject<List<AddRequestClass>>(result);
                            Title = name.Count().ToString() + " Blood Requests Found";
                            LvRequests.ItemsSource = name;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                    ShowOfflineData();
                }
                finally
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                }
            }
        }

        private void ShowOfflineData()
        {
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetDonorsDB().ToList();
            LvRequests.ItemsSource = data;
        }

        private void CallButtonClicked(object sender, EventArgs e)
        {
            Image imgClicked = (Image)sender;
            var item = (TapGestureRecognizer)imgClicked.GestureRecognizers[0];
            var cell = item.CommandParameter;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(cell.ToString());
        }

        private void MessageButtonClicked(object sender, EventArgs e)
        {
            Image imgClicked = (Image)sender;
            var item = (TapGestureRecognizer)imgClicked.GestureRecognizers[0];
            var cell = item.CommandParameter;
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(cell.ToString(), "\n\n-Red Assistance");
        }

        private void LvRequestsTapped(object sender, ItemTappedEventArgs e)
        {
            if (_listviewtwo == 1)
            {
                var id = ((LocalRequestsDB)e.Item)._ID;
                var _id = ((LocalRequestsDB)e.Item).Id;
                var fullname = ((LocalRequestsDB)e.Item).CapName;
                var cellnumber = ((LocalRequestsDB)e.Item).CellNumber;
                var bloodgroup = ((LocalRequestsDB)e.Item).BloodGroup;
                var hospital = ((LocalRequestsDB)e.Item).Hospitals;
                var city = ((LocalRequestsDB)e.Item).City;
                var todaydate = ((LocalRequestsDB)e.Item).TodayDate;
                var addedby = ((LocalRequestsDB)e.Item).AddedBy;
                Navigation.PushAsync(new AdminListviewTappedPage(_id, 1, id, fullname, cellnumber, bloodgroup, hospital, city, todaydate, addedby));

            }
            else
            {
                try
                {
                    var fullname = ((AddRequestClass)e.Item).CapName;
                    var cellnumber = ((AddRequestClass)e.Item).CellNumber;
                    var bloodgroup = ((AddRequestClass)e.Item).BloodGroup;
                    var hospital = ((AddRequestClass)e.Item).Hospitals;
                    var city = ((AddRequestClass)e.Item).City;
                    var todaydate = ((AddRequestClass)e.Item).TodayDate;
                    var addedby = ((AddRequestClass)e.Item).AddedBy;
                    int _val = 2;
                    Navigation.PushAsync(new ItemSelectedPage(_val, fullname, cellnumber, bloodgroup, hospital, city, todaydate, addedby));
                }
                catch (Exception ex)
                {
                    var msg = ex.ToString();
                    var fullname = ((LocalRequestsDB)e.Item).CapName;
                    var cellnumber = ((LocalRequestsDB)e.Item).CellNumber;
                    var bloodgroup = ((LocalRequestsDB)e.Item).BloodGroup;
                    var hospital = ((LocalRequestsDB)e.Item).Hospitals;
                    var city = ((LocalRequestsDB)e.Item).City;
                    var todaydate = ((LocalRequestsDB)e.Item).TodayDate;
                    var addedby = ((LocalRequestsDB)e.Item).AddedBy;
                    int _val = 2;
                    Navigation.PushAsync(new ItemSelectedPage(_val, fullname, cellnumber, bloodgroup, hospital, city, todaydate, addedby));
                }
            }
        }
    }
}