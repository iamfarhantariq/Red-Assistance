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
    public partial class AddUserPage : ContentPage
    {
        private string RoleValue;

        public AddUserPage()
        {
            InitializeComponent();
        }

        private void PickersItemsAdd()
        {
            PkrRole.Items.Add("Admin");
            PkrRole.Items.Add("Head");
            PkrRole.Items.Add("Volunteer");
            PkrRole.Items.Add("CR");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PickersItemsAdd();
        }

        private void PkrRoleSelectedIndexChanged(object sender, EventArgs e)
        {
            RoleValue = PkrRole.Items[PkrRole.SelectedIndex];
        }

        private async void BtnAddUser_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(EntCellNumber.Text)
                || string.IsNullOrWhiteSpace(RoleValue) || string.IsNullOrWhiteSpace(EntFrom.Text))
            {
                DependencyService.Get<IMessage>().LongAlert("Enter data to all feilds!");
            }
            else
            {
                if (!Regex.IsMatch(phone, phonepattern))
                {
                    DependencyService.Get<IMessage>().LongAlert("Incorrect phone number");
                }
                else
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().LongAlert("Turn on network connection!");
                    }
                    else
                    {
                        var cell = Application.Current.Properties["AdminCellNumber"].ToString();

                        AdminClass adminClass = new AdminClass()
                        {
                            UserName = EntFullName.Text,
                            CellNumber = EntCellNumber.Text,
                            Role = RoleValue,
                            From = EntFrom.Text,
                            Password = "12345",
                            FutureUse = cell,
                        };
                        try
                        {
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            StackLayoutAddUser.IsVisible = false;

                            var httpClient = new HttpClient();
                            var json = JsonConvert.SerializeObject(adminClass);
                            HttpContent httpContent = new StringContent(json);
                            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            var response = await httpClient.PostAsync("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi", httpContent);
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                var httpClientMsg = new HttpClient();
                                string cellnumber = EntCellNumber.Text.Substring(1).Insert(0, "92");
                                string message = string.Format("Dear '{0}', You are added as {1} in Admin Console of Red Assistance App. Your first password is 12345, change it to make your acount secure. " +
                                    "Download the Red Assistance app: https://bit.ly/2JJnyWW", EntFullName.Text, RoleValue);
                                await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", cellnumber, message), null);

                                LocalAdminDB localadmin = new LocalAdminDB()
                                {
                                    UserName = EntFullName.Text,
                                    CellNumber = EntCellNumber.Text,
                                    Role = RoleValue,
                                    From = EntFrom.Text,
                                    Password = "12345",
                                    FutureUse = cell
                                };
                                HandleDB handleDB = new HandleDB();
                                handleDB.AddAdminDB(localadmin);
                            }

                            DependencyService.Get<IMessage>().LongAlert("Added!");

                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                            StackLayoutAddUser.IsVisible = true;
                        }
                        finally
                        {
                            InitializeComponent();
                            StackLayoutAddUser.IsVisible = true;
                            PickersItemsAdd();
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                        }
                    }
                }
            }
        }
    }
}