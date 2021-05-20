using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class RecommendedTargets
    {
        string newStudentId;
        int currentYear;
        int matchYear;
        string matchStudentId;
        List<Chararcteristics> newStudentChars;
        List<Targets> recommendations;
        double countMatch;

        public RecommendedTargets(string newStudentId, int currentYear, int matchYear, string matchStudentId, List<Chararcteristics> newStudentChars, List<Targets> recommendations, double countMatch)
        {
            NewStudentId = newStudentId;
            CurrentYear = currentYear;
            MatchYear = matchYear;
            MatchStudentId = matchStudentId;
            NewStudentChars = newStudentChars;
            Recommendations = recommendations;
            CountMatch = countMatch;
    }

        public RecommendedTargets() { }

        public string NewStudentId { get => newStudentId; set => newStudentId = value; }
        public int CurrentYear { get => currentYear; set => currentYear = value; }
        public int MatchYear { get => matchYear; set => matchYear = value; }
        public string MatchStudentId { get => matchStudentId; set => matchStudentId = value; }
        public List<Chararcteristics> NewStudentChars { get => newStudentChars; set => newStudentChars = value; }
        public List<Targets> Recommendations { get => recommendations; set => recommendations = value; }
        public double CountMatch { get => countMatch; set => countMatch = value; }

        public RecommendedTargets GetRecommendedTargets()
        {
            DBServices dbs = new DBServices();
            List<Students> S = dbs.ReadStudents();

            int dis1 = 0;
            int dis2 = 0;

            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].StudentId == NewStudentId)
                {
                    dis1 = S[i].Dis1st;
                    dis2 = S[i].Dis2nd;
                }
            }

            RecommendedTargets recommendations = dbs.ActivateRecommendation(this, dis1, dis2);

            return recommendations;
        }

    }
}