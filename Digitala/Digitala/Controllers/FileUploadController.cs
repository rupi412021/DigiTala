using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class FileUploadController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Post()
        {
            List<string> imageLinks = new List<string>();
            var httpContext = HttpContext.Current;
            
            // Check for any uploaded file  
            if (httpContext.Request.Files.Count > 0)
            {
                //Loop through uploaded files  
                for (int i = 0; i < httpContext.Request.Files.Count; i++)
                {
                    HttpPostedFile httpPostedFile = httpContext.Request.Files[i];

                    // this is an example of how you can extract addional values from the Ajax call
                    string id = httpContext.Request.Form["ID"];
                    string year = httpContext.Request.Form["Year"];

                    string dir = HostingEnvironment.MapPath("~/StudentFiles/" + id + "/" + year + "/");

                    // If directory does not exist, create it
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (httpPostedFile != null)
                    {
                        // Construct file save path  
                        //var fileSavePath = Path.Combine(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadFolder"]), httpPostedFile.FileName);
                        string fname = httpPostedFile.FileName.Split('\\').Last();
                        var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/StudentFiles/"+id+"/"+year+"/"), fname);
                        // Save the uploaded file  
                        httpPostedFile.SaveAs(fileSavePath);
                        imageLinks.Add("StudentFiles/" + id + "/" + year + "/" + fname);

                        Students s = new Students();
                        s.InsertFile(id, year, fname);
                    }

                    

                }
            }

            // Return status code  
            return Request.CreateResponse(HttpStatusCode.Created, imageLinks);
        }

        [HttpGet]
        [Route("api/FileUpload/{id}/{year}")]
        public List<FileUpload> Get(string id,string year)
        {
            FileUpload f = new FileUpload();
            List<FileUpload> flist = f.Read(id,year);
            return flist;
        }
    }
}
