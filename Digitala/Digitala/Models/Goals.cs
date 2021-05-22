using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class Goals
    {
        int goalId;
        int serialTarget;
        int year;
        string studentId;
        string goal;
        string goalStatus;

        public Goals(int goalId, int serialTarget, int year, string studentId, string goal, string goalStatus)
        {
            GoalId = goalId;
            SerialTarget = serialTarget;
            Year = year;
            StudentId = studentId;
            Goal = goal;
            GoalStatus = goalStatus;
        }
        public Goals() { }

        public int GoalId { get => goalId; set => goalId = value; }
        public int SerialTarget { get => serialTarget; set => serialTarget = value; }
        public int Year { get => year; set => year = value; }
        public string StudentId { get => studentId; set => studentId = value; }
        public string Goal { get => goal; set => goal = value; }
        public string GoalStatus { get => goalStatus; set => goalStatus = value; }

        public List<Goals> Read(string studentId, int year)
        {
            DBServices dbs = new DBServices();
            List<Goals> gList = dbs.ReadGoals(studentId, year);

            return gList;
        }

        public void Update()
        {
            DBServices dbs = new DBServices();
            dbs.Update(this);
            UpdateTargetsAchievement(this.SerialTarget);
        }

        public void UpdateTargetsAchievement(int serialTar)
        {
            DBServices dbs = new DBServices();
            List<Goals> gList = Read(this.StudentId, this.Year);
            int E = 0;
            int P = 0;
            int countGoals = 0;
            for (int i = 0; i < gList.Count; i++)
            {
                if(gList[i].SerialTarget == serialTar)
                {
                    countGoals++;
                    if (gList[i].goalStatus == 'E'.ToString())
                    {
                        E++;
                    }
                    else if (gList[i].goalStatus == 'P'.ToString())
                    {
                        P++;
                    }

                }
            }

            if(E == countGoals)
                dbs.UpdateTargetsAchievement(serialTar, 'E');
            else if(E+P == 0)
                dbs.UpdateTargetsAchievement(serialTar, 'F');
            else
                dbs.UpdateTargetsAchievement(serialTar, 'P');
        }
    }
}