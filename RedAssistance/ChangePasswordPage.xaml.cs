using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using RedAssistance.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private string v;
        private List<LocalAdminDB> admindata;
        private List<LocalDB> values;

        public ChangePasswordPage(List<LocalDB> _values)
        {
            InitializeComponent();
            values = _values;
        }

        public ChangePasswordPage(List<LocalAdminDB> _values)
        {
            InitializeComponent();
            v = "Admin";
            admindata = _values;
        }

        private async void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntCurrentPassword.Text) || string.IsNullOrEmpty(EntNewPassword.Text) || string.IsNullOrEmpty(EntConfirmPassword.Text))
            {
                DependencyService.Get<IMessage>().LongAlert("One of the Entry is Empty!");

            }
            else
            {
                if (EntNewPassword.Text != EntConfirmPassword.Text)
                {
                    DependencyService.Get<IMessage>().LongAlert("New Password and Confirm Password didn't Match!");
                }
                else
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().LongAlert("You're offline!");
                    }
                    else
                    {
                        try
                        {
                            LayoutChangePassword.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            if (v == "Admin")
                            {
                                var oldPasswordA = EncryptDecryptClass.Decrypt(admindata[0].Password);
                                if (oldPasswordA != EntCurrentPassword.Text)
                                {
                                    DependencyService.Get<IMessage>().LongAlert("Your old password is incorrect!");
                                }
                                else
                                {
                                    AdminClass adminclass = new AdminClass()
                                    {
                                        Id = admindata[0]._Id,
                                        UserName = admindata[0].UserName,
                                        CellNumber = admindata[0].CellNumber,
                                        Role = admindata[0].Role,
                                        From = admindata[0].From,
                                        Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                        FutureUse = admindata[0].FutureUse
                                    };

                                    var httpClient = new HttpClient();
                                    var json = JsonConvert.SerializeObject(adminclass);
                                    HttpContent httpContent = new StringContent(json);
                                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi/{0}", admindata[0]._Id), httpContent);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        DependencyService.Get<IMessage>().LongAlert("Password Changed!");

                                        HandleDB handle = new HandleDB();
                                        LocalAdminDB localAdmin = new LocalAdminDB()
                                        {
                                            Id = admindata[0]._Id,
                                            UserName = admindata[0].UserName,
                                            CellNumber = admindata[0].CellNumber,
                                            Role = admindata[0].Role,
                                            From = admindata[0].From,
                                            Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                            FutureUse = admindata[0].FutureUse
                                        };
                                        handle.UpdateAdminDB(localAdmin);
                                        await Navigation.PopAsync();
                                    }
                                }
                            }
                            else
                            {
                                var oldpassword = EncryptDecryptClass.Decrypt(values[0].Password);
                                if (oldpassword != EntCurrentPassword.Text)
                                {
                                    DependencyService.Get<IMessage>().LongAlert("Your old password is incorrect!");
                                }
                                else
                                {
                                    SignupClass signupClass = new SignupClass()
                                    {
                                        Id = values[0]._ID,
                                        FullName = values[0].FullName,
                                        CellNumber = values[0].CellNumber,
                                        City = values[0].City,
                                        Area = values[0].Area,
                                        BloodGroup = values[0].BloodGroup,
                                        Email = values[0].Email,
                                        Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                        TodayDate = values[0].TodayDate,
                                        FutureUse = values[0].FutureUse,
                                    };

                                    var httpClient = new HttpClient();
                                    var json = JsonConvert.SerializeObject(signupClass);
                                    HttpContent httpContent = new StringContent(json);
                                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", values[0]._ID), httpContent);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        DependencyService.Get<IMessage>().LongAlert("Password Changed!");

                                        HandleDB handle = new HandleDB();
                                        LocalDB local = new LocalDB()
                                        {
                                            Id = values[0]._ID,
                                            FullName = values[0].FullName,
                                            CellNumber = values[0].CellNumber,
                                            City = values[0].City,
                                            Area = values[0].Area,
                                            BloodGroup = values[0].BloodGroup,
                                            Email = values[0].Email,
                                            Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                            TodayDate = values[0].TodayDate,
                                            FutureUse = values[0].FutureUse
                                        };
                                        handle.UpdateUserDB(local);
                                        await Navigation.PopAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            LayoutChangePassword.IsVisible = true;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                        }
                        finally
                        {
                            LayoutChangePassword.IsVisible = true;
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                        }
                    }
                }
            }
        }

    }
}
