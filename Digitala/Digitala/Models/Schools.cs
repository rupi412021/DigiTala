using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Schools
    {
        string schoolName;
        int schoolId;

        public Schools(string schoolName, int schoolId)
        {
            SchoolName = schoolName;
            SchoolId = schoolId;

        }
        public Schools() { }

        public string SchoolName { get => schoolName; set => schoolName = value; }
        public int SchoolId { get => schoolId; set => schoolId = value; }

        public List<Schools> Read()
        {
            DBServices dbs = new DBServices();
            List<Schools> sList = dbs.ReadSchools();
            return sList;
        }

    }
}