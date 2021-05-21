using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Privileges
    {
        int dCode;
        string dPhrase;
        string sId;
        string year;

        public Privileges(int dCode, string dPhrase, string sId, string year)
        {
            DCode = dCode;
            DPhrase = dPhrase;
            SId = sId;
            Year = year;
        }

        public Privileges() { }

        public int DCode { get => dCode; set => dCode = value; }
        public string DPhrase { get => dPhrase; set => dPhrase = value; }
        public string SId { get => sId; set => sId = value; }
        public string Year { get => year; set => year = value; }


        public List<Privileges> Read()
        {
            DBServices dbs = new DBServices();
            List<Privileges> dList = dbs.ReadPrivileges();
            return dList;
        }

        public List<Privileges> Read(string studentId, string year)
        {
            DBServices dbs = new DBServices();
            List<Privileges> dList = dbs.ReadPrivileges(studentId, year);
            return dList;
        }

        public void Delete()
        {
            DBServices dbs = new DBServices();
            dbs.DeletePrivileges(this);
        }

        public void Insert()
        {
            DBServices dbs = new DBServices();
            dbs.InsertPrivileges(this);
        }

    }
}