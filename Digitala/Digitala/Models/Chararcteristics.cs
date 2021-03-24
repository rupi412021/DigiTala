using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Chararcteristics
    {
        int characteristicKey;
        string chararcteristic;
        int faSerial;
        int sfaSerial;
        bool isWeakness;
        string studentId;
        int year;

        public Chararcteristics(int characteristicKey, string chararcteristic, int faSerial, int sfaSerial, bool isWeakness, string studentId, int year)
        {
            Chararcteristic = chararcteristic;
            FaSerial = faSerial;
            SfaSerial = sfaSerial;
            IsWeakness = isWeakness;
            StudentId = studentId;
            Year = year;
            CharacteristicKey = characteristicKey;
        }

        public Chararcteristics() { }

        public string Chararcteristic { get => chararcteristic; set => chararcteristic = value; }
        public string StudentId { get => studentId; set => studentId = value; }
        public int FaSerial { get => faSerial; set => faSerial = value; }
        public int SfaSerial { get => sfaSerial; set => sfaSerial = value; }
        public int Year { get => year; set => year = value; }
        public bool IsWeakness { get => isWeakness; set => isWeakness = value; }
        public int CharacteristicKey { get => characteristicKey; set => characteristicKey = value; }

        public List<Chararcteristics> Read()
        {
            DBServices dbs = new DBServices();
            List<Chararcteristics> cList = dbs.ReadChararcteristics();
            return cList;
        }

        public void PostCharsToStudent()
        {
            DBServices dbs = new DBServices();
            dbs.InsertChararcteristics(this);
        }

        public void Delete()
        {
            DBServices dbs = new DBServices();
            dbs.DeleteChars(this);
        }

        //public void TEMP(int id)
        //{
        //    DBServices dbs = new DBServices();
        //    dbs.TEMP(id);
        //}
    }
}