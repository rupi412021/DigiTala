using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Chararcteristics
    {
        string chararcteristic;
        int faSerial;
        int sfaSerial;
        bool isWeakness;

        public Chararcteristics(string chararcteristic, int faSerial, int sfaSerial, bool isWeakness)
        {
            Chararcteristic = chararcteristic;
            FaSerial = faSerial;
            SfaSerial = sfaSerial;
            IsWeakness = isWeakness;
        }

        public Chararcteristics() { }

        public string Chararcteristic { get => chararcteristic; set => chararcteristic = value; }
        public int FaSerial { get => faSerial; set => faSerial = value; }
        public int SfaSerial { get => sfaSerial; set => sfaSerial = value; }
        public bool IsWeakness { get => isWeakness; set => isWeakness = value; }

        public List<Chararcteristics> Read()
        {
            DBServices dbs = new DBServices();
            List<Chararcteristics> cList = dbs.ReadChararcteristics();
            return cList;
        }

        public void PostCharsToStudent(string studentId, string year, string[] chars)
        {
            DBServices dbs = new DBServices();
            dbs.InsertChararcteristics(studentId, year, chars);
        }
    }
}