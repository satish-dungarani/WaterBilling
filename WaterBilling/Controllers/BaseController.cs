using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using WaterBillingDB;

namespace WaterBilling.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                base.Initialize(requestContext);
                //clsCommonUI._Terminal = System.Net.Dns.GetHostName();

                //if (requestContext.RouteData.Values["action"].ToString().ToUpper() == "LOGIN" || requestContext.RouteData.Values["action"].ToString().ToUpper() == "LOGOFF")
                //{

                //    Session.Remove("UserName");
                //    return;
                //}
                //else
                //{
                //    if (Session == null || Session["UserName"] == null)
                //    {
                //        requestContext.HttpContext.Response.Close();
                //        requestContext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                //        requestContext.HttpContext.Response.End();
                //        //prequestcontext.HttpContext.Response.Clear();
                //        //prequestcontext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                //        //prequestcontext.HttpContext.Response.End();
                //        //filterContext.Result = new RedirectResult(Url.Action("Login", "Account"),true);
                //        //return;
                //        //RedirectToAction("Login", "Account");

                //    }
                //    else
                //    {
                //        if (Session != null || Session["AccessMenuList"] != null)
                //        {
                //            if (clsCommonUI.checkAccessAlloworNot((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"]) == false)
                //            {
                //                requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home"), false);
                //                //filterContext.Result = new RedirectResult(Url.Action("Index", "Home"), true);
                //                //return;
                //                //RedirectToAction("Index", "Home");
                //            }

                //        }
                //    }

                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {

                //System.Web.Routing.RequestContext prequestcontext = new RequestContext();


                clsCommonUI._Terminal = System.Net.Dns.GetHostName();

                if (filterContext.ActionDescriptor.ActionName.ToUpper() == "LOGIN" || filterContext.ActionDescriptor.ActionName.ToUpper() == "LOGOFF")
                {

                    //Session.Remove("UserName");
                    Session.RemoveAll();
                    //return;
                }
                else
                {
                    if (Session == null || Session["UserName"] == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account" }));
                        //RedirectToAction("LogOff", "Account");
                        //prequestcontext.HttpContext.Response.Clear();
                        //prequestcontext.HttpContext.Response.Redirect(Url.Action("Login", "Account"), true);
                        //filterContext.Result = RedirectToAction("Login", "Account");
                        //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "LogOff", controller = "Account" }));
                        //return;
                        //RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        if (Session != null || Session["AccessMenuList"] != null)
                        {
                            if (clsCommonUI.checkAccessAlloworNot((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"]) == false)
                            {
                                //prequestcontext.HttpContext.Response.Redirect(Url.Action("Index", "Home"), false);
                                //filterContext.Result = RedirectToAction("Index", "Home");
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
                                //filterContext.Result = new RedirectResult(Url.Action("Index", "Home"), true);
                                //return;
                                //RedirectToAction("Index", "Home");
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            //Logging the Exception
            filterContext.ExceptionHandled = true;

            var Result = this.View("Error", new HandleErrorInfo(exception,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString()));

            filterContext.Result = Result;

        }
    }
}
