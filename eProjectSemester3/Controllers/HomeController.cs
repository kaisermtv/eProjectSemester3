using eProjectSemester3.Application;
using eProjectSemester3.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProjectSemester3.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //var a = ServiceFactory.Get<MembershipSevice>();
            //ViewBag.Title = a.Test();
            return View();
        }
    }
}
