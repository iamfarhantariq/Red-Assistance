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
    public partial class AddRequestPage : ContentPage
    {
        private string CityValue;
        private string HospitalValue;
        private string bloodgroup;
        private Label _previousLabel;
        private Label _currentLabel;
        private string PickDrop = "No";
        private int _rid;
        private int rid;
        private string rfullname;
        private string rcellnumber;
        private string rbloodgroup;
        private string rhospital;
        private string rcity;
        private string rtodaydate;
        private string raddedby;

        public AddRequestPage()
        {
            InitializeComponent();
        }

        public AddRequestPage(int _rid, int rid, string rfullname, string rcellnumber, string rbloodgroup, string rhospital, string rcity, string rtodaydate, string raddedby)
        {
            InitializeComponent();
            this._rid = _rid;
            this.rid = rid;
            this.rfullname = rfullname;
            this.rcellnumber = rcellnumber;
            this.rbloodgroup = rbloodgroup;
            this.rhospital = rhospital;
            this.rcity = rcity;
            this.rtodaydate = rtodaydate;
            this.raddedby = raddedby;
            EntFullName.Text = rfullname;
            LblCellNumber.IsVisible = false;
            EntCellNumber.IsVisible = false;
            BtnUpdateRequest.IsVisible = true;
            BtnAddRequest.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PickersItemsAdd();
        }

        private void PickersItemsAdd()
        {
            PkrCity.Items.Add("Lahore");

            PkrHospital.Items.Add("Other");
            PkrHospital.Items.Add("Aadil Hospital");
            PkrHospital.Items.Add("Al-Khidmat Hospital, Multan Road");
            PkrHospital.Items.Add("Ali eye hospital");
            PkrHospital.Items.Add("Arif Memorial Teaching Hospital");
            PkrHospital.Items.Add("Avicenna Hospital");
            PkrHospital.Items.Add("Bahria Town Hospital");
            PkrHospital.Items.Add("Bajwa Hospital, Iqbal Town");
            PkrHospital.Items.Add("Chaudhary Muhammad Akram Teaching & Research Hospital 17 km Raiwind road,Lahore");
            PkrHospital.Items.Add("City Hospital Mohlanwal Lahore");
            PkrHospital.Items.Add("Combined Military Hospital");
            PkrHospital.Items.Add("Doctors Hospital");
            PkrHospital.Items.Add("Family Hospital");
            PkrHospital.Items.Add("Farooq Hospital");
            PkrHospital.Items.Add("Fatima Memorial Hospital");
            PkrHospital.Items.Add("Fauji Foundation Hospital");
            PkrHospital.Items.Add("Ganj Baksh Spinal Research & Rehabilitation Hospital");
            PkrHospital.Items.Add("Ghurki Trust Hospital");
            PkrHospital.Items.Add("Gulab Devi Chest Hospital");
            PkrHospital.Items.Add("Gulberg Hospital");
            PkrHospital.Items.Add("Haleema Memorial Foundation Hospital");
            PkrHospital.Items.Add("Hameed Latif Hospital");
            PkrHospital.Items.Add("Hijaz Hospital");
            PkrHospital.Items.Add("Ittefaq Hospital");
            PkrHospital.Items.Add("Janki Devi Hospital");
            PkrHospital.Items.Add("Jinnah Hospital");
            PkrHospital.Items.Add("Lady Aitchison Hospital");
            PkrHospital.Items.Add("Lady Willingdon Hospital");
            PkrHospital.Items.Add("Lahore General Hospital");
            PkrHospital.Items.Add("Makhdoom Hospital");
            PkrHospital.Items.Add("Masood Hospital");
            PkrHospital.Items.Add("Mayo Hospital");
            PkrHospital.Items.Add("Mid City Hospital");
            PkrHospital.Items.Add("Mumtaz Bukhtawar Memorial Trust Hospital Raiwind Road,Lahore. Unit 2");
            PkrHospital.Items.Add("Mumtaz Bukhtawar Memorial Trust Hospital Wahdat Road, Lahore.Unit 1");
            PkrHospital.Items.Add("National Hospital");
            PkrHospital.Items.Add("Nawaz Sharif Social Security Hospital, Multan Road");
            PkrHospital.Items.Add("OMC, Jail Road");
            PkrHospital.Items.Add("Omar Hospital, Jail Road");
            PkrHospital.Items.Add("Prime Care Hospital");
            PkrHospital.Items.Add("Punjab Institute of Cardiology");
            PkrHospital.Items.Add("Punjab Institute of Mental Health");
            PkrHospital.Items.Add("Punjab Social Security Hospital");
            PkrHospital.Items.Add("Railway Karen Hospital");
            PkrHospital.Items.Add("Ramzan Ali Memorial Hospital");
            PkrHospital.Items.Add("Salma Sarfraz Hospital");
            PkrHospital.Items.Add("Services Hospital");
            PkrHospital.Items.Add("Shaikh Zayed Hospital");
            PkrHospital.Items.Add("Shalamar Hospital (Shalamar Institute of Health Sciences)");
            PkrHospital.Items.Add("Shaukat Khanum Memorial Cancer Hospital & Research Centre");
            PkrHospital.Items.Add("Sir Ganga Ram Hospital");
            PkrHospital.Items.Add("Surgimed Hospital");
            PkrHospital.Items.Add("Surraya Azeem Hospital");
            PkrHospital.Items.Add("The Children's Hospital");
            PkrHospital.Items.Add("University Dental Hospital - University of Lahore");
            PkrHospital.Items.Add("University Teaching Hospital - University of Lahore");
            PkrHospital.Items.Add("Wapda Hospital");
            PkrHospital.Items.Add("Zahida Welfare hospital");
            PkrHospital.Items.Add("Zia Hospital, Ferozpur Road");
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

        private void PkrAddRequestCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CityValue = PkrCity.Items[PkrCity.SelectedIndex];
        }

        private void PkrAddRequestHospital_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            HospitalValue = PkrHospital.Items[PkrHospital.SelectedIndex];
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (AvlSwitch.IsToggled == true)
            {
                PickDrop = "Yes";
            }
            else
            {
                PickDrop = "No";
            }
        }

        private async void BtnAddRequest_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(EntCellNumber.Text)
                || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(HospitalValue))
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

                            AddRequestClass addRequestClass = new AddRequestClass()
                            {
                                FullName = EntFullName.Text,
                                CellNumber = EntCellNumber.Text,
                                City = CityValue,
                                Hospitals = HospitalValue,
                                BloodGroup = bloodgroup,
                                AddedBy = usercell,
                                TodayDate = dateValue.ToString(),
                                FutureUse = PickDrop
                            };
                            try
                            {
                                StackLayoutAddRequest.IsVisible = false;
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;

                                var httpClientR = new HttpClient();
                                var responseR = await httpClientR.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi");
                                var valuesR = JsonConvert.DeserializeObject<List<AddRequestClass>>(responseR);

                                Application.Current.Properties["RequestsCount"] = valuesR.Count().ToString();
                                await Application.Current.SavePropertiesAsync();

                                HandleDB handleDB = new HandleDB();
                                LocalRequestsDB localRequestsDB = new LocalRequestsDB();
                                handleDB.DeleteRequestsDB(localRequestsDB);

                                for (int i = 0; i < 35; i++)
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
                                var requestlocaldata = handleDB.GetRequestsDB(EntCellNumber.Text).ToList();

                                // Fresh Request
                                if (requestlocaldata.Count() == 0)
                                {
                                    var httpClient = new HttpClient();
                                    var json = JsonConvert.SerializeObject(addRequestClass);
                                    HttpContent httpContent = new StringContent(json);
                                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                    var response = await httpClient.PostAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi", httpContent);
                                    if (response.StatusCode == HttpStatusCode.Created)
                                    {
                                        var httpClientG = new HttpClient();
                                        var responseG = await httpClientG.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi?City=Lahore&CellNumber=" + EntCellNumber.Text);
                                        var value = JsonConvert.DeserializeObject<List<AddRequestClass>>(responseG);

                                        LocalRequestsDB localRequests = new LocalRequestsDB()
                                        {
                                            _ID = value[0].ID,
                                            FullName = value[0].FullName,
                                            CellNumber = value[0].CellNumber,
                                            City = value[0].City,
                                            Hospitals = value[0].Hospitals,
                                            BloodGroup = value[0].BloodGroup,
                                            AddedBy = value[0].AddedBy,
                                            TodayDate = value[0].TodayDate,
                                            FutureUse = value[0].FutureUse
                                        };
                                        handleDB.AddRequestsDB(localRequests);

                                        #region MessageSection
                                        var data = handleDB.RandomDonorsCellNumber().ToList();

                                        string numbers = string.Empty;
                                        string _bloodgroup = string.Empty;
                                        int i = 0;
                                        foreach (var v in data)
                                        {
                                            string cellnumber = data[i].CellNumber.Substring(1).Insert(0, "92");
                                            cellnumber = cellnumber.Insert(cellnumber.Length, ",");
                                            numbers = numbers.Insert(numbers.Length, cellnumber);
                                            i++;

                                            if (i == 20 || data.Count == i)
                                            {
                                                numbers = numbers.Remove(numbers.Length - 1);
                                                var httpClientMsg = new HttpClient();
                                                switch (bloodgroup)
                                                {
                                                    case "A+":
                                                        _bloodgroup = "A%2B";
                                                        break;

                                                    case "A-":
                                                        _bloodgroup = "A%2D";
                                                        break;

                                                    case "B+":
                                                        _bloodgroup = "B%2B";
                                                        break;

                                                    case "B-":
                                                        _bloodgroup = "B%2D";
                                                        break;

                                                    case "AB+":
                                                        _bloodgroup = "AB%2B";
                                                        break;

                                                    case "AB-":
                                                        _bloodgroup = "AB%2D";
                                                        break;

                                                    case "O+":
                                                        _bloodgroup = "O%2B";
                                                        break;

                                                    case "O-":
                                                        _bloodgroup = "O%2D";
                                                        break;
                                                }
                                                string message = string.Format("'{0}' Blood is required at {1} {2}. Contact at '{3}'. " +
                                                    "Download the Red Assistance app: https://bit.ly/2JJnyWW", _bloodgroup, HospitalValue, CityValue, EntCellNumber.Text);
                                                 await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbers, message), null);
                                                break;
                                            }
                                        };

                                        SendMessagesClass.SendMessagetoHeads(bloodgroup, HospitalValue, CityValue, EntCellNumber.Text);
                                        #endregion

                                        var URcount = Application.Current.Properties["UserRequests"];
                                        int AddedURcount = Convert.ToInt32(URcount) + 1;
                                        Application.Current.Properties["UserRequests"] = AddedURcount;

                                        var Rcount = Application.Current.Properties["RequestsCount"];
                                        int AddedRcount = Convert.ToInt32(Rcount) + 1;
                                        Application.Current.Properties["RequestsCount"] = AddedRcount;

                                        await Application.Current.SavePropertiesAsync();

                                        DependencyService.Get<IMessage>().ShortAlert("Added!");
                                        InitializeComponent();
                                    }
                                }
                                else
                                {
                                    DateTime dateTimeold = Convert.ToDateTime(requestlocaldata[0].TodayDate);
                                    var datedifference = dateValue.Subtract(dateTimeold).TotalHours;
                                    if (datedifference > 24)
                                    {
                                        var httpClientDelete = new HttpClient();
                                        await httpClientDelete.DeleteAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi/{0}", requestlocaldata[0]._ID));

                                        HandleDB handle = new HandleDB();
                                        handle.DeleteRequestsDB(requestlocaldata[0].Id);

                                        #region Request
                                        var httpClient = new HttpClient();
                                        var json = JsonConvert.SerializeObject(addRequestClass);
                                        HttpContent httpContent = new StringContent(json);
                                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                        var response = await httpClient.PostAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi", httpContent);
                                        if (response.StatusCode == HttpStatusCode.Created)
                                        {
                                            var httpClientG = new HttpClient();
                                            var responseG = await httpClientG.GetStringAsync("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi?City=Lahore&CellNumber=" + EntCellNumber.Text);
                                            var value = JsonConvert.DeserializeObject<List<AddRequestClass>>(responseG);

                                            LocalRequestsDB localRequests = new LocalRequestsDB()
                                            {
                                                _ID = value[0].ID,
                                                FullName = value[0].FullName,
                                                CellNumber = value[0].CellNumber,
                                                City = value[0].City,
                                                Hospitals = value[0].Hospitals,
                                                BloodGroup = value[0].BloodGroup,
                                                AddedBy = value[0].AddedBy,
                                                TodayDate = value[0].TodayDate,
                                                FutureUse = value[0].FutureUse
                                            };
                                            handleDB.AddRequestsDB(localRequests);

                                            var data = handleDB.RandomDonorsCellNumber().ToList();

                                            string numbers = string.Empty;
                                            string _bloodgroup = string.Empty;
                                            int i = 0;
                                            foreach (var v in data)
                                            {
                                                string cellnumber = data[i].CellNumber.Substring(1).Insert(0, "92");
                                                cellnumber = cellnumber.Insert(cellnumber.Length, ",");
                                                numbers = numbers.Insert(numbers.Length, cellnumber);
                                                i++;

                                                if (i == 20 || data.Count == i)
                                                {
                                                    numbers = numbers.Remove(numbers.Length - 1);
                                                    var httpClientMsg = new HttpClient();
                                                    switch (bloodgroup)
                                                    {
                                                        case "A+":
                                                            _bloodgroup = "A%2B";
                                                            break;

                                                        case "A-":
                                                            _bloodgroup = "A%2D";
                                                            break;

                                                        case "B+":
                                                            _bloodgroup = "B%2B";
                                                            break;

                                                        case "B-":
                                                            _bloodgroup = "B%2D";
                                                            break;

                                                        case "AB+":
                                                            _bloodgroup = "AB%2B";
                                                            break;

                                                        case "AB-":
                                                            _bloodgroup = "AB%2D";
                                                            break;

                                                        case "O+":
                                                            _bloodgroup = "O%2B";
                                                            break;

                                                        case "O-":
                                                            _bloodgroup = "O%2D";
                                                            break;
                                                    }
                                                    string message = string.Format("'{0}' Blood is required at {1} {2}. Contact at '{3}'. " +
                                                        "Download the Red Assistance app: https://bit.ly/2JJnyWW", _bloodgroup, HospitalValue, CityValue, EntCellNumber.Text);
                                                      await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbers, message), null);
                                                    break;
                                                }
                                            };

                                            SendMessagesClass.SendMessagetoHeads(bloodgroup, HospitalValue, CityValue, EntCellNumber.Text);

                                            var URcount = Application.Current.Properties["UserRequests"];
                                            int AddedURcount = Convert.ToInt32(URcount) + 1;
                                            Application.Current.Properties["UserRequests"] = AddedURcount;

                                            var Rcount = Application.Current.Properties["RequestsCount"];
                                            int AddedRcount = Convert.ToInt32(Rcount) - 1;
                                            Application.Current.Properties["RequestsCount"] = AddedRcount;

                                            await Application.Current.SavePropertiesAsync();

                                            DependencyService.Get<IMessage>().ShortAlert("Added!");
                                            InitializeComponent();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        await DisplayAlert("Request Exist", "Your Blood Request with " + EntCellNumber.Text + " exist. You can add another after 24hrs", "OK");
                                        InitializeComponent();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                                StackLayoutAddRequest.IsVisible = true;
                            }
                            finally
                            {
                                InitializeComponent();
                                PickersItemsAdd();
                                StackLayoutAddRequest.IsVisible = true;
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                            }
                        }
                    }
                }
            }
        }

        private async void BtnUpdateRequest_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (string.IsNullOrWhiteSpace(EntFullName.Text) || string.IsNullOrWhiteSpace(CityValue) || string.IsNullOrWhiteSpace(HospitalValue))
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
                            AddRequestClass updaterequest = new AddRequestClass()
                            {
                                ID = rid,
                                FullName = EntFullName.Text,
                                CellNumber = rcellnumber,
                                City = CityValue,
                                Hospitals = HospitalValue,
                                BloodGroup = bloodgroup,
                                AddedBy = raddedby,
                                TodayDate = rtodaydate,
                                FutureUse = PickDrop,
                            };
                            try
                            {
                                StackLayoutAddRequest.IsVisible = false;
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;

                                var httpClient = new HttpClient();
                                var json = JsonConvert.SerializeObject(updaterequest);
                                HttpContent httpContent = new StringContent(json);
                                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                var response = await httpClient.PutAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi/{0}", rid), httpContent);

                                if (response.IsSuccessStatusCode)
                                {
                                    LocalRequestsDB updatelocalrequest = new LocalRequestsDB()
                                    {
                                        Id = _rid,
                                        _ID = rid,
                                        FullName = EntFullName.Text,
                                        CellNumber = rcellnumber,
                                        City = CityValue,
                                        Hospitals = HospitalValue,
                                        BloodGroup = bloodgroup,
                                        AddedBy = raddedby,
                                        TodayDate = rtodaydate,
                                        FutureUse = PickDrop,
                                    };
                                    HandleDB handleDB = new HandleDB();
                                    handleDB.UpdateRequestDB(updatelocalrequest);
                                    DependencyService.Get<IMessage>().ShortAlert("Updated");
                                    await Navigation.PopAsync();
                                }
                            }

                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                                StackLayoutAddRequest.IsVisible = true;
                            }
                            finally
                            {
                                StackLayoutAddRequest.IsVisible = true;
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