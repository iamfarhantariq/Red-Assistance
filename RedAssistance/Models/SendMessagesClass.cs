using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace RedAssistance.Models
{
    public class SendMessagesClass
    {
        public static void SendMessagetoHeads(string bloodgroup, string hospital, string city, string cell)
        {
            HandleDB handleDB = new HandleDB();
            var data = handleDB.GetAdminDB().ToList();
            string numbers = string.Empty;
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

                    string message = string.Format("'{0}' Blood is required at {1} {2}. Contact at '{3}'. " +
                        "Download the app Red Assistance: https://bit.ly/2JJnyWW", bloodgroup, hospital, city, cell);

                    // await httpClientMsg.PostAsync(String.Format("http://sms4connect.com/api/sendsms.php/sendsms/url?id=educationuni&pass=sms4connect123&mask=UE-BloodApp&to={0}&lang=English&msg={1}&type=xml", numbers, message), null);

                    break;
                }
            };
        }
    }
}
