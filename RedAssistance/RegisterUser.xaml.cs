using Newtonsoft.Json;
using Plugin.Connectivity;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUser : ContentPage
    {
        private string bloodgroup = string.Empty;
        private Label _previousLabel;
        private Label _currentLabel;
        private string recoverycode;
        private string CellNumber;
        private string CityValue;
        private string AreaValue;
        private List<LocalDB> list;
        private List<LocalUsersDB> data;
        private int v;

        public RegisterUser()
        {
            InitializeComponent();
            BackgroundColor = Color.White;
            Title = "Registeration Form";
            PickersItemsAdd();
        }

        public RegisterUser(List<LocalDB> list)
        {
            InitializeComponent();
            this.list = list;
            BackgroundColor = Color.White;
            Title = "Update User";
            PickersItemsAdd();

            LayoutCellNumber.IsVisible = false;
            BtnCreateAccount.IsVisible = false;
            LblPassword.IsVisible = false;
            EntPassword.IsVisible = false;
            ScrollViewDetails.IsVisible = true;
            BtnUpdateAccount.IsVisible = true;
            EntFullName.Text = list[0].FullName;
            EntEmail.Text = list[0].Email;
        }

        public RegisterUser(List<LocalUsersDB> data, int v)
        {
            InitializeComponent();
            this.data = data;
            this.v = v;
            BackgroundColor = Color.White;
            Title = "Update User";
            PickersItemsAdd();

            LayoutCellNumber.IsVisible = false;
            BtnCreateAccount.IsVisible = false;
            LblPassword.IsVisible = false;
            EntPassword.IsVisible = false;
            ScrollViewDetails.IsVisible = true;
            BtnUpdateAccount2.IsVisible = true;
            EntFullName.Text = data[0].FullName;
            EntEmail.Text = data[0].Email;
        }

        private void PickersItemsAdd()
        {
            PkrCity.Items.Add("Lahore");

            PkrArea.Items.Add("Other");
            PkrArea.Items.Add("Allama Iqbal Town");
            PkrArea.Items.Add("Awan Town");
            PkrArea.Items.Add("Aziz Bhatti Town");
            PkrArea.Items.Add("Baghbanpura");
            PkrArea.Items.Add("Batapur");
            PkrArea.Items.Add("Begum kot");
            PkrArea.Items.Add("Chah Miran");
            PkrArea.Items.Add("Data Ganj Baksh Town");
            PkrArea.Items.Add("Defence Housing Authority (Lahore)");
            PkrArea.Items.Add("Faisal Town");
            PkrArea.Items.Add("Garden Town");
            PkrArea.Items.Add("Gawalmandi");
            PkrArea.Items.Add("Gosia Park (Band Road)");
            PkrArea.Items.Add("Green Town");
            PkrArea.Items.Add("Gulberg Town");
            PkrArea.Items.Add("Gulshan-e-Ravi");
            PkrArea.Items.Add("Harbanspura");
            PkrArea.Items.Add("Hassan Town");
            PkrArea.Items.Add("Ichhra");
            PkrArea.Items.Add("Jallo Mor");
            PkrArea.Items.Add("Johar Town");
            PkrArea.Items.Add("Krishan Nagar");
            PkrArea.Items.Add("Lahore Cantonment");
            PkrArea.Items.Add("Laxshmi Chowk");
            PkrArea.Items.Add("Model Town");
            PkrArea.Items.Add("Mughalpura");
            PkrArea.Items.Add("Muhafiz Town");
            PkrArea.Items.Add("Mustafa Town");
            PkrArea.Items.Add("New Muslim Town");
            PkrArea.Items.Add("Nishtar Town");
            PkrArea.Items.Add("Pasco Society");
            PkrArea.Items.Add("Punjab Cooperative Housing Society");
            PkrArea.Items.Add("Qila Gujar Singh");
            PkrArea.Items.Add("Ravi Town");
            PkrArea.Items.Add("Samanabad Town");
            PkrArea.Items.Add("Shad Bagh");
            PkrArea.Items.Add("Shadman Town");
            PkrArea.Items.Add("Shahdara");
            PkrArea.Items.Add("Shalimar Town");
            PkrArea.Items.Add("SukhChyan Housing Estate");
            PkrArea.Items.Add("Township");
            PkrArea.Items.Add("WAPDA Town");
            PkrArea.Items.Add("Wagah Town");
            PkrArea.Items.Add("Walled City of Lahore");
        }

        private void PkrAddDonorCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CityValue = PkrCity.Items[PkrCity.SelectedIndex];
        }

        private void PkrAddDonorArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AreaValue = PkrArea.Items[PkrArea.SelectedIndex];
        }


        #region BloodSelectionRegion
        private void BloodGroupSelect(Label lbl, string bld)
        {
            if (_previousLabel == null)
            {
                _previousLabel = lbl;
                bloodgroup = bld;
                lbl.BackgroundColor = Color.Default;
                lbl.TextColor = Color.FromHex("d21d1d");
                _currentLabel = _previousLabel;
            }
            else if (_currentLabel == lbl)
            {
                lbl.BackgroundColor = Color.FromHex("d21d1d");
                lbl.TextColor = Color.White;
                _previousLabel = null;
                _currentLabel = null;
                bloodgroup = string.Empty;
            }
            else
            {
                _currentLabel = lbl;
                bloodgroup = bld;
                _previousLabel.BackgroundColor = Color.FromHex("d21d1d");
                _previousLabel.TextColor = Color.White;
                lbl.BackgroundColor = Color.Default;
                lbl.TextColor = Color.FromHex("d21d1d");
                _previousLabel = lbl;
            }
        }

        private void Ap_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblAp, "A+");
        }

        private void An_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblAn, "A-");
        }

        private void Bp_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblBp, "B+");
        }

        private void Bn_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblBn, "B-");
        }

        private void Abp_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblAbp, "AB+");
        }

        private void Abn_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblAbn, "AB-");
        }

        private void Op_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblOp, "O+");
        }

        private void On_OnTapped(object sender, EventArgs e)
        {
            BloodGroupSelect(LblOn, "O-");
        }
        #endregion

        private async void BtnNextCellClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            int _min = 0000;
            int _max = 9999;
            Random _rdm = new Random();
            int code = _rdm.Next(_min, _max);
            recoverycode = code.ToString();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(EntCellNumber.Text))
                {
                    DependencyService.Get<IMessage>().LongAlert("Enter Cell Number!");
                }
                else
                {
                    if (!Regex.IsMatch(phone, phonepattern))
                    {
                        DependencyService.Get<IMessage>().LongAlert("Cell Number is incorrect!");
                    }
                    else
                    {
                        try
                        {
                            LayoutCellNumber.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;

                            var httpClient = new HttpClient();
                            var response = await httpClient.GetAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi?cellnumber={0}", EntCellNumber.Text));
                            var result = response.Content.ReadAsStringAsync().Result;
                            var name = JsonConvert.DeserializeObject<List<SignupClass>>(result);

                            if (result == "[]")
                            {
                                var httpClientMsg = new HttpClient();
                                var orgfarmat = EntCellNumber.Text.Substring(1);
                                String message = code.ToString() + " is your Verification Code";
                                await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to=92{0}&lang=English&msg={1}&type=xml", orgfarmat, message), null);
                                DependencyService.Get<IMessage>().ShortAlert("Sending Message..!");

                                LayoutCodeSection.IsVisible = true;
                                DependencyService.Get<IMessage>().ShortAlert("Sent");

                            }
                            else if (EntCellNumber.Text == name[0].CellNumber)
                            {
                                DependencyService.Get<IMessage>().LongAlert("Cell Number Exist. Please Login!");
                                LayoutCellNumber.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
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

        private void BtnCodeVeryClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntVerificationCode.Text))
            {
                DependencyService.Get<IMessage>().LongAlert("Enter Code");
            }
            else
            {
                if (EntVerificationCode.Text != recoverycode)
                {
                    DependencyService.Get<IMessage>().LongAlert("Code is incorrect");
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert("Verified!");
                    LayoutCodeSection.IsVisible = false;
                    ScrollViewDetails.IsVisible = true;
                }
            }
        }

        private async void CreateAccountClicked(object sender, EventArgs e)
        {
            String email = EntEmail.Text;
            String emailpattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(AreaValue)
                || string.IsNullOrWhiteSpace(EntEmail.Text) || string.IsNullOrWhiteSpace(EntPassword.Text))
                {
                    DependencyService.Get<IMessage>().LongAlert("Fields are Empty!");
                }
                else
                {
                    if (!Regex.IsMatch(email, emailpattern))
                    {
                        DependencyService.Get<IMessage>().LongAlert("Incorrect Email Format");
                    }
                    else
                    {
                        var ans = await DisplayAlert("Disclaimer", "- People registering on this App must understand that the information provided by them on the registration page is available to a person seeking for a particular blood group.\n - We do not sell contact details of potential donors to any third party or use it in any way for commercial gains.\n - We do not arrange for blood. We only provide relevant information about potential donors to those in need of blood.\n - We do not guarantee that a potential donor will agree to donate blood whenever called upon to do so. It is entirely at the discretion of the individual whether or not to donate blood.\n - We do not claim that potential donors are free from any disease, ailment, or bodily conditions preventing them from donating blood at the time when they are contacted for blood donation. Onus is completely on the individual looking for blood to verify these details from the donor.\n - We urge you not to make false registrations if you do not seriously wish to donate blood. It is a matter of life and death for those in need of blood in an emergency or otherwise.\n - We reserve right to inactivate member at any given time in case found wrong information given or misuse of service.", "Accept", "Deny");
                        if (ans == true)
                        {
                            DateTime dateValue = DateTime.Now;
                            if (bloodgroup == string.Empty)
                            {
                                bloodgroup = "No";
                            }
                            SignupClass signupClass = new SignupClass()
                            {
                                FullName = EntFullName.Text,
                                CellNumber = EntCellNumber.Text,
                                City = CityValue,
                                Area = AreaValue,
                                BloodGroup = bloodgroup,
                                Email = EntEmail.Text,
                                Password = EncryptDecryptClass.Crypt(EntPassword.Text),
                                TodayDate = dateValue.ToString(),
                                FutureUse = recoverycode,
                            };

                            try
                            {
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;
                                ScrollViewDetails.IsVisible = false;

                                var httpClient = new HttpClient();
                                var json = JsonConvert.SerializeObject(signupClass);
                                HttpContent httpContent = new StringContent(json);
                                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                await httpClient.PostAsync("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi", httpContent);

                                DependencyService.Get<IMessage>().ShortAlert("Created");
                                await Navigation.PopAsync();
                                DependencyService.Get<IMessage>().ShortAlert("Login");
                            }
                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                                ScrollViewDetails.IsVisible = true;
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

        private async void UpdateAccountClicked(object sender, EventArgs e)
        {
            String email = EntEmail.Text;
            String emailpattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(AreaValue)
                || string.IsNullOrWhiteSpace(EntEmail.Text))
                {
                    DependencyService.Get<IMessage>().LongAlert("Fields are Empty!");
                }
                else
                {
                    if (!Regex.IsMatch(email, emailpattern))
                    {
                        DependencyService.Get<IMessage>().LongAlert("Incorrect Email Format");
                    }
                    else
                    {
                        if (bloodgroup == string.Empty)
                        {
                            bloodgroup = "No";
                        }

                        SignupClass signupClass = new SignupClass()
                        {
                            Id = list[0]._ID,
                            FullName = EntFullName.Text,
                            CellNumber = list[0].CellNumber,
                            City = CityValue,
                            Area = AreaValue,
                            BloodGroup = bloodgroup,
                            Email = EntEmail.Text,
                            Password = list[0].Password,
                            TodayDate = list[0].TodayDate,
                            FutureUse = list[0].FutureUse,
                        };

                        try
                        {
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            ScrollViewDetails.IsVisible = false;

                            var httpClient = new HttpClient();
                            var json = JsonConvert.SerializeObject(signupClass);
                            HttpContent httpContent = new StringContent(json);
                            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", list[0]._ID), httpContent);

                            if (response.IsSuccessStatusCode)
                            {
                                DependencyService.Get<IMessage>().LongAlert("Updated");

                                HandleDB handle = new HandleDB();
                                LocalDB local = new LocalDB()
                                {
                                    Id = list[0]._ID,
                                    FullName = EntFullName.Text,
                                    CellNumber = list[0].CellNumber,
                                    City = CityValue,
                                    Area = AreaValue,
                                    BloodGroup = bloodgroup,
                                    Email = EntEmail.Text,
                                    Password = list[0].Password,
                                    TodayDate = list[0].TodayDate,
                                    FutureUse = list[0].FutureUse,
                                };
                                handle.UpdateUserDB(local);
                                await Navigation.PopAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                            ScrollViewDetails.IsVisible = true;
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

        private async void UpdateAccountClicked2(object sender, EventArgs e)
        {
            String email = EntEmail.Text;
            String emailpattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("You're offline!");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(AreaValue)
                || string.IsNullOrWhiteSpace(EntEmail.Text))
                {
                    DependencyService.Get<IMessage>().LongAlert("Fields are Empty!");
                }
                else
                {
                    if (!Regex.IsMatch(email, emailpattern))
                    {
                        DependencyService.Get<IMessage>().LongAlert("Incorrect Email Format");
                    }
                    else
                    {
                        if (bloodgroup == string.Empty)
                        {
                            bloodgroup = "No";
                        }

                        SignupClass signupClass = new SignupClass()
                        {
                            Id = data[0]._ID,
                            FullName = EntFullName.Text,
                            CellNumber = data[0].CellNumber,
                            City = CityValue,
                            Area = AreaValue,
                            BloodGroup = bloodgroup,
                            Email = EntEmail.Text,
                            Password = data[0].Password,
                            TodayDate = data[0].TodayDate,
                            FutureUse = data[0].FutureUse,
                        };

                        try
                        {
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;
                            ScrollViewDetails.IsVisible = false;

                            var httpClient = new HttpClient();
                            var json = JsonConvert.SerializeObject(signupClass);
                            HttpContent httpContent = new StringContent(json);
                            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", data[0]._ID), httpContent);

                            if (response.IsSuccessStatusCode)
                            {
                                DependencyService.Get<IMessage>().LongAlert("Updated");

                                HandleDB handle = new HandleDB();
                                LocalUsersDB local = new LocalUsersDB()
                                {
                                    Id = data[0].Id,
                                    _ID = data[0]._ID,
                                    FullName = EntFullName.Text,
                                    CellNumber = data[0].CellNumber,
                                    City = CityValue,
                                    Area = AreaValue,
                                    BloodGroup = bloodgroup,
                                    Email = EntEmail.Text,
                                    Password = data[0].Password,
                                    TodayDate = data[0].TodayDate,
                                    FutureUse = data[0].FutureUse,
                                };
                                handle.UpdateAllUserDB(local);
                                await Navigation.PopAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                            ScrollViewDetails.IsVisible = true;
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
