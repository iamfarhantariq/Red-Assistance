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
    public partial class AddDonorPage : ContentPage
    {
        private string CityValue;
        private string AreaValue;
        private string bloodgroup;
        private Label _previousLabel;
        private Label _currentLabel;
        private string Available = "Available";
        private int _did;
        private int did;
        private string dfullname;
        private string dcellnumber;
        private string dbloodgroup;
        private string darea;
        private string dcity;
        private string dtodaydate;
        private string daddedby;

        public AddDonorPage()
        {
            InitializeComponent();
        }

        public AddDonorPage(int _did, int did, string dfullname, string dcellnumber, string dbloodgroup, string darea, string dcity, string dtodaydate, string daddedby)
        {
            InitializeComponent();
            this._did = _did;
            this.did = did;
            this.dfullname = dfullname;
            this.dcellnumber = dcellnumber;
            this.dbloodgroup = dbloodgroup;
            this.darea = darea;
            this.dcity = dcity;
            this.dtodaydate = dtodaydate;
            this.daddedby = daddedby;
            EntFullName.Text = dfullname;
            EntCellNumber.Text = dcellnumber;
            BtnUpdateDonor.IsVisible = true;
            BtnAddDonor.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PickersItemsAdd();
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

        private void PkrAddDonorCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CityValue = PkrCity.Items[PkrCity.SelectedIndex];
        }

        private void PkrAddDonorArea_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AreaValue = PkrArea.Items[PkrArea.SelectedIndex];
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (AvlSwitch.IsToggled == true)
            {
                Available = "Available";
            }
            else
            {
                Available = "Not Available";
            }
        }

        private async void BtnAddDonor_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(EntCellNumber.Text)
                || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(AreaValue))
            {
                DependencyService.Get<IMessage>().LongAlert("Enter data to all feilds!");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(bloodgroup))
                {
                    DependencyService.Get<IMessage>().LongAlert("Select Blood Group!");
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
                            var usercell = Application.Current.Properties["UserCell"].ToString();
                            DateTime dateValue = DateTime.Now.ToLocalTime();

                            AddDonorClass addDonorClass = new AddDonorClass()
                            {
                                FullName = EntFullName.Text,
                                CellNumber = EntCellNumber.Text,
                                City = CityValue,
                                Area = AreaValue,
                                BloodGroup = bloodgroup,
                                AddedBy = usercell,
                                TodayDate = dateValue.ToString(),
                                FutureUse = Available
                            };
                            try
                            {
                                StackLayoutAddDonor.IsVisible = false;
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;

                                var httpClient = new HttpClient();
                                var json = JsonConvert.SerializeObject(addDonorClass);
                                HttpContent httpContent = new StringContent(json);
                                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                var response = await httpClient.PostAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi", httpContent);
                                if (response.StatusCode == HttpStatusCode.InternalServerError)
                                {
                                    await DisplayAlert("Donor Exist", "Donor with the number you added is already exist.", "OK");
                                }
                                else
                                {
                                    var httpClientG = new HttpClient();
                                    var responseG = await httpClientG.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi");
                                    var data = JsonConvert.DeserializeObject<List<AddDonorClass>>(responseG);

                                    var value = data.Where(c => c.CellNumber == EntCellNumber.Text).ToList();

                                    //new RefreshClass(3);
                                    LocalDonorsDB localDonors = new LocalDonorsDB()
                                    {
                                        _ID = value[0].ID,
                                        FullName = value[0].FullName,
                                        CellNumber = value[0].CellNumber,
                                        City = value[0].City,
                                        Area = value[0].Area,
                                        BloodGroup = value[0].BloodGroup,
                                        AddedBy = value[0].AddedBy,
                                        TodayDate = value[0].TodayDate,
                                        FutureUse = value[0].FutureUse
                                    };
                                    HandleDB handleDB = new HandleDB();
                                    handleDB.AddDonorsDB(localDonors);

                                    var httpClientMsg = new HttpClient();
                                    string cellnumber = EntCellNumber.Text.Substring(1).Insert(0, "92");
                                    string message = string.Format("Dear '{0}', you are added as a donor in Red Assistance app. Donate Blood to save a life. " +
                                        "Download the Red Assistance app: https://bit.ly/2JJnyWW", EntFullName.Text);
                                    // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", cellnumber, message), null);

                                    var Dcount = Application.Current.Properties["DonorsCount"];
                                    int AddedDcount = Convert.ToInt32(Dcount) + 1;
                                    Application.Current.Properties["DonorsCount"] = AddedDcount;

                                    var UDcount = Application.Current.Properties["UserDonors"];
                                    int AddedUDcount = Convert.ToInt32(UDcount) + 1;
                                    Application.Current.Properties["UserDonors"] = AddedUDcount;
                                    await Application.Current.SavePropertiesAsync();

                                    DependencyService.Get<IMessage>().ShortAlert("Added!");
                                    InitializeComponent();
                                }
                            }
                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                                StackLayoutAddDonor.IsVisible = true;
                            }
                            finally
                            {
                                StackLayoutAddDonor.IsVisible = true;
                                PickersItemsAdd();
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                            }
                        }
                    }
                }
            }
        }

        private async void BtnUpdateDonor_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(EntCellNumber.Text)
                || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(AreaValue))
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
                    if (string.IsNullOrWhiteSpace(bloodgroup))
                    {
                        DependencyService.Get<IMessage>().LongAlert("Select Blood Group!");
                    }
                    else
                    {
                        if (!CrossConnectivity.Current.IsConnected)
                        {
                            DependencyService.Get<IMessage>().LongAlert("Turn on network connection!");
                        }
                        else
                        {
                            AddDonorClass updatedonor = new AddDonorClass()
                            {
                                ID = did,
                                FullName = EntFullName.Text,
                                CellNumber = EntCellNumber.Text,
                                City = CityValue,
                                Area = AreaValue,
                                BloodGroup = bloodgroup,
                                AddedBy = daddedby,
                                TodayDate = dtodaydate,
                                FutureUse = Available,
                            };
                            try
                            {
                                StackLayoutAddDonor.IsVisible = false;
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;

                                var httpClient = new HttpClient();
                                var json = JsonConvert.SerializeObject(updatedonor);
                                HttpContent httpContent = new StringContent(json);
                                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi/{0}", did), httpContent);

                                if (response.StatusCode == HttpStatusCode.InternalServerError)
                                {
                                    await DisplayAlert("Update Error", "Donor with the number you added is already exist.", "OK");
                                }
                                else
                                {
                                    LocalDonorsDB updatelocaldonor = new LocalDonorsDB()
                                    {
                                        Id = _did,
                                        _ID = did,
                                        FullName = EntFullName.Text,
                                        CellNumber = EntCellNumber.Text,
                                        City = CityValue,
                                        Area = AreaValue,
                                        BloodGroup = bloodgroup,
                                        AddedBy = daddedby,
                                        TodayDate = dtodaydate,
                                        FutureUse = Available,
                                    };
                                    HandleDB handleDB = new HandleDB();
                                    handleDB.UpdateDonorDB(updatelocaldonor);

                                    DependencyService.Get<IMessage>().LongAlert("Updated");
                                    await Navigation.PopAsync();
                                }
                            }

                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                                StackLayoutAddDonor.IsVisible = true;

                            }
                            finally
                            {
                                StackLayoutAddDonor.IsVisible = true;
                                WaitingLoader.IsVisible = false;
                                WaitingLoader.IsRunning = false;

                            }
                        }
                    }

                }
            }
        }
    }
}