using Newtonsoft.Json;
using Plugin.Connectivity;
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
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Application.Current.Properties.ContainsKey("DonorsCount") || Application.Current.Properties.ContainsKey("RequestsCount"))
                {
                    LblDonorCount.Text = Application.Current.Properties["DonorsCount"].ToString();
                    LblPatientsCount.Text = Application.Current.Properties["RequestsCount"].ToString();
                }
                else
                {
                    try
                    {
                        var httpClientDonors = new HttpClient();
                        var httpClientRequests = new HttpClient();
                        var httpClientTeam = new HttpClient();
                        var responseD = await httpClientDonors.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi");
                        var responseR = await httpClientDonors.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi");
                        // var responseT = await httpClientTeam.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi");
                        var valuesD = JsonConvert.DeserializeObject<List<AddDonorClass>>(responseD);
                        var valuesR = JsonConvert.DeserializeObject<List<AddRequestClass>>(responseR);
                        // var valuesT = JsonConvert.DeserializeObject<List<AdminClass>>(responseT);

                        HandleDB handleDB = new HandleDB();

                        for (int i = 0; i < valuesR.Count(); i++)
                        {
                            LocalRequestsDB requestsDB = new LocalRequestsDB()
                            {
                                _ID = valuesR[i].ID,
                                FullName = valuesR[i].FullName,
                                CellNumber = valuesR[i].CellNumber,
                                City = valuesR[i].City,
                                Hospitals = valuesR[i].Hospitals,
                                BloodGroup = valuesR[i].BloodGroup,
                                TodayDate = valuesR[i].TodayDate,
                                AddedBy = valuesR[i].AddedBy,
                                FutureUse = valuesR[i].FutureUse
                            };
                            handleDB.AddRequestsDB(requestsDB);
                        }
                        for (int i = 0; i < valuesD.Count(); i++)
                        {
                            LocalDonorsDB donorsDB = new LocalDonorsDB()
                            {
                                _ID = valuesD[i].ID,
                                FullName = valuesD[i].FullName,
                                CellNumber = valuesD[i].CellNumber,
                                City = valuesD[i].City,
                                Area = valuesD[i].Area,
                                BloodGroup = valuesD[i].BloodGroup,
                                TodayDate = valuesD[i].TodayDate,
                                AddedBy = valuesD[i].AddedBy,
                                FutureUse = valuesD[i].FutureUse
                            };
                            handleDB.AddDonorsDB(donorsDB);
                        }

                        //for (int i = 0; i < valuesT.Count(); i++)
                        //{
                        //    LocalAdminDB adminDB = new LocalAdminDB()
                        //    {
                        //        _Id = valuesT[i].Id,
                        //        UserName = valuesT[i].UserName,
                        //        CellNumber = valuesT[i].CellNumber,
                        //        Role = valuesT[i].Role,
                        //        From = valuesT[i].From,
                        //        Password = valuesT[i].Password,
                        //        FutureUse = valuesT[i].FutureUse
                        //    };
                        //    handleDB.AddAdminDB(adminDB);
                        //}

                        Application.Current.Properties["DonorsCount"] = LblDonorCount.Text = valuesD.Count().ToString();
                        Application.Current.Properties["RequestsCount"] = LblPatientsCount.Text = valuesR.Count().ToString();
                        await Application.Current.SavePropertiesAsync();
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                    }
                }
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("DonorsCount") || Application.Current.Properties.ContainsKey("RequestsCount"))
                {
                    LblDonorCount.Text = Application.Current.Properties["DonorsCount"].ToString();
                    LblPatientsCount.Text = Application.Current.Properties["RequestsCount"].ToString();
                }
            }

            HandleDB userhandle = new HandleDB();
            var data = userhandle.GetUserInfoDB().ToList();
            if (data.Count() != 0) //data is available
            {
                UserNote.IsVisible = true;
                GuestNote.IsVisible = false;

                SpanUserName.Text = Application.Current.Properties["UserName"].ToString();
                SpanUserDonors.Text = Application.Current.Properties["UserDonors"].ToString();
                SpanUserRequests.Text = Application.Current.Properties["UserRequests"].ToString();
            }
            else
            {
                GuestNote.IsVisible = true;
                UserNote.IsVisible = false;
            }
        }

        private void ContactusClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TeamPage());
        }
        private async void FindDonorsClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Select Blood Group", "Cancel", null, "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-");
            switch (action)
            {
                case "Cancel":
                    break;

                case "A+":
                    DonorsDataGet("A%2B");
                    break;

                case "A-":
                    DonorsDataGet("A%2D");
                    break;

                case "B+":
                    DonorsDataGet("B%2B");
                    break;

                case "B-":
                    DonorsDataGet("B%2D");
                    break;

                case "AB+":
                    DonorsDataGet("AB%2B");
                    break;

                case "AB-":
                    DonorsDataGet("AB%2D");
                    break;

                case "O+":
                    DonorsDataGet("O%2B");
                    break;

                case "O-":
                    DonorsDataGet("O%2D");
                    break;
            }
        }

        private void DonorsDataGet(string v)
        {
            Navigation.PushAsync(new DonorsPage(v));
        }
        private void FindPatientsClicked_Event(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RequestsPage());
        }
        private void BloodBankClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BloodBanks());
        }

        private void TeamImgClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TeamPage());
        }
        private void DetailClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExtraDetailsPage(1));
        }
        private void TermsConditionClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExtraDetailsPage(2));
        }
        private void EligibiltyClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExtraDetailsPage(3));
        }
        private async void ShareImgClicked(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = "Find Blood Donors, Add Blood Requests for free and Donate Blood. \n" +
                            "Download App Now \n\n" +
                            "Red Assistance:",
                Title = "Share Mobile app",
                Url = "https://play.google.com/store/apps/details?id=com.BloodDonation.BloodDonation"
            });
        }

        private async void BtnPlusClicked(object sender, EventArgs e)
        {
            HandleDB userhandle = new HandleDB();
            var data = userhandle.GetUserInfoDB().ToList();
            if (data.Count() != 0) //data is available
            {
                var action = await DisplayActionSheet("Choose to add: ", "Cancel", null, "Add Donor", "Add Blood Request");
                switch (action)
                {
                    case "Cancel":
                        break;

                    case "Add Donor":
                        await Navigation.PushAsync(new AddDonorPage());
                        break;

                    case "Add Blood Request":
                        await Navigation.PushAsync(new AddRequestPage());
                        break;
                }
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("Login first");
            }
        }
        private void RefreshClicked(object sender, EventArgs e)
        {
            new RefreshClass(1);
        }
        private void AdminConsolClicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("AdminCellNumber") || Application.Current.Properties.ContainsKey("AdminPassword"))
            {
                Navigation.PushAsync(new AdminDashboardPage());
            }
            else
            {
                Navigation.PushAsync(new AdminLoginPage());
            }
        }

        private void BtnRefreshClicked(object sender, EventArgs e)
        {
            new RefreshClass(1);
        }
    }
}