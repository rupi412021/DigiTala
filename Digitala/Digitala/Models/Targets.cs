﻿using Digitala.Models.DAL;
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
        bool newTar;
        bool newPhrase;
        List<string> tools;
        List<string> goals;

        public Targets(int tarSerial, int faSerial, int sfaSerial, string target, string functionArea, string subFunctionArea, double suitability, double originality, int numOfUses, bool newTar, bool newPhrase, List<string> tools, List<string> goals)
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
            NewTar = newTar;
            Tools = tools;
            Goals = goals;
            NewPhrase = newPhrase;
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
        public bool NewTar { get => newTar; set => newTar = value; }
        public bool NewPhrase { get => newPhrase; set => newPhrase = value; }
        public List<string> Tools { get => tools; set => tools = value; }
        public List<string> Goals { get => goals; set => goals = value; }


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
    
}
}