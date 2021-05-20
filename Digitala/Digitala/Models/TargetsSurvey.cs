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

        public void Delete()
        {
            DBServices dbs = new DBServices();
            DateTime today = DateTime.Now;
            List<TargetsSurvey> tList = dbs.ReadTargetsForSurveys();
            List <int> tempList = new List<int>();

            int count = 0;
            int ind = 0;
            double avgO = 0;
            double avgS = 0;

            for (int i = 0; i < tList.Count; i++)
            {
                if (tList[i].CreationDate.AddDays(14) <= today)
                {
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (tempList[j] == tList[i].TarId)
                            ind = 1;
                    }
                    if (ind == 0)
                        tempList.Add(tList[i].TarId);
                    ind = 0;
                }                    
            }
            for (int j = 0; j < tempList.Count; j++)                
            {
                Targets target = new Targets();
                for (int i = 0; i < tList.Count; i++)
                {
                    if(tList[i].TarId == tempList[j])
                    {
                        avgO = (avgO * count + tList[i].Originality) / (count + 1);
                        avgS = (avgS * count + tList[i].Suitability) / (count + 1);
                        count++;
                        target.FaSerial = tList[i].FaSerial;
                        target.SfaSerial = tList[i].SfaSerial;
                        target.Target = tList[i].Target;
                        target.TarSerial = tList[i].TarId;
                    }
                }

                if (avgO * 0.5 + avgS * 0.5 >= 4)
                {                    
                    target.Suitability = avgS;
                    target.Originality = avgO;
                    target.NumOfUses = 1;
                    
                    dbs.Insert(target);
                    dbs.DeleteSurvey(target.TarSerial);
                }
                else
                {
                    dbs.DeleteSurvey(target.TarSerial);
                }

                count = 0;
                avgO = 0;
                avgS = 0;
            }
        }

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