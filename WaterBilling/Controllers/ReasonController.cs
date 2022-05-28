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
    public class ReasonController : BaseController
    {
        clsReasonMaster _objReasonMaster = new clsReasonMaster();
        string _Message = string.Empty;
        //
        // GET: /Reason/
        public ActionResult Index()
        {
            return View();
        }

        #region Transactin Events

        public ActionResult editSession(int pReasonID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "REASON")))
            {
                Session["pReasonID"] = pReasonID;
                return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _Message = "Update Rights not given!";
                return Json(new { Saved = "No", _Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Manage()
        {
            ReasonMasterModel _obj = new ReasonMasterModel();
            bool result = true;
            if (Session["UserID"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "REASON"));
            }
            if (result)
            {
                try
                {
                    int pReasonID = 0;

                    if (Session["pReasonID"] != null && !string.IsNullOrEmpty(Session["pReasonID"].ToString()))
                        pReasonID = Convert.ToInt32(Session["pReasonID"].ToString());


                    //To fill combo from reason master
                    SelectList _objFillCombo = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.ReasonType);
                    ViewData["fillReason"] = _objFillCombo;


                    _obj = _objReasonMaster.getReasonMaster(pReasonID).Select(x => new ReasonMasterModel
                    {

                        ID = x.ID,
                        refReasonTypeID = x.RefReasonTypeID,
                        ReasonType = x.ReasonType,
                        ReasonName = x.ReasonName,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_obj == null)
                    {
                        _obj = new ReasonMasterModel();
                    }

                    Session["pReasonID"] = null;
                }
                catch (Exception ex)
                {
                    //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
                }
                return View(_obj);
            }
            else
            {
                TempData["Warning"] = "Insert rights not given!";
                Index();
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Save(ReasonMasterModel _paramObj)
        {

            try
            {

                bool _result = false;
                string _strResult = string.Empty;



                #region To insert record in database

                if (_paramObj.ID == 0)
                {

                    //_paramObj.InsUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.InsTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objReasonMaster.saveReasonMaster(_paramObj.ID, _paramObj.refReasonTypeID,
                                 _paramObj.ReasonName, _paramObj.InsUser, _paramObj.InsTerminal,
                                 _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objReasonMaster.saveReasonMaster(_paramObj.ID, _paramObj.refReasonTypeID,
                                 _paramObj.ReasonName, _paramObj.InsUser, _paramObj.InsTerminal,
                                 _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.ID == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        return RedirectToAction("index", "reason");

                        //return Json(new { Result = "Success", msg = "Record successfully inserted!" });
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return RedirectToAction("index", "reason");
                        //return Json(new { Result = "Success", msg = "Record successfully updated!" });
                    }
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    return View();
                    //return Json(new { Result = "Success", msg = "There was some server error. Please try again later!" });
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "reason");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int inID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "REASON")))
            {
                try
                {
                    bool _result = false;

                    #region To insert record in database
                    _result = Convert.ToBoolean(_objReasonMaster.deleteReasonMaster(inID));

                    if (_result)
                    {
                        _Message = "Record sucessfully deleted!";
                    }
                    else
                    {
                        _Message = "There was some server error. Please try again later!";
                    }
                    #endregion
                    return Json(new { _result, _Message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    _Message = ex.Message;
                    return Json(new { _result = false, _Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                _Message = "Delete Rights not given!";
                return Json(new { _result = false, _Message }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult AjaxHandler(clsJQueryDataTableParamModel param)
        {

            List<sp_ReasonMaster_SelectWhere_Result> _objList;

            string _strwhere = "";
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                _strwhere += " and (ReasonName like '%" + param.sSearch + "%' " +
                                ")";
            }

            #region Sorting Started

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            string Sortingname = " order by ";

            switch (sortColumnIndex)
            {
                case 1:
                    Sortingname += "ReasonType " + sortDirection;
                    break;
                case 2:
                    Sortingname += "ReasonName " + sortDirection;
                    break;
                default:
                    Sortingname += "ReasonType " + sortDirection;
                    break;
            }
            _strwhere += Sortingname;

            #endregion

            _objList = _objReasonMaster.getReasonMasterWhere(_strwhere).ToList();
            if (_objList != null && _objList.ToList().Count > 0)
            {
                var masterValue = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var result = from c in masterValue
                             select new[] { c.ReasonType, c.ReasonName, Convert.ToString(c.ID) };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = _objList.Count(),
                    iTotalDisplayRecords = _objList.Count(),
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = _objList.Count(),
                    aaData = new List<string[]>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}