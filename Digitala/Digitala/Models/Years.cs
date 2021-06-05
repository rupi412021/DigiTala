using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Years
    {
        string hebrewYear;
        string gregorianYear;

        public Years(string hebrewYear, string gregorianYear)
        {
            HebrewYear = hebrewYear;
            GregorianYear = gregorianYear;
        }
        public Years() { }

        public string HebrewYear { get => hebrewYear; set => hebrewYear = value; }
        public string GregorianYear { get => gregorianYear; set => gregorianYear = value; }

        public List<Years> Read()
        {
            DBServices dbs = new DBServices();
            List<Years> yList = dbs.ReadYears();
            return yList;
        }
    }
}