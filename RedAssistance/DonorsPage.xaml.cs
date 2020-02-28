using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Messaging;
using Plugin.Share;
using Plugin.Share.Abstractions;
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
    public partial class DonorsPage : ContentPage
    {
        private readonly int _listviewtwo;
        private string _cell;
        private List<AddDonorClass> data;
        private string exportdata = "Donors \n";
        private string bloodgroup;

        public DonorsPage()
        {
            InitializeComponent();

        }

        public DonorsPage(List<LocalDonorsDB> data, string _cell)
        {
            InitializeComponent();
            this._cell = _cell;
            _listviewtwo = 1;
            LvDonors.ItemsSource = data;
        }

        public DonorsPage(string bloodgroup)
        {
            this.bloodgroup = bloodgroup;
            InitializeComponent();
            GetData(bloodgroup);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HandleDB dB = new HandleDB();
            if (_listviewtwo == 1)
            {
                LvDonors.ItemsSource = dB.GetDonorsByCell(_cell).ToList();
            }
            else
            {
                LvDonors.ItemsSource = data;
            }
        }

        private async void GetData(string bloodgroup)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
                ShowOfflineData(bloodgroup);
            }
            else
            {
                try
                {
                    WaitingLoader.IsRunning = true;
                    WaitingLoader.IsVisible = true;

                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi?city=Lahore&&blood=" + bloodgroup);
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
                            data = JsonConvert.DeserializeObject<List<AddDonorClass>>(result);
                            Title = data.Count().ToString() + " Donors Found";
                            LvDonors.ItemsSource = data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                    ShowOfflineData(bloodgroup);
                }
                finally
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                }
            }
        }

        private void ShowOfflineData(string bloodgroup)
        {
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetDonorsDB().Where(c => c.BloodGroup.Contains(bloodgroup)).ToList();
            LvDonors.ItemsSource = data;
        }

        private void LvDonorsTapped(object sender, ItemTappedEventArgs e)
        {
            if (_listviewtwo == 1)
            {
                var id = ((LocalDonorsDB)e.Item)._ID;
                var _id = ((LocalDonorsDB)e.Item).Id;
                var fullname = ((LocalDonorsDB)e.Item).CapName;
                var cellnumber = ((LocalDonorsDB)e.Item).CellNumber;
                var bloodgroup = ((LocalDonorsDB)e.Item).BloodGroup;
                var area = ((LocalDonorsDB)e.Item).Area;
                var city = ((LocalDonorsDB)e.Item).City;
                var todaydate = ((LocalDonorsDB)e.Item).TodayDate;
                var addedby = ((LocalDonorsDB)e.Item).AddedBy;

                Navigation.PushAsync(new AdminListviewTappedPage(_id, id, fullname, cellnumber, bloodgroup, area, city, todaydate, addedby));
            }
            else
            {
                try
                {
                    var fullname = ((AddDonorClass)e.Item).CapName;
                    var cellnumber = ((AddDonorClass)e.Item).CellNumber;
                    var bloodgroup = ((AddDonorClass)e.Item).BloodGroup;
                    var area = ((AddDonorClass)e.Item).Area;
                    var city = ((AddDonorClass)e.Item).City;
                    var todaydate = ((AddDonorClass)e.Item).TodayDate;
                    var addedby = ((AddDonorClass)e.Item).AddedBy;
                    int _val = 1;
                    Navigation.PushAsync(new ItemSelectedPage(_val, fullname, cellnumber, bloodgroup, area, city, todaydate, addedby));

                }
                catch (Exception ex)
                {
                    var msg = ex.ToString();
                    var fullname = ((LocalDonorsDB)e.Item).CapName;
                    var cellnumber = ((LocalDonorsDB)e.Item).CellNumber;
                    var bloodgroup = ((LocalDonorsDB)e.Item).BloodGroup;
                    var area = ((LocalDonorsDB)e.Item).Area;
                    var city = ((LocalDonorsDB)e.Item).City;
                    var todaydate = ((LocalDonorsDB)e.Item).TodayDate;
                    var addedby = ((LocalDonorsDB)e.Item).AddedBy;
                    int _val = 1;
                    Navigation.PushAsync(new ItemSelectedPage(_val, fullname, cellnumber, bloodgroup, area, city, todaydate, addedby));
                }
            }
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

        private async void ExportClicked(object sender, EventArgs e)
        {
            if (_listviewtwo == 1)
            {
                DependencyService.Get<IMessage>().LongAlert("Cannot Export");
            }
            else
            {
                HandleDB userhandle = new HandleDB();
                var localDBs = userhandle.GetUserInfoDB().ToList();
                if (localDBs.Count() != 0) //data is available
                {
                    int i = 0;
                    foreach (var v in data)
                    {
                        string row = "(" + data[i].BloodGroup + ") " + data[i].CapName + " " + data[i].CellNumber;
                        row = row.Insert(row.Length, "\n");
                        exportdata = exportdata.Insert(exportdata.Length, row);
                        i++;
                    };

                    await CrossShare.Current.Share(new ShareMessage
                    {
                        Text = exportdata,
                        Title = "Red Assistance",
                        Url = "https://bit.ly/2JJnyWW"
                    });
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert("Login first");
                }
            }
        }
    }
}