using Plugin.Connectivity;
using RedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedAssistance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendMessagePage : ContentPage
    {
        private string role;

        public SendMessagePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PickersItemsAdd();
        }
        private void PickersItemsAdd()
        {
            PkrRole.Items.Add("Enter Number");
            PkrRole.Items.Add("All");
            PkrRole.Items.Add("Patients");
            PkrRole.Items.Add("Donors");
            PkrRole.Items.Add("Users");
            PkrRole.Items.Add("Team");
        }
        private void PkrRoleSelectedIndexChanged(object sender, EventArgs e)
        {
            role = PkrRole.Items[PkrRole.SelectedIndex];
        }
        private async void BtnSendMessage_OnClicked(object sender, EventArgs e)
        {
            String phone = EntCellNumber.Text;
            String phonepattern = "^((\\+92-?)|0)?[0-9]{10}$";

            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IMessage>().LongAlert("Turn on network connection!");
            }
            else
            {
                try
                {
                    DependencyService.Get<IMessage>().ShortAlert("Sending");
                    StackLayoutAddDonor.IsVisible = false;
                    WaitingLoader.IsRunning = true;
                    WaitingLoader.IsVisible = true;

                    switch (role)
                    {
                        case "Cancel":
                            break;

                        case "Enter Number":
                            EntCellNumber.IsVisible = true;
                            if (string.IsNullOrWhiteSpace(EntCellNumber.Text))
                            {
                                DependencyService.Get<IMessage>().LongAlert("Enter Cell Number");
                            }
                            else
                            {
                                if (!Regex.IsMatch(phone, phonepattern))
                                {
                                    DependencyService.Get<IMessage>().LongAlert("Incorrect phone number");
                                }
                                else
                                {
                                    var httpClientMsg = new HttpClient();
                                    string cellnumber = EntCellNumber.Text.Substring(1).Insert(0, "92");
                                    string message = EditorMessage.Text +
                                    "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                                    await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", cellnumber, message), null);
                                }
                            }
                            break;

                        case "All":
                            HandleDB team = new HandleDB();
                            var tdata = team.GetAdminDB().ToList();
                            var ddata = team.GetDonorsDB().ToList();
                            var rdata = team.GetRequestsDB().ToList();
                            var udata = team.GetAllUserInfoDB().ToList();
                            List<string> cellNumz = new List<string>();
                            foreach (var item in tdata)
                            {
                                cellNumz.Add(item.CellNumber);
                            }
                            foreach (var item in ddata)
                            {
                                cellNumz.Add(item.CellNumber);
                            }
                            foreach (var item in rdata)
                            {
                                cellNumz.Add(item.CellNumber);
                            }
                            foreach (var item in udata)
                            {
                                cellNumz.Add(item.CellNumber);
                            }
                           // var data = (tdata.Union(ddata).Union(rdata).Union(udata)).Tolist();
                            //for (int i = 0; i < 4; i++)
                            //{
                            //    var data = handles[i];
                            //    string numbers = string.Empty;

                            //    for (int a = 0; a < 4; a++)
                            //    {
                            //        string cellnumber = data[a].CellNumber.Substring(1).Insert(0, "92");
                            //        cellnumber = cellnumber.Insert(cellnumber.Length, ",");
                            //        numbersr = numbersr.Insert(numbersr.Length, cellnumber);
                            //       

                            //        numbersr = numbersr.Remove(numbersr.Length - 1);
                            //        var httpClientMsg = new HttpClient();

                            //        var message = "Dear " + dataR[r].CapName + ", " + EditorMessage.Text +
                            //        "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                            //        // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbers, message), null);
                            // r++;       
                            // break;
                            //    };
                            //}
                            break;

                        case "Patients":
                            HandleDB requestsdb = new HandleDB();
                            var dataR = requestsdb.GetDonorsDB().ToList();
                            string numbersr = string.Empty;
                            int r = 0;
                            foreach (var v in dataR)
                            {
                                string cellnumber = dataR[r].CellNumber.Substring(1).Insert(0, "92");
                                cellnumber = cellnumber.Insert(cellnumber.Length, ",");
                                numbersr = numbersr.Insert(numbersr.Length, cellnumber);

                                numbersr = numbersr.Remove(numbersr.Length - 1);
                                var httpClientMsg = new HttpClient();

                                var message = "Dear " + dataR[r].CapName + ", " + EditorMessage.Text +
                                "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                                // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbersr, message), null);
                                r++;
                                break;
                            };
                            break;

                        case "Donors":
                            HandleDB donorsdb = new HandleDB();
                            var dataD = donorsdb.GetDonorsDB().ToList();
                            string numbersd = string.Empty;
                            int d = 0;
                            foreach (var v in dataD)
                            {
                                string cellnumber = dataD[d].CellNumber.Substring(1).Insert(0, "92");
                                cellnumber = cellnumber.Insert(cellnumber.Length, ",");
                                numbersd = numbersd.Insert(numbersd.Length, cellnumber);

                                numbersd = numbersd.Remove(numbersd.Length - 1);
                                var httpClientMsg = new HttpClient();

                                var message = "Dear " + dataD[d].CapName + ", " + EditorMessage.Text +
                                "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                                // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbersd, message), null);
                                d++;
                                break;
                            };
                            break;

                        case "Users":
                            HandleDB userdb = new HandleDB();
                            var dataU = userdb.GetAdminDB().ToList();
                            string numbersu = string.Empty;
                            int u = 0;
                            foreach (var v in dataU)
                            {
                                string cellnumberu = dataU[u].CellNumber.Substring(1).Insert(0, "92");
                                cellnumberu = cellnumberu.Insert(cellnumberu.Length, ",");
                                numbersu = numbersu.Insert(numbersu.Length, cellnumberu);

                                numbersu = numbersu.Remove(numbersu.Length - 1);
                                var httpClientMsg = new HttpClient();

                                var message = "Dear " + dataU[u].UserName + ", " + EditorMessage.Text +
                                "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                                // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbersu, message), null);
                                u++;
                                break;
                            };
                            break;

                        case "Team":
                            HandleDB teamdb = new HandleDB();
                            var dataT = teamdb.GetAdminDB().ToList();
                            string numberst = string.Empty;
                            int t = 0;
                            foreach (var v in dataT)
                            {
                                string cellnumbert = dataT[t].CellNumber.Substring(1).Insert(0, "92");
                                cellnumbert = cellnumbert.Insert(cellnumbert.Length, ",");
                                numberst = numberst.Insert(numberst.Length, cellnumbert);

                                numberst = numberst.Remove(numberst.Length - 1);
                                var httpClientMsg = new HttpClient();

                                var message = "Dear " + dataT[t].UserName + ", " + EditorMessage.Text +
                                "\nDownload the app Red Assistance: https://bit.ly/2JJnyWW";
                                await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numberst, message), null);
                                t++;
                                break;
                            };
                            break;
                    }
                    DependencyService.Get<IMessage>().ShortAlert("Sent");
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