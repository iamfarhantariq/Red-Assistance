using Newtonsoft.Json;
using Plugin.Connectivity;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SigninPage : ContentPage
    {
        readonly string phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";


        public SigninPage()
        {
            InitializeComponent();
        }


        private void BtnForgetPasswordLabel_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgetPassword());
        }
        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntCellName.Text) || string.IsNullOrEmpty(EntPassword.Text))
            {
                DependencyService.Get<IMessage>().ShortAlert("Feild is Empty!");
            }
            else
            {
                if (!Regex.IsMatch(EntCellName.Text, phonepattern))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Incorrect Cellnumber!");
                }
                else
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Network Error!");
                    }
                    else
                    {
                        try
                        {
                            LoginLayoutScrlvw.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            var cellnumber = EntCellName.Text;
                            var password = EncryptDecryptClass.Crypt(EntPassword.Text);
                            var httpClient = new HttpClient();
                            var httpClientD = new HttpClient();
                            var httpClientR = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi?cellnumber={0}&&password={1}",
                                cellnumber,
                                password));
                            var responserequests = await httpClientR.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi?cellnumber=" + cellnumber);
                            var responsedonors = await httpClientD.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi?cellnumber=" + cellnumber);

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = response.Content.ReadAsStringAsync().Result;
                                if (result == "[]")
                                {
                                    DependencyService.Get<IMessage>().ShortAlert("User Not Found!");
                                    LoginLayoutScrlvw.IsVisible = true;
                                }
                                else
                                {
                                    var values = JsonConvert.DeserializeObject<List<SignupClass>>(result);
                                    LocalDB localDB = new LocalDB()
                                    {
                                        _ID = values[0].Id,
                                        FullName = values[0].FullName,
                                        CellNumber = values[0].CellNumber,
                                        City = values[0].City,
                                        Area = values[0].Area,
                                        BloodGroup = values[0].BloodGroup,
                                        Email = values[0].Email,
                                        Password = values[0].Password,
                                        TodayDate = values[0].TodayDate,
                                        FutureUse = null,
                                    };
                                    //insert in localDB
                                    HandleDB dB = new HandleDB();
                                    dB.AddUserDB(localDB);


                                    var valuesUR = JsonConvert.DeserializeObject<List<AddRequestClass>>(responserequests);
                                    var valuesUD = JsonConvert.DeserializeObject<List<AddDonorClass>>(responsedonors);
                                    var a = Application.Current.Properties["UserDonors"] = valuesUD.Count();
                                    var b = Application.Current.Properties["UserRequests"] = valuesUR.Count();

                                    //Application.Current.Properties["Logged"] = true;
                                    Application.Current.Properties["UserCell"] = cellnumber;
                                    Application.Current.Properties["UserName"] = values[0].FullName;
                                    await Application.Current.SavePropertiesAsync();
                                    await Navigation.PopAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                            LoginLayoutScrlvw.IsVisible = true;
                        }
                        finally
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                        }
                    }
                }
            }
        }
        private void RegisterBtn_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterUser());
        }
    }
}