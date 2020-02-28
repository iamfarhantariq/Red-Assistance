using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Messaging;
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
    public partial class TeamPage : ContentPage
    {
        public TeamPage()
        {
            InitializeComponent();
            GetContacts();
        }

        private void GetContacts()
        {
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetAdminDB().ToList();
            LvContactus.ItemsSource = data;
        }

        private void LvContactus_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            string phonenumber = ((AdminClass)LvContactus.SelectedItem).CellNumber;
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall(phonenumber);
        }
    }
}