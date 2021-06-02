using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Tools
    {
        int tid;
        int tserial;
        int year;
        int studentId;
        string tool;

        public Tools(int tid, int tserial, int year, int studentId, string tool)
        {
            Tid = tid;
            Tserial = tserial;
            Year = year;
            StudentId = studentId;
            Tool = tool;
        }

        public Tools(){}

        public int Tid { get => tid; set => tid = value; }
        public int Tserial { get => tserial; set => tserial = value; }
        public int Year { get => year; set => year = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string Tool { get => tool; set => tool = value; }


        public List<Tools> Read(int sid,int syear)
        {
            DBServices dbs = new DBServices();
            List<Tools> tList = dbs.ReadTools(sid, syear);
            return tList;
        }

        public List<Tools> Delete()
        {
            DBServices dbs = new DBServices();
            dbs.DeleteTool(this);
            List<Tools> tList = dbs.ReadTools(StudentId, Year);
            return tList;
        }

        public List<Tools> Insert()
        {
            DBServices dbs = new DBServices();
            dbs.InsertTool(this);
            List<Tools> tList = dbs.ReadTools(StudentId, Year);
            return tList;
        }
    }
}