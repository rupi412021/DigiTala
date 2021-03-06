using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Targets
    {
        string target;
        string functionArea;
        string subFunctionArea;
        double suitability;
        double originality;
        int numOfUses;
        

        public Targets(string target, string functionArea, string subFunctionArea, double suitability, double originality, int numOfUses)
        {
            Target = target;
            FunctionArea = functionArea;
            SubFunctionArea = subFunctionArea;
            Suitability = suitability;
            Originality = originality;
            NumOfUses = numOfUses;
        }

        public Targets() { }

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
    }
}