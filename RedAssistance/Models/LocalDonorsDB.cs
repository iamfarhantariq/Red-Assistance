using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedAssistance.Models
{
    public class LocalDonorsDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int _ID { get; set; }
        public string FullName { get; set; }
        public string CellNumber { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string BloodGroup { get; set; }
        //public string _bloodgroup;
        //public string BloodGroup
        //{
        //    get
        //    {
        //        return _bloodgroup;
        //    }
        //    set
        //    {
        //        _bloodgroup = value;
        //        switch (_bloodgroup)
        //        {
        //            case "Dont Know":
        //                _bloodgroup = "Unknown";
        //                break;
        //            case "A positive":
        //                _bloodgroup = "A+";
        //                break;

        //            case "A negative":
        //                _bloodgroup = "A-";
        //                break;

        //            case "B positive":
        //                _bloodgroup = "B+";
        //                break;

        //            case "B negative":
        //                _bloodgroup = "B-";
        //                break;

        //            case "AB positive":
        //                _bloodgroup = "AB+";
        //                break;

        //            case "AB negative":
        //                _bloodgroup = "AB-";
        //                break;

        //            case "O positive":
        //                _bloodgroup = "O+";
        //                break;

        //            case "O negative":
        //                _bloodgroup = "O-";
        //                break;
        //        }
        //    }
        //}
        public string TodayDate { get; set; }
        public string AddedBy { get; set; }
        public string FutureUse { get; set; }
        public string CapName => FullName.First().ToString().ToUpper() + FullName.Substring(1);
    }
}
