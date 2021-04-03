using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Tala
    {
        string studentId;
        int currentYear;
        List<Chararcteristics> studentChars;
        List<Targets> targets;

        public Tala() { }

        public Tala(string studentId, int currentYear, List<Chararcteristics> studentChars, List<Targets> targets)
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
    }
}