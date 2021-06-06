using Digitala.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digitala.Models
{
    public class FileUpload
    {
        string sid;
        string syear;
        string filename;

        public FileUpload(string sid, string syear, string filename)
        {
            Sid = sid;
            Syear = syear;
            Filename = filename;
        }

        public FileUpload(){}

        public string Sid { get => sid; set => sid = value; }
        public string Syear { get => syear; set => syear = value; }
        public string Filename { get => filename; set => filename = value; }

        public List<FileUpload> Read(string id, string year)
        {
            DBServices dbs = new DBServices();
            List<FileUpload> fList = dbs.ReadFilesName(id,year);
            return fList;
        }

        public void Delete(string filename)
        {
            DBServices dbs = new DBServices();
            dbs.DeleteFile(filename);
        }
    }
}