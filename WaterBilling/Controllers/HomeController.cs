using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBilling.Models;
using WaterBillingDA;
using WaterBillingDB;

namespace WaterBilling.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            //if (Session["UserName"] == null)
            //    return RedirectToAction("Login", "Account");
            //else

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

        public ActionResult editSession(int? inRefMasterID)
        {
            Session["refMasterID"] = inRefMasterID;
            return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
        }
    }
}