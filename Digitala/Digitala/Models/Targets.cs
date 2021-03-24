using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Targets
    {
        int tarSerial;
        int faSerial;
        int sfaSerial;
        string target;
        string functionArea;
        string subFunctionArea;
        double suitability;
        double originality;
        int numOfUses;
        

        public Targets(int tarSerial, int faSerial, int sfaSerial, string target, string functionArea, string subFunctionArea, double suitability, double originality, int numOfUses)
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
        }

        public Targets() { }

        public int TarSerial { get => tarSerial; set => tarSerial = value; }
        public int FaSerial { get => faSerial; set => faSerial = value; }
        public int SfaSerial { get => sfaSerial; set => sfaSerial = value; }
        public string Target { get => target; set => target = value; }
        public string FunctionArea { get => functionArea; set => functionArea = value; }
        public string SubFunctionArea { get => subFunctionArea; set => subFunctionArea = value; }
        public double Suitability { get => suitability; set => suitability = value; }
        public double Originality { get => originality; set => originality = value; }
        public int NumOfUses { get => numOfUses; set => numOfUses = value; }

        public List<Targets> Read()
        {
            DBServices dbs = new DBServices();
            List<Targets> tList = dbs.ReadTargets();
            return tList;
        }

        public List<Targets> Delete()
        {
            DBServices dbs = new DBServices();
            dbs.DeleteTarget(this.TarSerial);
            return Read();
        }

        public List<Targets> Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
            return Read();
        }

        public List<Targets> Update()
        {
            DBServices dbs = new DBServices();
            dbs.Update(this);
            return Read();
        }

        public List<Targets> GetRecommendedTargets(string SId, int year)
        {
            DBServices dbs = new DBServices();
            List <Students> S = dbs.ReadStudents();
            int dis1 = 0;
            int dis2 = 0;

            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].StudentId == SId)
                {
                    dis1 = S[i].Dis1st;
                    dis2 = S[i].Dis2nd;
                }
            }

            List<Students> SDB = dbs.ReadStudentsByDis(dis1, dis2);

            List<Targets> tList = dbs.ActivateRecommendation(SId, year, SDB);

            return tList;
        }
}
}