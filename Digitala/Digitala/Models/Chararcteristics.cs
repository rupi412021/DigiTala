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
        bool isWeekness;

        public Chararcteristics(string chararcteristic, int faSerial, int sfaSerial, bool isWeekness)
        {
            Chararcteristic = chararcteristic;
            FaSerial = faSerial;
            SfaSerial = sfaSerial;
            IsWeekness = isWeekness;
        }

        public Chararcteristics() { }

        public string Chararcteristic { get => chararcteristic; set => chararcteristic = value; }
        public int FaSerial { get => faSerial; set => faSerial = value; }
        public int SfaSerial { get => sfaSerial; set => sfaSerial = value; }
        public bool IsWeekness { get => isWeekness; set => isWeekness = value; }

        public List<Chararcteristics> Read(int[] areas)
        {
            DBServices dbs = new DBServices();
            List<Chararcteristics> cList = dbs.ReadChararcteristics(areas);
            return cList;
        }
    }
}