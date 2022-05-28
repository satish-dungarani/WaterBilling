using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBillingDB;
using WaterBilling.Models;

namespace WaterBilling.Controllers
{
    public class SetupController : BaseController
    {

        clsMasterValue _objMasterVal = new clsMasterValue();
        clsCommon _ObjCommon = new clsCommon();
        static bool IsSystemGenerated;
        string _Message = string.Empty;
        //
        // GET: /Setup/
        public ActionResult Index()
        {
            //ViewBag.masterType = id;
            //Session["refMasterID"] = 1;
            if (Session["refMasterID"] == null || string.IsNullOrEmpty(Session["refMasterID"].ToString()) || !Convert.ToBoolean(clsCommonUI.checkAccessIndividualInMapPath((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "editSessionMasterID('" + Session["refMasterID"].ToString() + "')")))
            {
                return RedirectToAction("index", "home");
            }
            MasterValueModel _ObjMasterValue = new MasterValueModel();

            clsCommonUI.MasterType _masterType = (clsCommonUI.MasterType)Convert.ToInt32(Session["refMasterID"].ToString());
            _ObjMasterValue.IsSystemGenerated = IsSystemGenerated = _ObjCommon.get_MasterList(Convert.ToInt32(Session["refMasterID"].ToString()));

            ViewBag.SetupID = Session["refMasterID"].ToString();
            ViewBag.Title = _masterType.ToString() + " Setup";

            //_refMasterID = Convert.ToInt32(Session["refMasterID"].ToString());
            return View(_ObjMasterValue);
        }

        #region Transactin Events

        public ActionResult editSession(int? pID)
        {

            if (!IsSystemGenerated && Convert.ToBoolean(clsCommonUI.checkAccessIndividualInMapPath((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "editSessionMasterID('" + Session["refMasterID"].ToString() + "')")))
            {
                Session["ID"] = pID;
                
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
            MasterValueModel _obj = new MasterValueModel();
            bool result = true;
            if (Session["ID"] == null && Session["refMasterID"] != null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividualInMapPath((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "editSessionMasterID('" + Session["refMasterID"].ToString() + "')"));
            }
            if (!IsSystemGenerated && result)
            {
                try
                {
                    if (Session["refMasterID"] != null && !string.IsNullOrEmpty(Session["refMasterID"].ToString()))
                    {
                        int pRefMasterID = Convert.ToInt32(Session["refMasterID"].ToString());
                        int pID = 0;
                        if (Session["ID"] != null && !string.IsNullOrEmpty(Session["ID"].ToString()))
                            pID = Convert.ToInt32(Session["ID"].ToString());

                        _obj = _objMasterVal.getMasterValue(pRefMasterID, pID).Select(x => new MasterValueModel
                        {

                            refMasterID = pRefMasterID,
                            ID = x.ID,
                            ValueName = x.ValueName,
                            ShortName = x.ShortName,
                            MarathiName = x.MarathiName,
                            OrdNo = x.OrdNo.HasValue ? x.OrdNo.Value : 0,
                            IsActive = x.IsActive,
                            InsUser = x.InsUser,
                            InsDate = x.InsDate,
                            InsTerminal = x.InsTerminal,
                            UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                            UpdDate = x.UpdDate,
                            UpdTerminal = x.UpdTerminal

                        }).FirstOrDefault();

                        if (_obj == null)
                        {
                            _obj = new MasterValueModel();
                            _obj.refMasterID = pRefMasterID;
                        }

                        Session["ID"] = null;
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }

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
        public ActionResult Save(MasterValueModel _paramObj)
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

                    _result = Convert.ToBoolean(_objMasterVal.saveMasterValue(_paramObj.refMasterID, _paramObj.ID, _paramObj.ValueName,
                                 _paramObj.ShortName, _paramObj.MarathiName, Convert.ToInt32(_paramObj.OrdNo), _paramObj.IsActive, _paramObj.InsUser,
                                 _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objMasterVal.saveMasterValue(_paramObj.refMasterID, _paramObj.ID, _paramObj.ValueName,
                                 _paramObj.ShortName, _paramObj.MarathiName, Convert.ToInt32(_paramObj.OrdNo), _paramObj.IsActive, _paramObj.InsUser,
                                 _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.ID == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        return RedirectToAction("index", "setup");

                        //return Json(new { Result = "Success", msg = "Record successfully inserted!" });
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return RedirectToAction("index", "setup");
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
                return RedirectToAction("index", "setup");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int inID)
        {

            if (!IsSystemGenerated && Convert.ToBoolean(clsCommonUI.checkAccessIndividualInMapPath((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "editSessionMasterID('" + Session["refMasterID"].ToString() + "')")))
            {
                try
                {
                    bool _result = false;

                    #region To insert record in database
                    _result = Convert.ToBoolean(_objMasterVal.deleteMasterValue(inID));

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

        [HttpPost]
        public JsonResult updateStatus(int inID, bool inCurrentStatus)
        {
            try
            {
                bool _result = false;

                _result = Convert.ToBoolean(_objMasterVal.updateStatus(inID, !inCurrentStatus));

                if (_result)
                {
                    //TempData["Success"] = "Status sucessfully updated!";
                    return Json(new { Result = "Success", msg = "Status sucessfully updated!" });
                }
                else
                {
                    //TempData["Error"] = "There was some server error. Please try again later!";
                    return Json(new { Result = "Error", msg = "There was some server error. Please try again later!" });
                }
            }
            catch (Exception ex)
            {
                //TempData["Error"] = ex.Message;
                return Json(new { Result = "Error", msg = ex.ToString() });
            }

        }

        public ActionResult AjaxHandler(clsJQueryDataTableParamModel param)
        {

            if (Session["refMasterID"] == null || string.IsNullOrEmpty(Session["refMasterID"].ToString()))
            {
                return RedirectToAction("index", "home");
            }

            List<sp_MasterValue_SelectWhere_Result> _objList;

            string _strwhere = "";
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                _strwhere += " and (ValueName like '%" + param.sSearch + "%' " +
                                  " or ShortName like '%" + param.sSearch + "%' " +
                                ")";
            }

            #region Sorting Started

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            string Sortingname = " order by ";

            switch (sortColumnIndex)
            {
                case 1:
                    Sortingname += "ValueName " + sortDirection;
                    break;
                case 2:
                    Sortingname += "ShortName " + sortDirection;
                    break;
                case 3:
                    Sortingname += "OrdNo " + sortDirection;
                    break;
                case 4:
                    Sortingname += "IsActive" + sortDirection;
                    break;
                default:
                    Sortingname += "ValueName " + sortDirection;
                    break;
            }
            _strwhere += Sortingname;

            #endregion

            _objList = _objMasterVal.getMasterValueWhere(Convert.ToInt32(Session["refMasterID"].ToString()), _strwhere).ToList();
            if (_objList != null && _objList.ToList().Count > 0)
            {
                var masterValue = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var result = from c in masterValue
                             select new[] { c.ValueName, c.ShortName, Convert.ToString(c.OrdNo), Convert.ToString(c.IsActive), Convert.ToString(c.ID) };
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

        #region Validation

        [HttpPost]
        public JsonResult isValueExists(int inID, string inValueName)
        {
            try
            {
                if (_objMasterVal.isValueExists(Convert.ToInt32(Session["refMasterID"]), inID, inValueName))
                {
                    //TempData["Warning"] = "Value with the same name already exists!";
                    return Json(new { Result = "Warning", msg = "Value with the same name already exists!" });
                }
                return Json(new { Result = "Success", msg = "" });
            }
            catch (Exception ex)
            {
                //TempData["Error"] = ex.ToString();
                return Json(new { Result = "Error", msg = ex.Message });
            }

        }
        #endregion
    }
}