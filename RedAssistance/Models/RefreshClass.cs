using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace RedAssistance.Models
{
    public class RefreshClass
    {
        private HandleDB handleDB;
        private LocalDonorsDB donorsdb;
        private LocalRequestsDB requestsdb;
        private LocalUsersDB userdb;
        private LocalAdminDB admindb;
        private LocalDB localdb;
        private int donorscount;
        private int requestscount;
        private int teamscount;
        private int usersscount;

        public RefreshClass(int i)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("Turn on network connection!");
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Refreshing");
                handleDB = new HandleDB();
                donorsdb = new LocalDonorsDB();
                requestsdb = new LocalRequestsDB();
                userdb = new LocalUsersDB();
                admindb = new LocalAdminDB();
                localdb = new LocalDB();
                if (i == 1)
                {
                    donorscount = Convert.ToInt32(Application.Current.Properties["DonorsCount"]);
                    requestscount = Convert.ToInt32(Application.Current.Properties["RequestsCount"]);
                    DonorsRefresh();
                    RequestsRefresh();
                }
                else if (i == 2)
                {
                    donorscount = Convert.ToInt32(Application.Current.Properties["DonorsCount"]);
                    requestscount = Convert.ToInt32(Application.Current.Properties["RequestsCount"]);
                    teamscount = Convert.ToInt32(Application.Current.Properties["TeamsCount"]);
                    usersscount = Convert.ToInt32(Application.Current.Properties["UsersCount"]);
                    DonorsRefresh();
                    RequestsRefresh();
                    UsersRefresh();
                    TeamsRefresh();
                }
                else if (i == 3)
                {
                    donorscount = Convert.ToInt32(Application.Current.Properties["DonorsCount"]);
                    DonorsRefresh();
                }
                //else if (i == 4)
                //{
                //    requestscount = Convert.ToInt32(Application.Current.Properties["RequestsCount"]);
                //    RequestsRefresh();
                //}
                //else if (i == 5)
                //{
                //    teamscount = Convert.ToInt32(Application.Current.Properties["TeamsCount"]);
                //    TeamsRefresh();
                //}

                DependencyService.Get<IMessage>().ShortAlert("Success");
            }
        }

        private async void TeamsRefresh()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var valuesT = JsonConvert.DeserializeObject<List<AdminClass>>(result);

                if (teamscount != valuesT.Count())
                {
                    handleDB.DeleteAdminDB(admindb);

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
                        HandleDB dB = new HandleDB();
                        dB.AddAdminDB(adminDB);
                    }
                    Application.Current.Properties["TeamsCount"] = valuesT.Count().ToString();
                    await Application.Current.SavePropertiesAsync();
                }
            }
        }
        private async void UsersRefresh()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var valuesU = JsonConvert.DeserializeObject<List<SignupClass>>(result);
                if (usersscount != valuesU.Count())
                {
                    handleDB.DeleteAllUserDB(userdb);

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
                        HandleDB dB = new HandleDB();
                        dB.AddAllUsersDB(usersDB);
                    }

                    Application.Current.Properties["UsersCount"] = valuesU.Count().ToString();
                    await Application.Current.SavePropertiesAsync();
                }

            }
        }
        private async void RequestsRefresh()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var valuesR = JsonConvert.DeserializeObject<List<AddRequestClass>>(result);

                if (requestscount != valuesR.Count())
                {
                    handleDB.DeleteRequestsDB(requestsdb);

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
                        HandleDB dB = new HandleDB();
                        dB.AddRequestsDB(requestsDB);
                    }

                    Application.Current.Properties["RequestsCount"] = valuesR.Count().ToString();
                    await Application.Current.SavePropertiesAsync();
                }
            }
        }
        private async void DonorsRefresh()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var valuesD = JsonConvert.DeserializeObject<List<AddDonorClass>>(result);

                if (donorscount != valuesD.Count())
                {
                    handleDB.DeleteDonorsDB(donorsdb);

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
                        HandleDB dB = new HandleDB();
                        dB.AddDonorsDB(donorsDB);
                    }
                    Application.Current.Properties["DonorsCount"] = valuesD.Count().ToString();
                    await Application.Current.SavePropertiesAsync();
                }
            }
        }
    }
}
