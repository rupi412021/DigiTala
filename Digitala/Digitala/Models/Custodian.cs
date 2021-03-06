using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Custodian
    {
        string cEmail;
        string cName;
        string cPhone;
        string cSID;

        public Custodian(string cEmail, string cName, string cPhone, string cSID)
        {
            CEmail = cEmail;
            CName = cName;
            CPhone = cPhone;
            CSID = cSID;
        }

        public Custodian() { }
        
        public string CEmail { get => cEmail; set => cEmail = value; }
        public string CName { get => cName; set => cName = value; }
        public string CPhone { get => cPhone; set => cPhone = value; }
        public string CSID { get => cSID; set => cSID = value; }

        public void Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
        }

        public Custodian Read(string studentId)
        {
            DBServices dbs = new DBServices();
            Custodian cust = dbs.ReadCustodiansForStudent(studentId);
            return cust;
        }

        public void Update()
        {
            DBServices dbs = new DBServices();
            dbs.Update(this);
        }

    }
}