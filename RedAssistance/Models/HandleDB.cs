using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RedAssistance.Models
{
    public class HandleDB
    {
        private SQLiteConnection _sqlconnection;

        public HandleDB()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Pushover.db3");
            _sqlconnection = new SQLiteConnection(dbPath);
            _sqlconnection.CreateTable<LocalDB>();
            _sqlconnection.CreateTable<LocalDonorsDB>();
            _sqlconnection.CreateTable<LocalRequestsDB>();
            _sqlconnection.CreateTable<LocalAdminDB>();
            _sqlconnection.CreateTable<LocalUsersDB>();
        }

        //Get Users info
        public IEnumerable<LocalUsersDB> GetAllUserInfoDB()
        {
            return (from t in _sqlconnection.Table<LocalUsersDB>() select t).OrderByDescending(c => c.Id).ToList();
        }
        //Get User info
        public IEnumerable<LocalDB> GetUserInfoDB()
        {
            return (from t in _sqlconnection.Table<LocalDB>() select t).ToList();
        }
        //Get Admin info
        public IEnumerable<LocalAdminDB> GetAdminDB()
        {
            return (from t in _sqlconnection.Table<LocalAdminDB>() select t).ToList();
        }
        //Get Donors
        public IEnumerable<LocalDonorsDB> GetDonorsDB()
        {
            return (from t in _sqlconnection.Table<LocalDonorsDB>() select t).OrderByDescending(c => c.Id).ToList();
        }
        //Get Requests
        public IEnumerable<LocalRequestsDB> GetRequestsDB()
        {
            return (from t in _sqlconnection.Table<LocalRequestsDB>() select t).OrderByDescending(c => c.Id).ToList();
        }


        public void AddAllUsersDB(LocalUsersDB dB)
        {
            _sqlconnection.Insert(dB);
        }
        public void AddUserDB(LocalDB dB)
        {
            _sqlconnection.Insert(dB);
        }
        public void AddAdminDB(LocalAdminDB dB)
        {
            _sqlconnection.Insert(dB);
        }
        public void AddDonorsDB(LocalDonorsDB dB)
        {
            _sqlconnection.Insert(dB);
        }
        public void AddRequestsDB(LocalRequestsDB dB)
        {
            _sqlconnection.Insert(dB);
        }
        public void DeleteAllUserDB(LocalUsersDB dB)
        {
            _sqlconnection.Query<LocalUsersDB>("Drop Table LocalUsersDB");
        }
        public void DeleteUserDB(LocalDB dB)
        {
            _sqlconnection.Query<LocalDB>("Drop Table LocalDB");
        }
        public void DeleteAdminDB(LocalAdminDB dB)
        {
            _sqlconnection.Query<LocalAdminDB>("Drop Table LocalAdminDB");
        }
        public void DeleteDonorsDB(LocalDonorsDB dB)
        {
            _sqlconnection.Query<LocalDonorsDB>("Drop Table LocalDonorsDB");
        }
        public void DeleteRequestsDB(LocalRequestsDB dB)
        {
            _sqlconnection.Query<LocalRequestsDB>("Drop Table LocalRequestsDB");
        }

        public void DeleteUserDB(int id)
        {
            _sqlconnection.Delete<LocalUsersDB>(id);
        }
        public void DeleteAdminsDB(int id)
        {
            _sqlconnection.Delete<LocalAdminDB>(id);
        }
        public void DeleteDonorsDB(int id)
        {
            _sqlconnection.Delete<LocalDonorsDB>(id);
        }
        public void DeleteRequestsDB(int id)
        {
            _sqlconnection.Delete<LocalRequestsDB>(id);
        }

        public void UpdateUserDB(LocalDB dB)
        {
            _sqlconnection.Update(dB);
        }
        public void UpdateDonorDB(LocalDonorsDB dB)
        {
            _sqlconnection.Update(dB);
        }
        public void UpdateRequestDB(LocalRequestsDB dB)
        {
            _sqlconnection.Update(dB);
        }
        public void UpdateAllUserDB(LocalUsersDB dB)
        {
            _sqlconnection.Update(dB);
        }
        public void UpdateAdminDB(LocalAdminDB dB)
        {
            _sqlconnection.Update(dB);
        }

        public IEnumerable<LocalDonorsDB> GetDonorsDB(string _bloodgroup)
        {
            return _sqlconnection.Query<LocalDonorsDB>(String.Format("Select * from LocalDonorsDB Where BloodGroup='{0}' ORDER BY Id Desc", _bloodgroup));
        }
        public IEnumerable<LocalDonorsDB> GetDonorsByCell(string _cell)
        {
            return _sqlconnection.Query<LocalDonorsDB>(String.Format("Select * from LocalDonorsDB Where AddedBy='{0}' ORDER BY Id Desc", _cell));
        }
        public IEnumerable<LocalRequestsDB> GetRequestByCell(string cellnumber)
        {
            return _sqlconnection.Query<LocalRequestsDB>(String.Format("Select * from LocalRequestsDB Where AddedBy='{0}' ORDER BY Id Desc", cellnumber));
        }
        public IEnumerable<LocalRequestsDB> GetRequestsDB(string cellnumber)
        {
            return _sqlconnection.Query<LocalRequestsDB>(String.Format("Select * from LocalRequestsDB Where CellNumber='{0}' ORDER BY Id Desc", cellnumber));
        }
        public IEnumerable<LocalDonorsDB> RandomDonorsCellNumber()
        {
            return _sqlconnection.Query<LocalDonorsDB>("SELECT * FROM LocalDonorsDB ORDER BY RANDOM() LIMIT 20;");
        }
        public IEnumerable<LocalDonorsDB> RandomDonorsForty()
        {
            return _sqlconnection.Query<LocalDonorsDB>("SELECT * FROM LocalDonorsDB ORDER BY RANDOM() LIMIT 40;");
        }
    }
}
