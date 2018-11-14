using FilesOnCloud.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Amazon;

namespace FilesOnCloud.Controllers
{
    public class HomeController : Controller
    {
        private ICloudFileHandler handler; 
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IEnumerable<HttpPostedFileBase> files)
        {
            if ("Dropbox".Equals(ConfigurationManager.AppSettings["Provider"]))
            {
                handler = new DropboxHandler(ConfigurationManager.AppSettings["DropboxToken"]);
            }
            else
            {
                handler = new AWSHandler(RegionEndpoint.USEast1, ConfigurationManager.AppSettings["AWSProfileName"]);
            }
            foreach (HttpPostedFileBase file in files)
            {
                try
                {
                    string newFileName = file.FileName; //Change the file name if you need to
                    string filePath = ConfigurationManager.AppSettings["FolderPath"];
                    await handler.Upload(filePath, newFileName, Helpers.Utility.ReadData(file.InputStream));

                }
                catch (IOException ioex)
                {
                    //TODO: Log your exception
                }
                catch (Exception ex)
                {
                    //TODO: Log your exception
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}