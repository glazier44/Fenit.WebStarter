using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSBuild.Community.Tasks.Git;

namespace Fenit.WebStarter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TODOTK
            //var task = new GitVersion();
            //task.ToolPath = @"C:\Users\kwiek\AppData\Local\Atlassian\SourceTree\git_local\cmd";
            ////task.LocalPath = Path.Combine(prjRootPath, @"Source");
            //bool result = task.Execute();
            return View();
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