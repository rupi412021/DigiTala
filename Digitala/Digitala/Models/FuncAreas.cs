﻿using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class FuncAreas
    {
        int subAreaId;
        int areaId;
        string subArea;
        string area;

        public FuncAreas(int subAreaId, int areaId, string subArea, string area)
        {
            SubAreaId = subAreaId;
            AreaId = areaId;
            SubArea = subArea;
            Area = area;
        }
        public FuncAreas() { }

        public int SubAreaId { get => subAreaId; set => subAreaId = value; }
        public int AreaId { get => areaId; set => areaId = value; }
        public string SubArea { get => subArea; set => subArea = value; }
        public string Area { get => area; set => area = value; }

        public List<FuncAreas> Read()
        {
            DBServices dbs = new DBServices();
            List<FuncAreas> tList = dbs.ReadSubAreas();
            return tList;
        }

        public List<FuncAreas> NewArea(int areaId, string area)
        {
            DBServices dbs = new DBServices();
            dbs.InsertArea(areaId, area);
            return Read();
        }
        
        public List<FuncAreas> NewSubArea(int areaId, int subAreaId, string subArea)
        {
            DBServices dbs = new DBServices();
            dbs.InsertSubArea(areaId, subAreaId, subArea);
            return Read();
        }

    }
}