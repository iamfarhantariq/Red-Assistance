using Newtonsoft.Json;
using Plugin.Connectivity;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {
        readonly string phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";
        private string password;
        private List<AdminClass> admindata;

        public AdminLoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey("AdminCellNumber") || Application.Current.Properties.ContainsKey("AdminPassword"))
            {
                Navigation.PopAsync();
            }
        }

        private void BtnForgetPasswordLabel_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgetPassword("Admin"));
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntCell.Text) || string.IsNullOrEmpty(EntPassword.Text))
            {
                DependencyService.Get<IMessage>().ShortAlert("Feild is Empty!");
            }
            else
            {
                if (!Regex.IsMatch(EntCell.Text, phonepattern))
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
                            var cellnumber = EntCell.Text;

                            if (EntPassword.Text == "12345")
                            {
                                password = "12345";
                            }
                            else
                            {
                                password = EncryptDecryptClass.Crypt(EntPassword.Text);
                            }

                            var httpClient = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi?cellnumber={0}&&password={1}",
                                cellnumber,
                                password));
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
                                    admindata = JsonConvert.DeserializeObject<List<AdminClass>>(result);
                                    if (admindata[0].Password == "12345")
                                    {
                                        ChangePasswordLayer.IsVisible = true;
                                        LoginLayoutScrlvw.IsVisible = false;

                                    }
                                    else
                                    {
                                        Application.Current.Properties["AdminCellNumber"] = admindata[0].CellNumber;
                                        Application.Current.Properties["AdminPassword"] = admindata[0].Password;
                                        Application.Current.Properties["AdminRole"] = admindata[0].Role;
                                        await Application.Current.SavePropertiesAsync();
                                        await Navigation.PushAsync(new AdminDashboardPage());
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoginLayoutScrlvw.IsVisible = true;
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
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

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().ShortAlert("Network Error!");
            }
            else
            {
                if (string.IsNullOrEmpty(EntNewPassword.Text) || string.IsNullOrEmpty(EntConfirmPassword.Text))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Feild is Empty!");
                }
                else
                {
                    if (EntNewPassword.Text != EntConfirmPassword.Text)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Password didn't Match!");
                    }
                    else
                    {
                        try
                        {
                            ChangePasswordLayer.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            AdminClass adminclass = new AdminClass()
                            {
                                Id = admindata[0].Id,
                                UserName = admindata[0].UserName,
                                CellNumber = admindata[0].CellNumber,
                                Role = admindata[0].Role,
                                From = admindata[0].From,
                                Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                FutureUse = admindata[0].FutureUse
                            };

                            var httpClientA = new HttpClient();
                            var json = JsonConvert.SerializeObject(adminclass);
                            HttpContent httpContentA = new StringContent(json);
                            httpContentA.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var responseA = await httpClientA.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi/{0}", admindata[0].Id), httpContentA);

                            Application.Current.Properties["AdminCellNumber"] = admindata[0].CellNumber;
                            Application.Current.Properties["AdminPassword"] = EncryptDecryptClass.Crypt(EntNewPassword.Text);
                            Application.Current.Properties["AdminRole"] = admindata[0].Role;
                            await Application.Current.SavePropertiesAsync();
                            await Navigation.PushAsync(new AdminDashboardPage());
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().ShortAlert(ex.ToString());
                            ChangePasswordLayer.IsVisible = true;
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
    }
}