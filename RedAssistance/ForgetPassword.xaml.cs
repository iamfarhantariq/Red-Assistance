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
    public partial class ForgetPassword : ContentPage
    {
        private SignupClass signupClass;
        private List<SignupClass> data;
        private string v;
        private List<AdminClass> Admindata;
        private AdminClass _AdminClass;

        public ForgetPassword()
        {
            InitializeComponent();
        }

        public ForgetPassword(string v)
        {
            InitializeComponent();
            this.v = v;
        }

        private async void BtnFindAccount_Clicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";


            if (string.IsNullOrEmpty(EntCellNumber.Text))
            {
                DependencyService.Get<IMessage>().ShortAlert("Empty!");
            }
            else
            {
                if (!Regex.IsMatch(phone, phonepattern))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Incorrect Cell Number!");
                }
                else
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Turn on network Connection!");
                    }
                    else
                    {
                        var findcell = EntCellNumber.Text;

                        try
                        {
                            LayoutCellNumber.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            if (v == "Admin")
                            {
                                var httpClientA = new HttpClient();
                                var responseA = await httpClientA.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi?cellnumber={0}", findcell));
                                if (responseA.StatusCode == HttpStatusCode.OK || responseA.StatusCode == HttpStatusCode.RequestTimeout)
                                {
                                    var result = responseA.Content.ReadAsStringAsync().Result;
                                    if (result == "[]")
                                    {
                                        LayoutCellNumber.IsVisible = true;
                                        DependencyService.Get<IMessage>().ShortAlert("Cell Number Not Found!");
                                    }
                                    else
                                    {
                                        var values = JsonConvert.DeserializeObject<List<AdminClass>>(result);
                                        Admindata = values;
                                        LblSendCode.Text = findcell + " found! Send Confirmation code for account recovery.";
                                        LayoutSendMessage.IsVisible = true;
                                    }
                                }
                            }
                            else
                            {
                                var httpClient = new HttpClient();
                                var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi?cellnumber={0}", findcell));

                                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.RequestTimeout)
                                {
                                    var result = response.Content.ReadAsStringAsync().Result;
                                    if (result == "[]")
                                    {
                                        LayoutCellNumber.IsVisible = true;
                                        DependencyService.Get<IMessage>().ShortAlert(" Cell Number Not Found!");
                                    }
                                    else
                                    {
                                        var values = JsonConvert.DeserializeObject<List<SignupClass>>(result);
                                        data = values;
                                        LblSendCode.Text = findcell + " found! Send Confirmation code for account recovery.";
                                        LayoutSendMessage.IsVisible = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().ShortAlert(ex.ToString());
                            LayoutCellNumber.IsVisible = true;
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
        private async void BtnSendCodeToNumber_Clicked(object sender, EventArgs e)
        {
            int _min = 0000;
            int _max = 9999;
            Random _rdm = new Random();
            int code = _rdm.Next(_min, _max);

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().ShortAlert("Network not available!");
            }
            else
            {
                try
                {
                    LayoutSendMessage.IsVisible = false;
                    WaitingLoader.IsRunning = true;
                    WaitingLoader.IsVisible = true;

                    if (v == "Admin")
                    {
                        var httpClientMsg = new HttpClient();
                        var orgfarmat = Admindata[0].CellNumber.Substring(1);
                        string message = code + " is your Recovery Code for 'Blood App'  \n\n A Project of University of Education!";
                        await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to=92{0}&lang=English&msg={1}&type=xml", orgfarmat, message), null);

                        AdminClass adminClass = new AdminClass()
                        {
                            Id = Admindata[0].Id,
                            UserName = Admindata[0].UserName,
                            CellNumber = Admindata[0].CellNumber,
                            Role = Admindata[0].Role,
                            From = Admindata[0].From,
                            Password = Admindata[0].Password,
                            FutureUse = code.ToString(),
                        };

                        var httpClientCode = new HttpClient();
                        var json = JsonConvert.SerializeObject(adminClass);
                        HttpContent httpContent = new StringContent(json);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        await httpClientCode.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi/{0}", adminClass.Id), httpContent);

                        _AdminClass = adminClass;

                        await DisplayAlert("Dear " + Admindata[0].UserName, "We are sending you a recovery code to " + Admindata[0].CellNumber, "Ok");
                        DependencyService.Get<IMessage>().ShortAlert("Sending...!");
                        LayoutCodeSection.IsVisible = true;
                        DependencyService.Get<IMessage>().ShortAlert("Sent");
                    }
                    else
                    {
                        var httpClientMsg = new HttpClient();
                        var orgfarmat = data[0].CellNumber.Substring(1);
                        string message = code + " is your Recovery Code for 'Blood App'  \n\n A Project of University of Education!";
                        await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to=92{0}&lang=English&msg={1}&type=xml", orgfarmat, message), null);

                        SignupClass updateinfo = new SignupClass()
                        {
                            Id = data[0].Id,
                            FullName = data[0].FullName,
                            CellNumber = data[0].CellNumber,
                            City = data[0].City,
                            Area = data[0].Area,
                            BloodGroup = data[0].BloodGroup,
                            Email = data[0].Email,
                            Password = data[0].Password,
                            TodayDate = data[0].TodayDate,
                            FutureUse = code.ToString(),
                        };

                        var httpClientCode = new HttpClient();
                        var json = JsonConvert.SerializeObject(updateinfo);
                        HttpContent httpContent = new StringContent(json);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        await httpClientCode.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", updateinfo.Id), httpContent);

                        signupClass = updateinfo;

                        await DisplayAlert("Dear " + data[0].FullName, "We are sending you a recovery code to " + data[0].CellNumber, "Ok");
                        DependencyService.Get<IMessage>().ShortAlert("Sending...!");
                        LayoutCodeSection.IsVisible = true;
                        DependencyService.Get<IMessage>().ShortAlert("Sent");
                    }
                }
                catch (Exception ex)
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                    DependencyService.Get<IMessage>().ShortAlert(ex.ToString());
                    LayoutSendMessage.IsVisible = true;
                }
                finally
                {
                    WaitingLoader.IsRunning = false;
                    WaitingLoader.IsVisible = false;
                }
            }
        }
        private async void BtnSavePassword_Clicked(object sender, EventArgs e)
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
                    if (EntNewPassword.Text == "12345")
                    {
                        DependencyService.Get<IMessage>().ShortAlert("You cannot set 12345");
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

                                if (v == "Admin")
                                {
                                    AdminClass adminClass = new AdminClass()
                                    {
                                        Id = _AdminClass.Id,
                                        UserName = _AdminClass.UserName,
                                        CellNumber = _AdminClass.CellNumber,
                                        Role = _AdminClass.Role,
                                        From = _AdminClass.From,
                                        Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                        FutureUse = _AdminClass.FutureUse,
                                    };

                                    var httpClient = new HttpClient();
                                    var json = JsonConvert.SerializeObject(adminClass);
                                    HttpContent httpContent = new StringContent(json);
                                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi/{0}", adminClass.Id), httpContent);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert("Password Changed!");
                                        await Navigation.PopAsync();
                                    }
                                }
                                else
                                {
                                    SignupClass pswd = new SignupClass()
                                    {
                                        Id = signupClass.Id,
                                        FullName = signupClass.FullName,
                                        CellNumber = signupClass.CellNumber,
                                        City = signupClass.City,
                                        Area = signupClass.Area,
                                        BloodGroup = signupClass.BloodGroup,
                                        Email = signupClass.Email,
                                        TodayDate = signupClass.TodayDate,
                                        Password = EncryptDecryptClass.Crypt(EntNewPassword.Text),
                                        FutureUse = signupClass.FutureUse,
                                    };

                                    var httpClient = new HttpClient();
                                    var json = JsonConvert.SerializeObject(pswd);
                                    HttpContent httpContent = new StringContent(json);
                                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", pswd.Id), httpContent);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert("Password Changed!");
                                        await Navigation.PopAsync();
                                    }
                                }
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
        private void BtnCodeSend_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().ShortAlert("Network Error!");
            }
            else
            {
                if (String.IsNullOrEmpty(EntRecoveryCode.Text))
                {
                    DependencyService.Get<IMessage>().ShortAlert("Feild is Empty!");
                }
                else
                {
                    try
                    {
                        LayoutCodeSection.IsVisible = false;
                        WaitingLoader.IsRunning = true;
                        WaitingLoader.IsVisible = true;
                        if (v == "Admin")
                        {
                            if (EntRecoveryCode.Text == _AdminClass.FutureUse)
                            {
                                ChangePasswordLayer.IsVisible = true;
                            }
                            else
                            {
                                LayoutCodeSection.IsVisible = true;
                                DependencyService.Get<IMessage>().ShortAlert("Incorrect code!");
                            }
                        }
                        else
                        {
                            if (EntRecoveryCode.Text == signupClass.FutureUse)
                            {
                                ChangePasswordLayer.IsVisible = true;
                            }
                            else
                            {
                                LayoutCodeSection.IsVisible = true;
                                DependencyService.Get<IMessage>().ShortAlert("Incorrect code!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WaitingLoader.IsRunning = false;
                        WaitingLoader.IsVisible = false;
                        DependencyService.Get<IMessage>().ShortAlert(ex.ToString());
                        LayoutCodeSection.IsVisible = true;

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