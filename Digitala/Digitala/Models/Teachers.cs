using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Digitala.Models
{
    public class Teachers
    {
        int teacherID;
        string teacherFname;
        string teacherSurName;
        string teacherEmail;
        string teacherPassword;
        bool teacherAdmin;
        int teacherSchoolId;
        int teacherYear;
        string teacherClass;


        public Teachers(){}

        public Teachers(int teacherID, string teacherFname, string teacherSurName, string teacherEmail, string teacherPassword, bool teacherAdmin,  int teacherSchoolId, int teacherYear, string teacherClass)
        {
            TeacherID = teacherID;
            TeacherFname = teacherFname;
            TeacherSurName = teacherSurName;
            TeacherEmail = teacherEmail;
            TeacherPassword = teacherPassword;
            TeacherAdmin = teacherAdmin;
            TeacherSchoolId =  teacherSchoolId;
            TeacherYear = teacherYear;
            TeacherClass = teacherClass;
        }

        public int TeacherSchoolId { get => teacherSchoolId; set => teacherSchoolId = value; }
        public int TeacherID { get => teacherID; set => teacherID = value; }
        public string TeacherFname { get => teacherFname; set => teacherFname = value; }
        public string TeacherSurName { get => teacherSurName; set => teacherSurName = value; }
        public string TeacherEmail { get => teacherEmail; set => teacherEmail = value; }
        public string TeacherPassword { get => teacherPassword; set => teacherPassword = value; }
        public bool TeacherAdmin { get => teacherAdmin; set => teacherAdmin = value; }
        public int TeacherYear { get => teacherYear; set => teacherYear = value; }
        public string TeacherClass { get => teacherClass; set => teacherClass = value; }

        public void Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
        }

        public List<Teachers> Read()
        {
            DBServices dbs = new DBServices();
            List<Teachers> teachersList = dbs.ReadTeachers();
            return teachersList;
        }

        public string sendEmail(string e)
        {
            DBServices dbs = new DBServices();
            string newPass =  dbs.SendMailToUser(e, CreatePassword(6));
            return newPass;
        }

        public void setTpass(string p, string e)
        {
            DBServices dbs = new DBServices();
            dbs.setNewPass(p, e);
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

    }
}