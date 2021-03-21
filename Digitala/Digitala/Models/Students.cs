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
        string SGender;
        string sAddress;
        string sPhone;
        string sDescripion;
        int Dis1st;
        int Dis2nd;

        public Students(string studentId, string sFirstName, string sLastName, string sEmail, DateTime sBirthDate, string sGender1, string sAddress, string sPhone, string sDescripion, int dis1st1, int dis2nd1)
        {
            StudentId = studentId;
            SFirstName = sFirstName;
            SLastName = sLastName;
            SEmail = sEmail;
            SBirthDate = sBirthDate;
            SGender1 = sGender1;
            SAddress = sAddress;
            SPhone = sPhone;
            SDescripion = sDescripion;
            Dis1st1 = dis1st1;
            Dis2nd1 = dis2nd1;
        }
        public Students() { }

        public string StudentId { get => studentId; set => studentId = value; }
        public string SFirstName { get => sFirstName; set => sFirstName = value; }
        public string SLastName { get => sLastName; set => sLastName = value; }
        public string SEmail { get => sEmail; set => sEmail = value; }
        public DateTime SBirthDate { get => sBirthDate; set => sBirthDate = value; }
        public string SGender1 { get => SGender; set => SGender = value; }
        public string SAddress { get => sAddress; set => sAddress = value; }
        public string SPhone { get => sPhone; set => sPhone = value; }
        public string SDescripion { get => sDescripion; set => sDescripion = value; }
        public int Dis1st1 { get => Dis1st; set => Dis1st = value; }
        public int Dis2nd1 { get => Dis2nd; set => Dis2nd = value; }
    }
}