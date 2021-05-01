using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Talas
    {
        string studentId;
        int currentYear;
        List<Chararcteristics> studentChars;
        List<Targets> targets;

        public Talas() { }

        public Talas(string studentId, int currentYear, List<Chararcteristics> studentChars, List<Targets> targets)
        {
            StudentId = studentId;
            CurrentYear = currentYear;
            StudentChars = studentChars;
            Targets = targets;
        }

        public string StudentId { get => studentId; set => studentId = value; }
        public int CurrentYear { get => currentYear; set => currentYear = value; }
        public List<Chararcteristics> StudentChars { get => studentChars; set => studentChars = value; }
        public List<Targets> Targets { get => targets; set => targets = value; }

        public Talas Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
            dbs.InsertToolsAndGoals(Targets, StudentId, CurrentYear);

            Talas Tala = Read(StudentId, CurrentYear);

            return Tala;
        }

        public Talas Read(string studentId, int year)
        {
            DBServices dbs = new DBServices();
            Talas Tala = dbs.ReadTala(studentId, year);

            return Tala;
        }

    }
}
