using Plugin.Connectivity;
using Plugin.Messaging;
using Plugin.Share;
using Plugin.Share.Abstractions;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminListviewTappedPage : ContentPage
    {
        private int tid;
        private int _tid;
        private string tusername;
        private string trole;
        private string tfrom;
        private string tcellnumber;
        private string taddedby;
        private int did;
        private int _did;
        private string dfullname;
        private string dcellnumber;
        private string dbloodgroup;
        private string darea;
        private string dcity;
        private string dtodaydate;
        private string daddedby;
        private int val;
        private int rid;
        private int _rid;
        private string rfullname;
        private string rcellnumber;
        private string rbloodgroup;
        private string rhospital;
        private string rcity;
        private string rtodaydate;
        private string raddedby;
        private int uid;
        private int _uid;
        private string ufullname;
        private string ucellnumber;
        private string ubloodgroup;
        private string uarea;
        private string ucity;
        private string uemail;
        private string utodaydate;
        private string ufutureuse;
        private string role;

        public AdminListviewTappedPage()
        {
            InitializeComponent();
        }

        public AdminListviewTappedPage(int _tid, int tid, string tusername, string trole, string tfrom, string tcellnumber, string taddedby)
        {
            InitializeComponent();
            this.tid = tid;
            this._tid = _tid;
            this.tusername = tusername;
            this.trole = trole;
            this.tfrom = tfrom;
            this.tcellnumber = tcellnumber;
            this.taddedby = taddedby;
            TLblName.Text = tusername;
            TLblRole.Text = trole;
            TLblFrom.Text = tfrom;
            TLblCellNumber.Text = tcellnumber;
            TLblAddedBy.Text = taddedby;
            LayoutSelection(0);
            Title = "Member Detail";
        }

        public AdminListviewTappedPage(int _did, int did, string dfullname, string dcellnumber, string dbloodgroup, string darea, string dcity, string dtodaydate, string daddedby)
        {
            InitializeComponent();
            this.did = did;
            this._did = _did;
            this.dfullname = dfullname;
            this.dcellnumber = dcellnumber;
            this.dbloodgroup = dbloodgroup;
            this.darea = darea;
            this.dcity = dcity;
            this.dtodaydate = dtodaydate;
            this.daddedby = daddedby;
            DLblName.Text = dfullname;
            DLblCell.Text = dcellnumber;
            DLblBlood.Text = dbloodgroup;
            DLblCity.Text = dcity;
            DLblArea.Text = darea;
            DLblAddedby.Text = daddedby;
            DLblTodayDate.Text = dtodaydate;
            LayoutSelection(1);
            Title = "Donor Detail";
        }

        public AdminListviewTappedPage(int _rid, int val, int rid, string rfullname, string rcellnumber, string rbloodgroup, string rhospital, string rcity, string rtodaydate, string raddedby)
        {
            InitializeComponent();
            this.val = val;
            this.rid = rid;
            this._rid = _rid;
            this.rfullname = rfullname;
            this.rcellnumber = rcellnumber;
            this.rbloodgroup = rbloodgroup;
            this.rhospital = rhospital;
            this.rcity = rcity;
            this.rtodaydate = rtodaydate;
            this.raddedby = raddedby;
            RLblName.Text = rfullname;
            RLblCell.Text = rcellnumber;
            rLblBlood.Text = rbloodgroup;
            RLblHospital.Text = rhospital;
            RLblCity.Text = rcity;
            RLblAddedby.Text = raddedby;
            RLblTodayDate.Text = rtodaydate;
            LayoutSelection(2);
            Title = "Blood Request Detail";
        }

        public AdminListviewTappedPage(int _uid, int uid, string ufullname, string ucellnumber, string ucity, string uarea, string ubloodgroup, string uemail, string utodaydate, string ufutureuse, int uextra)
        {
            InitializeComponent();
            this.uid = uid;
            this._uid = _uid;
            this.ufullname = ufullname;
            this.ucellnumber = ucellnumber;
            this.ucity = ucity;
            this.uarea = uarea;
            this.ubloodgroup = ubloodgroup;
            this.uemail = uemail;
            this.utodaydate = utodaydate;
            this.ufutureuse = ufutureuse;
            ULblName.Text = ufullname;
            ULblCell.Text = ucellnumber.ToString();
            ULblBlood.Text = ubloodgroup;
            ULblArea.Text = uarea;
            ULblCity.Text = ucity;
            ULblEmail.Text = uemail;
            ULblAddedby.Text = ufutureuse;
            ULblTodayDate.Text = utodaydate;
            LayoutSelection(3);
            Title = "User Detail";
        }

        private void LayoutSelection(int v)
        {
            role = (string)Application.Current.Properties["AdminRole"];
            StackLayout[] DataStacks = new StackLayout[] { TeamStack, DonorStack, RequestStack, UserStack };
            for (int i = 0; i < 4; i++)
            {
                DataStacks[i].IsVisible = false;
            }
            DataStacks[v].IsVisible = true;
        }

        #region TeamTapped
        private void TCallButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = tcellnumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }

        private void TMessageButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = tcellnumber;
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(phonenumber, "\n\n-Red Assistance");
        }

        private async void TDeleteLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Head" || role == "Admin, Developer")
            {
                var ans = await DisplayAlert("Delete", "Do you want to Delete this Member? ", "Delete", "Cancel");
                if (ans == true)
                {
                    HandleDB handleDB = new HandleDB();
                    if (trole == "Admin, Developer")
                    {
                        DependencyService.Get<IMessage>().LongAlert("You cannot delete this entry!");
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
                                TeamStack.IsVisible = false;
                                WaitingLoader.IsRunning = true;
                                WaitingLoader.IsVisible = true;

                                var httpClient = new HttpClient();
                                var response = httpClient.DeleteAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/AdminConsoleApi/{0}", tid));

                                handleDB.DeleteAdminsDB(_tid);

                                DependencyService.Get<IMessage>().LongAlert("Deleted");
                                await Navigation.PopAsync();

                            }
                            catch (Exception ex)
                            {
                                WaitingLoader.IsRunning = false;
                                WaitingLoader.IsVisible = false;
                                TeamStack.IsVisible = true;
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
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Admin or Head can delete Team!");
            }
        }

        private void TEditLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Head" || role == "Admin, Developer")
            {

            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Admin or Head can delete Team!");
            }
        }

        private async void TShareLabel_Tapped(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = tusername + " (" + trole + ") " + tcellnumber +
            "\nFor more detail download the app",
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
        #endregion

        #region DonorTapped
        private void DCallButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = dcellnumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }

        private void DMessageButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = dcellnumber;
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(phonenumber, "\n\n-Red Assistance");
        }

        private async void DDeleteLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "CR")
            {
                DependencyService.Get<IMessage>().ShortAlert("You don't have permission to delete it");
            }
            else
            {
                var ans = await DisplayAlert("Delete", "Do you want to Delete this Donor? ", "Delete", "Cancel");
                if (ans == true)
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().LongAlert("You're offline!");
                    }
                    else
                    {
                        try
                        {
                            DonorStack.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;

                            var httpClient = new HttpClient();
                            var response = httpClient.DeleteAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/DonorsApi/{0}", did));

                            HandleDB handleDB = new HandleDB();
                            handleDB.DeleteDonorsDB(_did);

                            var Dcount = Application.Current.Properties["DonorsCount"];
                            int AddedDcount = Convert.ToInt32(Dcount) - 1;
                            Application.Current.Properties["DonorsCount"] = AddedDcount;

                            var UDcount = Application.Current.Properties["UserDonors"];
                            int AddedUDcount = Convert.ToInt32(UDcount) - 1;
                            Application.Current.Properties["UserDonors"] = AddedUDcount;
                            await Application.Current.SavePropertiesAsync();

                            DependencyService.Get<IMessage>().LongAlert("Deleted");
                            await Navigation.PopAsync();

                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            DonorStack.IsVisible = true;
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

        private void DEditLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "CR")
            {
                DependencyService.Get<IMessage>().ShortAlert("You cannot Edit");
            }
            else
            {
                Navigation.PushAsync(new AddDonorPage(_did, did, dfullname, dcellnumber, dbloodgroup, darea, dcity, dtodaydate, daddedby));
            }
        }

        private async void DShareLabel_Tapped(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = dfullname + " (" + dbloodgroup + ") " + dcellnumber +
            "\nFor more detail download the app",
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
        #endregion

        #region RequestTapped
        private void RCallButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = rcellnumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }

        private void RMessageButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = rcellnumber;
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(phonenumber, "\n\n-Red Assistance");
        }

        private async void RDeleteLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "CR")
            {
                DependencyService.Get<IMessage>().ShortAlert("You don't have permission to delete it");
            }
            else
            {
                var ans = await DisplayAlert("Delete", "Do you want to Delete this Request? ", "Delete", "Cancel");
                if (ans == true)
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().LongAlert("You're offline!");
                    }
                    else
                    {
                        try
                        {
                            RequestStack.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;

                            var httpClient = new HttpClient();
                            var response = httpClient.DeleteAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/RequestApi/{0}", rid));

                            HandleDB handleDB = new HandleDB();
                            handleDB.DeleteRequestsDB(_rid);


                            var Rcount = Application.Current.Properties["RequestsCount"];
                            int AddedRcount = Convert.ToInt32(Rcount) - 1;
                            Application.Current.Properties["RequestsCount"] = AddedRcount;

                            var URcount = Application.Current.Properties["UserRequests"];
                            int AddedURcount = Convert.ToInt32(URcount) - 1;
                            Application.Current.Properties["UserRequests"] = AddedURcount;
                            await Application.Current.SavePropertiesAsync();


                            DependencyService.Get<IMessage>().LongAlert("Deleted");
                            await Navigation.PopAsync();

                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            RequestStack.IsVisible = true;
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

        private void REditLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "CR")
            {
                DependencyService.Get<IMessage>().ShortAlert("You cannot Edit");
            }
            else
            {
                Navigation.PushAsync(new AddRequestPage(_rid, rid, rfullname, rcellnumber, rbloodgroup, rhospital, rcity, rtodaydate, raddedby));
            }
        }

        private async void RShareLabel_Tapped(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = rfullname + " (" + rbloodgroup + ") " + rcellnumber +
            "\nFor more detail download the app",
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
        #endregion

        #region UserTapped
        private void UCallButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = ucellnumber.ToString();
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }

        private void UMessageButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = ucellnumber.ToString();
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(phonenumber, "\n\n-Red Assistance");
        }

        private async void UDeleteLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Head" || role == "Admin, Developer")
            {
                var ans = await DisplayAlert("Delete", "Do you want to Delete this User? ", "Delete", "Cancel");
                if (ans == true)
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        DependencyService.Get<IMessage>().LongAlert("You're offline!");
                    }
                    else
                    {
                        try
                        {
                            UserStack.IsVisible = false;
                            WaitingLoader.IsRunning = true;
                            WaitingLoader.IsVisible = true;

                            var httpClient = new HttpClient();
                            var response = httpClient.DeleteAsync(String.Format("http://blooddonationlahoreapp.azurewebsites.net/api/BloodUsersApi/{0}", uid));

                            HandleDB handleDB = new HandleDB();
                            handleDB.DeleteUserDB(_uid);

                            DependencyService.Get<IMessage>().LongAlert("Deleted");
                            await Navigation.PopAsync();

                        }
                        catch (Exception ex)
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                            UserStack.IsVisible = true;
                            DependencyService.Get<IMessage>().LongAlert(ex.ToString());
                        }
                        finally
                        {
                            WaitingLoader.IsRunning = false;
                            WaitingLoader.IsVisible = false;
                        }
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Admin or Head can Delete Users!");
                }
            }
        }

        private void UEditLabel_Tapped(object sender, EventArgs e)
        {
            if (role == "Admin" || role == "Admin, Developer")
            {
                HandleDB dB = new HandleDB();
                var data = dB.GetAllUserInfoDB().Where(x => x._ID == uid).ToList();
                Navigation.PushAsync(new RegisterUser(data, 1));
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Admin can Edit Users!");
            }

        }

        private async void UShareLabel_Tapped(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = ufullname + " (" + ubloodgroup + ") " + ucellnumber +
            "\nFor more detail download the app",
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
        #endregion
    }
}