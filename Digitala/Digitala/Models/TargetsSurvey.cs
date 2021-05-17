using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class TargetsSurvey
    {
        int tarSerial;
        int tarId;
        int faSerial;
        int sfaSerial;
        string target;
        string functionArea;
        string subFunctionArea;
        double suitability;
        double originality;
        int numOfUses;
        DateTime creationDate;
        string teacherId;


        public TargetsSurvey(int tarSerial, int tarId, int faSerial, int sfaSerial, string target, string functionArea, string subFunctionArea, double suitability, double originality, int numOfUses, DateTime creationDate, string teacherId)
        {
            TarSerial = tarSerial;
            FaSerial = faSerial;
            SfaSerial = sfaSerial;
            Target = target;
            FunctionArea = functionArea;
            SubFunctionArea = subFunctionArea;
            Suitability = suitability;
            Originality = originality;
            NumOfUses = numOfUses;
            TarId = tarId;
            CreationDate = creationDate;
            TeacherId = teacherId;
        }

        public TargetsSurvey() { }

        public int TarSerial { get => tarSerial; set => tarSerial = value; }
        public int TarId { get => tarId; set => tarId = value; }
        public int FaSerial { get => faSerial; set => faSerial = value; }
        public int SfaSerial { get => sfaSerial; set => sfaSerial = value; }
        public string Target { get => target; set => target = value; }
        public string FunctionArea { get => functionArea; set => functionArea = value; }
        public string SubFunctionArea { get => subFunctionArea; set => subFunctionArea = value; }
        public double Suitability { get => suitability; set => suitability = value; }
        public double Originality { get => originality; set => originality = value; }
        public int NumOfUses { get => numOfUses; set => numOfUses = value; }
        public string TeacherId { get => teacherId; set => teacherId = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }

        public List<TargetsSurvey> Read(string teacherId)
        {
            DBServices dbs = new DBServices();
            List<TargetsSurvey> tList = dbs.ReadTargetsSurvey(teacherId);
            return tList;
        }

        //public void Delete()
        //{
        //    DBServices dbs = new DBServices();
        //    dbs.DeleteTarget(this.TarSerial);
        //}

        public void Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
        }

        public List<TargetsSurvey> Update()
        {
            DBServices dbs = new DBServices();
            dbs.Update(this);
            return Read(TeacherId);
        }

    }
}