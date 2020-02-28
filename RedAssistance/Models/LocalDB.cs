using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedAssistance.Models
{
    public class LocalDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int _ID { get; set; }
        public string FullName { get; set; }
        public string CellNumber { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string _bloodgroup;
        public string BloodGroup
        {
            get
            {
                return _bloodgroup;
            }
            set
            {
                _bloodgroup = value;
                if (_bloodgroup == "Dont Know")
                {
                    _bloodgroup = "No";
                }
            }
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TodayDate { get; set; }
        public string FutureUse { get; set; }
    }
}
