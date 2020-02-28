using Plugin.Messaging;
using Plugin.Share;
using Plugin.Share.Abstractions;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemSelectedPage : ContentPage
    {
        private int _val;
        private string fullname;
        private string cellnumber;
        private string bloodgroup;
        private string area;
        private string city;
        private string todaydate;
        private string addedby;

        public ItemSelectedPage()
        {
            InitializeComponent();
        }

        public ItemSelectedPage(int _val, string fullname, string cellnumber, string bloodgroup, string area, string city, string todaydate, string addedby)
        {
            InitializeComponent();
            this._val = _val;
            this.fullname = fullname;
            this.cellnumber = cellnumber;
            this.bloodgroup = bloodgroup;
            this.area = area;
            this.city = city;
            this.todaydate = todaydate;
            this.addedby = addedby;

            Title = fullname;
            if (_val == 1)
            {
                LblAreaText.IsVisible = true;
                LbHospitalText.IsVisible = false;
            }
            else if (_val == 2)
            {
                LblAreaText.IsVisible = false;
                LbHospitalText.IsVisible = true;
            }
            ShowData();
        }

        private void ShowData()
        {
            LblName.Text = fullname;
            LblCell.Text = cellnumber;
            LblBlood.Text = bloodgroup;
            LblArea.Text = area;
            LblCity.Text = city;
            LblAddedby.Text = addedby;
            LblTodayDate.Text = todaydate;
        }

        private void CallButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = cellnumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }

        private void MessageButtonClicked(object sender, EventArgs e)
        {
            string phonenumber = cellnumber;
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
                smsMessenger.SendSms(phonenumber, "\n\n-Red Assistance");
        }

        private async void ShareLabel_Tapped(object sender, EventArgs e)
        {
            await CrossShare.Current.Share(new ShareMessage
            {
                Text = fullname + " (" + bloodgroup + ") " + cellnumber +
            "\nFor more detail download the app",
                Title = "Red Assistance",
                Url = "https://bit.ly/2JJnyWW"
            });
        }
    }
}