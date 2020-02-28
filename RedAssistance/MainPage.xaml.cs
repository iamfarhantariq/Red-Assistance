using Newtonsoft.Json;
using Plugin.Connectivity;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RedAssistance
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new HomePage())
            { BarBackgroundColor = Color.FromHex("#d21d1d"), BarTextColor = Color.White };
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HandleDB userhandle = new HandleDB();
            var data = userhandle.GetUserInfoDB().ToList();
            if (data.Count() != 0) //data is available
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    LvUserInfo.ItemsSource = data;
                    LoginLayoutScrlvw.IsVisible = false;
                    UserDetailLayoutScrlvw.IsVisible = true;
                }
                else
                {
                    try
                    {
                        var cell = data[0].CellNumber;
                        var password = data[0].Password;

                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi?cellnumber={0}&&password={1}",
                            cell,
                            password));

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            if (result == "[]")
                            {
                                // Application.Current.Properties["Logged"] = false;
                                HandleDB logouthandle = new HandleDB();
                                LocalDB _localdb = new LocalDB();
                                logouthandle.DeleteUserDB(_localdb);

                                Application.Current.Properties.Remove("UserName");
                                Application.Current.Properties.Remove("UserCell");
                                Application.Current.Properties.Remove("UserDonors");
                                Application.Current.Properties.Remove("UserRequests");
                                await Application.Current.SavePropertiesAsync();

                                UserDetailLayoutScrlvw.IsVisible = false;
                                LoginLayoutScrlvw.IsVisible = true;
                                await Navigation.PushAsync(new SigninPage());
                                DependencyService.Get<IMessage>().LongAlert("Kindly Login again!");
                            }
                            else
                            {
                                var values = JsonConvert.DeserializeObject<List<SignupClass>>(result);
                                Application.Current.Properties["UserCell"] = values[0].CellNumber;
                                Application.Current.Properties["UserName"] = values[0].FullName;
                                Application.Current.Properties["Logged"] = true;
                                await Application.Current.SavePropertiesAsync();

                                LvUserInfo.ItemsSource = values;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                        LoginLayoutScrlvw.IsVisible = true;
                    }
                }

                LoginLayoutScrlvw.IsVisible = false;
                UserDetailLayoutScrlvw.IsVisible = true;
            }
            else
            {
                LoginLayoutScrlvw.IsVisible = true;
                UserDetailLayoutScrlvw.IsVisible = false;
            }
        }

        private void RegisterBtn_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterUser());
            IsPresented = false;
        }
        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SigninPage());
            IsPresented = false;
        }
        private async void LogoutBtnClicked(object sender, EventArgs e)
        {
            //Application.Current.Properties["Logged"] = false;
            HandleDB logouthandle = new HandleDB();
            LocalDB _localdb = new LocalDB();
            logouthandle.DeleteUserDB(_localdb);

            Application.Current.Properties["UserName"] = null;
            Application.Current.Properties["UserCell"] = null;
            Application.Current.Properties["UserDonors"] = null;
            Application.Current.Properties["UserRequests"] = null;
            await Application.Current.SavePropertiesAsync();

            UserDetailLayoutScrlvw.IsVisible = false;
            LoginLayoutScrlvw.IsVisible = true;
            await Navigation.PushAsync(new SigninPage());
            IsPresented = false;
        }

        private void DonorBtnClicked(object sender, EventArgs e)
        {
            string cell = (string)Application.Current.Properties["UserCell"];
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetDonorsByCell(cell).ToList();
            Navigation.PushAsync(new DonorsPage(data, cell));
            IsPresented = false;
        }

        private void RequestBtnClicked(object sender, EventArgs e)
        {
            string cell = (string)Application.Current.Properties["UserCell"];
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetRequestByCell(cell).ToList();
            Navigation.PushAsync(new RequestsPage(data, cell));
            IsPresented = false;
        }

        private void ChangePasswordClicked(object sender, EventArgs e)
        {
            HandleDB dB = new HandleDB();
            Navigation.PushAsync(new ChangePasswordPage(dB.GetUserInfoDB().ToList()));
            IsPresented = false;
        }

        private void BtnEditLabel_Tapped(object sender, EventArgs e)
        {
            HandleDB dB = new HandleDB();
            Navigation.PushAsync(new RegisterUser(dB.GetUserInfoDB().ToList()));
        }
    }
}
