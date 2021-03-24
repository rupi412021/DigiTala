using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Students
    {
        string studentId;
        string sFirstName;
        string sLastName;
        string sEmail;
        DateTime sBirthDate;
        string sGender;
        string sAddress;
        string sPhone;
        string sDescripion;
        int dis1st;
        int dis2nd;
        int countMatch;

        public Students(string studentId, string sFirstName, string sLastName, string sEmail, DateTime sBirthDate, string sGender, string sAddress, string sPhone, string sDescripion, int dis1st, int dis2nd, int countMatch)
        {
            StudentId = studentId;
            SFirstName = sFirstName;
            SLastName = sLastName;
            SEmail = sEmail;
            SBirthDate = sBirthDate;
            SGender = sGender;
            SAddress = sAddress;
            SPhone = sPhone;
            SDescripion = sDescripion;
            Dis1st = dis1st;
            Dis2nd = dis2nd;
            CountMatch = countMatch;
        }

        public Students() { }

        public string StudentId { get => studentId; set => studentId = value; }
        public string SFirstName { get => sFirstName; set => sFirstName = value; }
        public string SLastName { get => sLastName; set => sLastName = value; }
        public string SEmail { get => sEmail; set => sEmail = value; }
        public DateTime SBirthDate { get => sBirthDate; set => sBirthDate = value; }
        public string SGender { get => sGender; set => sGender = value; }
        public string SAddress { get => sAddress; set => sAddress = value; }
        public string SPhone { get => sPhone; set => sPhone = value; }
        public string SDescripion { get => sDescripion; set => sDescripion = value; }
        public int Dis1st { get => dis1st; set => dis1st = value; }
        public int Dis2nd { get => dis2nd; set => dis2nd = value; }
        public int CountMatch { get => countMatch; set => countMatch = value; }

        public List<Students> Read()
        {
            DBServices dbs = new DBServices();
            List<Students> sList = dbs.ReadStudents();
            return sList;
        }

        public List<Students> Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
            return Read();
        }

        public List<Students> Update()
        {
            DBServices dbs = new DBServices();
            dbs.Update(this);
            return Read();
        }
    }
}