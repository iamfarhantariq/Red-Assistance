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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminDashboardPage : ContentPage
    {
        HandleDB dB = new HandleDB();
        private List<LocalUsersDB> UserData;
        private List<LocalRequestsDB> RequestData;
        private List<LocalDonorsDB> DonorData;
        private List<LocalAdminDB> TeamData;
        private string searchby;
        private string role;
        private int teamscount;
        private string exportdata = string.Empty;

        public AdminDashboardPage()
        {
            InitializeComponent();
            LblTeam.TextColor = Color.FromHex("#d21d1d");
            SLblTeam.BackgroundColor = Color.White;
        }
        protected override void OnAppearing()
        {
            role = (string)Application.Current.Properties["AdminRole"];
            CheckUser();
            GetAllData();
            GetData();
        }
        private async void CheckUser()
        {
            if (Application.Current.Properties.ContainsKey("AdminCellNumber")
                || Application.Current.Properties.ContainsKey("AdminPassword")) //data is available
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        string cell = (string)Application.Current.Properties["AdminCellNumber"];
                        var password = (string)Application.Current.Properties["AdminPassword"];

                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsole?cellnumber={0}&&password={1}",
                            cell,
                            password));

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            if (result == "[]")
                            {
                                HandleDB handleDB = new HandleDB();
                                LocalAdminDB localAdminDB = new LocalAdminDB();
                                LocalUsersDB localUsers = new LocalUsersDB();
                                handleDB.DeleteAdminDB(localAdminDB);
                                handleDB.DeleteAllUserDB(localUsers);
                                Application.Current.Properties.Remove("HasAdminAndUsersData");
                                Application.Current.Properties.Remove("AdminCellNumber");
                                Application.Current.Properties.Remove("AdminPassword");
                                Application.Current.Properties.Remove("AdminRole");
                                await Application.Current.SavePropertiesAsync();
                                await Navigation.PopAsync();
                                DependencyService.Get<IMessage>().LongAlert("Kindly Login again!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                    }
                }

            }
        }
        private void GetAllData()
        {
            TeamShow();
            DonorShow();
            RequestShow();
            UserShow();
        }
        private void UserShow()
        {
            UserData = dB.GetAllUserInfoDB().ToList();
            LvUsers.ItemsSource = UserData;
        }
        private void RequestShow()
        {
            RequestData = dB.GetRequestsDB().ToList();
            LvRequests.ItemsSource = RequestData;
        }
        private void DonorShow()
        {
            DonorData = dB.GetDonorsDB().ToList();
            LvDonors.ItemsSource = DonorData;
        }
        private void TeamShow()
        {
            TeamData = dB.GetAdminDB().ToList();
            LvTeam.ItemsSource = TeamData;
        }
        private async void GetData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Application.Current.Properties.ContainsKey("HasAdminAndUsersData"))
                {
                    GetAllData();
                    LayoutAdmin.IsVisible = true;
                }
                else
                {
                    try
                    {
                        LayoutAdmin.IsVisible = false;
                        WaitingLoader.IsRunning = true;
                        WaitingLoader.IsVisible = true;

                        var httpClientUsers = new HttpClient();
                        var httpClientTeam = new HttpClient();
                        var responseU = await httpClientUsers.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi");
                        var responseT = await httpClientTeam.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi");
                        var valuesU = JsonConvert.DeserializeObject<List<SignupClass>>(responseU);
                        var valuesT = JsonConvert.DeserializeObject<List<AdminClass>>(responseT);

                        HandleDB handleDB = new HandleDB();

                        for (int i = 0; i < valuesU.Count(); i++)
                        {
                            LocalUsersDB usersDB = new LocalUsersDB()
                            {
                                _ID = valuesU[i].Id,
                                FullName = valuesU[i].FullName,
                                CellNumber = valuesU[i].CellNumber,
                                City = valuesU[i].City,
                                Area = valuesU[i].Area,
                                BloodGroup = valuesU[i].BloodGroup,
                                Email = valuesU[i].Email,
                                Password = valuesU[i].Password,
                                TodayDate = valuesU[i].TodayDate,
                                FutureUse = valuesU[i].FutureUse
                            };
                            handleDB.AddAllUsersDB(usersDB);
                        }

                        //   teamscount = Convert.ToInt32(Application.Current.Properties["TeamsCount"]);

                        for (int i = 0; i < valuesT.Count(); i++)
                        {
                            LocalAdminDB adminDB = new LocalAdminDB()
                            {
                                _Id = valuesT[i].Id,
                                UserName = valuesT[i].UserName,
                                CellNumber = valuesT[i].CellNumber,
                                Role = valuesT[i].Role,
                                From = valuesT[i].From,
                                Password = valuesT[i].Password,
                                FutureUse = valuesT[i].FutureUse
                            };
                            handleDB.AddAdminDB(adminDB);
                        }


                        Application.Current.Properties["TeamsCount"] = valuesT.Count().ToString();
                        Application.Current.Properties["UsersCount"] = valuesU.Count().ToString();
                        Application.Current.Properties["HasAdminAndUsersData"] = "YesHasData";
                        await Application.Current.SavePropertiesAsync();
                        LayoutAdmin.IsVisible = true;
                        GetAllData();
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                        LayoutAdmin.IsVisible = true;
                        WaitingLoader.IsRunning = false;
                        WaitingLoader.IsVisible = false;
                    }
                    finally
                    {
                        WaitingLoader.IsRunning = false;
                        WaitingLoader.IsVisible = false;
                    }
                }
            }
        }
        private void BtnRefreshClicked(object sender, EventArgs e)
        {
            new RefreshClass(2);
        }
        private async void AdminLogout(object sender, EventArgs e)
        {
            HandleDB handleDB = new HandleDB();
            LocalAdminDB localAdminDB = new LocalAdminDB();
            LocalUsersDB localUsers = new LocalUsersDB();
            handleDB.DeleteAdminDB(localAdminDB);
            handleDB.DeleteAllUserDB(localUsers);
            Application.Current.Properties.Remove("HasAdminAndUsersData");
            Application.Current.Properties.Remove("AdminCellNumber");
            Application.Current.Properties.Remove("AdminPassword");
            Application.Current.Properties.Remove("AdminRole");
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopAsync();
        }
        private void AddDonorClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddDonorPage());
        }
        private void AddRequestClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddRequestPage());
        }
        private void SendMessageClicked(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Admin, Developer")
            {
                Navigation.PushAsync(new SendMessagePage());
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Admin can send messages!");
            }
        }
        private void AddUserClicked(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Head" || role == "Admin, Developer")
            {
                Navigation.PushAsync(new AddUserPage());
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Admin or Head can add Users!");
            }
        }
        private void TLblTeamTapped(object sender, EventArgs e)
        {

            HeaderSetting(0);
        }
        private void TLblDonorsTapped(object sender, EventArgs e)
        {
            HeaderSetting(1);
        }
        private void TLblRequestTapped(object sender, EventArgs e)
        {
            HeaderSetting(2);
        }
        private void TLblUserTapped(object sender, EventArgs e)
        {
            if (role == "CR")
            {
                DependencyService.Get<IMessage>().ShortAlert("You cannot view users");
            }
            else
            {
                HeaderSetting(3);
            }
        }
        private void HeaderSetting(int v)
        {
            Label[] labels = new Label[] { LblTeam, LblDonors, LblRequests, LblUsers };
            StackLayout[] stacks = new StackLayout[] { SLblTeam, SLblDonors, SLblRequests, SLblUsers };
            StackLayout[] DataStacks = new StackLayout[] { TeamStack, DonorStack, RequestStack, UsersStack };
            switch (v)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        labels[i].TextColor = Color.White;
                        stacks[i].BackgroundColor = Color.FromHex("#d21d1d");
                        DataStacks[i].IsVisible = false;
                    }
                    labels[0].TextColor = Color.FromHex("#d21d1d");
                    stacks[0].BackgroundColor = Color.White;
                    DataStacks[0].IsVisible = true;
                    break;

                case 1:
                    for (int i = 0; i < 4; i++)
                    {
                        labels[i].TextColor = Color.White;
                        stacks[i].BackgroundColor = Color.FromHex("#d21d1d");
                        DataStacks[i].IsVisible = false;
                    }
                    labels[1].TextColor = Color.FromHex("#d21d1d");
                    stacks[1].BackgroundColor = Color.White;
                    DataStacks[1].IsVisible = true;
                    break;

                case 2:
                    for (int i = 0; i < 4; i++)
                    {
                        labels[i].TextColor = Color.White;
                        stacks[i].BackgroundColor = Color.FromHex("#d21d1d");
                        DataStacks[i].IsVisible = false;
                    }
                    labels[2].TextColor = Color.FromHex("#d21d1d");
                    stacks[2].BackgroundColor = Color.White;
                    DataStacks[2].IsVisible = true;
                    break;

                case 3:
                    for (int i = 0; i < 4; i++)
                    {
                        labels[i].TextColor = Color.White;
                        stacks[i].BackgroundColor = Color.FromHex("#d21d1d");
                        DataStacks[i].IsVisible = false;
                    }
                    labels[3].TextColor = Color.FromHex("#d21d1d");
                    stacks[3].BackgroundColor = Color.White;
                    DataStacks[3].IsVisible = true;
                    break;
            }

        }
        private void LvTeamTapped(object sender, ItemTappedEventArgs e)
        {
            var TId = ((LocalAdminDB)e.Item).Id;
            var Tid = ((LocalAdminDB)e.Item)._Id;
            var Tusername = ((LocalAdminDB)e.Item).UserName;
            var Trole = ((LocalAdminDB)e.Item).Role;
            var Tfrom = ((LocalAdminDB)e.Item).From;
            var Tcellnumber = ((LocalAdminDB)e.Item).CellNumber;
            var Taddedby = ((LocalAdminDB)e.Item).FutureUse;
            Navigation.PushAsync(new AdminListviewTappedPage(TId, Tid, Tusername, Trole, Tfrom, Tcellnumber, Taddedby));
        }
        private void LvDonorsTapped(object sender, ItemTappedEventArgs e)
        {
            var DId = ((LocalDonorsDB)e.Item).Id;
            var Did = ((LocalDonorsDB)e.Item)._ID;
            var Dfullname = ((LocalDonorsDB)e.Item).CapName;
            var Dcellnumber = ((LocalDonorsDB)e.Item).CellNumber;
            var Dbloodgroup = ((LocalDonorsDB)e.Item).BloodGroup;
            var Darea = ((LocalDonorsDB)e.Item).Area;
            var Dcity = ((LocalDonorsDB)e.Item).City;
            var Dtodaydate = ((LocalDonorsDB)e.Item).TodayDate;
            var Daddedby = ((LocalDonorsDB)e.Item).AddedBy;
            Navigation.PushAsync(new AdminListviewTappedPage(DId, Did, Dfullname, Dcellnumber, Dbloodgroup, Darea, Dcity, Dtodaydate, Daddedby));
        }
        private void LvRequestsTapped(object sender, ItemTappedEventArgs e)
        {
            var RId = ((LocalRequestsDB)e.Item).Id;
            var Rid = ((LocalRequestsDB)e.Item)._ID;
            var Rfullname = ((LocalRequestsDB)e.Item).CapName;
            var Rcellnumber = ((LocalRequestsDB)e.Item).CellNumber;
            var Rbloodgroup = ((LocalRequestsDB)e.Item).BloodGroup;
            var Rhospital = ((LocalRequestsDB)e.Item).Hospitals;
            var Rcity = ((LocalRequestsDB)e.Item).City;
            var Rtodaydate = ((LocalRequestsDB)e.Item).TodayDate;
            var Raddedby = ((LocalRequestsDB)e.Item).AddedBy;
            var val = 1;
            Navigation.PushAsync(new AdminListviewTappedPage(RId, val, Rid, Rfullname, Rcellnumber, Rbloodgroup, Rhospital, Rcity, Rtodaydate, Raddedby));
        }
        private void LvUsersTapped(object sender, ItemTappedEventArgs e)
        {
            var UId = ((LocalUsersDB)e.Item).Id;
            var Uid = ((LocalUsersDB)e.Item)._ID;
            var Ufullname = ((LocalUsersDB)e.Item).CapName;
            var Ucellnumber = (((LocalUsersDB)e.Item).CellNumber);
            var Ucity = ((LocalUsersDB)e.Item).City;
            var Uarea = ((LocalUsersDB)e.Item).Area;
            var Ubloodgroup = ((LocalUsersDB)e.Item).BloodGroup;
            var Uemail = ((LocalUsersDB)e.Item).Email;
            var Utodaydate = ((LocalUsersDB)e.Item).TodayDate;
            var Ufutureuse = ((LocalUsersDB)e.Item).FutureUse;
            var Uextra = 1;
            Navigation.PushAsync(new AdminListviewTappedPage(UId, Uid, Ufullname, Ucellnumber, Ucity, Uarea, Ubloodgroup, Uemail, Utodaydate, Ufutureuse, Uextra));
        }
        private void AdminSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AdminSearchBar.Text))
            {
                LvTeam.ItemsSource = TeamData;
                LvDonors.ItemsSource = DonorData;
                LvRequests.ItemsSource = RequestData;
                LvUsers.ItemsSource = UserData;
            }
            else
            {
                LvTeam.ItemsSource = TeamData.Where(x => x.CellNumber.Contains(AdminSearchBar.Text));
                LvDonors.ItemsSource = DonorData.Where(x => x.CellNumber.Contains(AdminSearchBar.Text));
                LvRequests.ItemsSource = RequestData.Where(x => x.CellNumber.Contains(AdminSearchBar.Text));
                LvUsers.ItemsSource = UserData.Where(x => x.CellNumber.Contains(AdminSearchBar.Text));
            }
        }
        private async void SearchClicked(object sender, EventArgs e)
        {
            AdminSearchBar.IsVisible = true;
            searchby = await DisplayActionSheet("Search Filter", "Cancel", null, "Search by Name", "Search by Cell Number", "Search by Blood Group", "Search Added by Cell Number");
            switch (searchby)
            {
                case "Cancel":
                    break;

                case "Search by Name":
                    searchby = "Search by Name";
                    break;

                case "Search by Cell Number":
                    searchby = "Search by Cell Number";
                    break;

                case "Search by Blood Group":
                    searchby = "Search by Blood Group";
                    break;

                case "Search Added by Cell Number":
                    searchby = "Search Added by Cell Number";
                    break;
            }
        }
        private void ChangePasswordClicked(object sender, EventArgs e)
        {
            HandleDB dB = new HandleDB();
            Navigation.PushAsync(new ChangePasswordPage(dB.GetAdminDB().ToList()));
        }
        private async void ExportClicked(object sender, EventArgs e)
        {
            int i = 0;
            searchby = await DisplayActionSheet("Choose what to export:", "Cancel", null, "Donors", "Request", "Team", "Users");
            switch (searchby)
            {
                case "Cancel":
                    break;

                case "Donors":
                    exportdata = "Donors: \n";
                    foreach (var v in DonorData)
                    {
                        string row = "(" + DonorData[i].BloodGroup + ") " + DonorData[i].CapName + " " + DonorData[i].CellNumber;
                        row = row.Insert(row.Length, "\n");
                        exportdata = exportdata.Insert(exportdata.Length, row);
                        i++;
                    };
                    break;

                case "Requests":
                    exportdata = "Blood Requests: \n";
                    foreach (var v in RequestData)
                    {
                        string row = "(" + RequestData[i].BloodGroup + ") " + RequestData[i].CapName + " " + RequestData[i].CellNumber;
                        row = row.Insert(row.Length, "\n");
                        exportdata = exportdata.Insert(exportdata.Length, row);
                        i++;
                    };
                    break;

                case "Team":
                    exportdata = "Members: \n";
                    foreach (var v in TeamData)
                    {
                        string row = "(" + TeamData[i].Role + ") " + TeamData[i].UserName + " " + TeamData[i].CellNumber;
                        row = row.Insert(row.Length, "\n");
                        exportdata = exportdata.Insert(exportdata.Length, row);
                        i++;
                    };
                    break;

                case "Users":
                    exportdata = "Users: \n";
                    foreach (var v in UserData)
                    {
                        string row = "(" + UserData[i].CapName + ") " + UserData[i].CellNumber;
                        row = row.Insert(row.Length, "\n");
                        exportdata = exportdata.Insert(exportdata.Length, row);
                        i++;
                    };
                    break;
            }
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = exportdata,
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
    }
}