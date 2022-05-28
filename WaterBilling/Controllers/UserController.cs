using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBilling.Models;
using WaterBillingDB;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace WaterBilling.Controllers
{
    public class UserController : BaseController
    {
        clsUserMaster _objUserMaster = new clsUserMaster();
        string _Message = string.Empty;
        //
        // GET: /Setup/
        public ActionResult Index()
        {
            SelectList _objRole = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.UserRole);
            ViewData["fillUserRole"] = _objRole;

            SelectList _objCollectionCenter = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CollectionCenter);
            ViewData["fillCollectionCenter"] = _objCollectionCenter;

            return View();
        }

        #region Transactin Events

        public ActionResult editSession(int? pID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "USER")))
            {
                Session["UserID"] = pID;
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
            bool result = true;
            if (Session["UserID"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "USER"));
            }
            if (result)
            {
                SelectList _objCollectionCenter = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CollectionCenter);
                ViewData["fillCollectionCenter"] = _objCollectionCenter;

                SelectList _objUserType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.UserType);
                ViewData["fillUserType"] = _objUserType;

                SelectList _objUserRole = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.UserRole);
                ViewData["fillUserRole"] = _objUserRole;

                UserMasterModel _obj = new UserMasterModel();
                try
                {
                    int pID = 0;
                    if (Session["UserID"] != null && !string.IsNullOrEmpty(Session["UserID"].ToString()))
                        pID = Convert.ToInt32(Session["UserID"].ToString());

                    //List<int> _RefCollcentId = _objUserMaster.GetListofCollectionCenter(pID);
                    string _RefCollcentId = string.Empty;
                    var tuple =_objUserMaster.GetListofCollectionCenter(pID);
                    foreach (var item in tuple.Item1)
                    {
                        _RefCollcentId += item + ",";
                    }
                    if (!string.IsNullOrEmpty(_RefCollcentId))
                        ViewBag._RefCollCentId = _RefCollcentId.Substring(0, _RefCollcentId.Length - 1);

                    _obj = _objUserMaster.getUserMaster(pID).Select(x => new UserMasterModel
                    {

                        ID = x.ID,
                        refUserTypeID = x.RefuserTypeID,
                        UserType = x.UserType,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        PhotoPath = x.PhotoPath,
                        Gender = x.Gender,
                        DOB = x.DOB,
                        MobileNo = x.MobileNo,
                        ContactNo1 = x.ContactNo1,
                        ContactNo2 = x.ContactNo2,
                        EmailID = x.EmailID,
                        IsActive = x.IsActive,
                        refRoleID = Convert.ToInt32(x.RefRoleID),
                        UserRole = x.UserRole,
                        UserName = x.UserName,
                        AllowMobileRegistration = x.AllowMobileRegistration,
                        RefCollectionCenterId = tuple.Item1,
                        CollCentIsActive = tuple.Item2,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_obj == null)
                    {
                        _obj = new UserMasterModel();
                    }

                    Session["UserID"] = null;
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
        public ActionResult Save(UserMasterModel _paramObj)
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

                    _paramObj.IsActive = true;
                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objUserMaster.saveUserMaster(_paramObj.ID, _paramObj.refUserTypeID, _paramObj.FirstName, _paramObj.LastName, _paramObj.PhotoPath,
                                _paramObj.Gender, Convert.ToDateTime(_paramObj.DOB), _paramObj.MobileNo, _paramObj.ContactNo1, _paramObj.ContactNo2, _paramObj.EmailID, _paramObj.IsActive, _paramObj.refRoleID,
                                _paramObj.UserName, _paramObj.AllowMobileRegistration,
                                _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal, _paramObj.RefCollectionCenterId, _paramObj.CollCentIsActive));
                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objUserMaster.saveUserMaster(_paramObj.ID, _paramObj.refUserTypeID, _paramObj.FirstName, _paramObj.LastName, _paramObj.PhotoPath,
                                _paramObj.Gender, Convert.ToDateTime(_paramObj.DOB), _paramObj.MobileNo, _paramObj.ContactNo1, _paramObj.ContactNo2, _paramObj.EmailID, _paramObj.IsActive, _paramObj.refRoleID,
                                _paramObj.UserName, _paramObj.AllowMobileRegistration, _paramObj.InsUser,
                                 _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal, _paramObj.RefCollectionCenterId, _paramObj.CollCentIsActive));
                }

                if (_result)
                {
                    if (_paramObj.ID == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        return RedirectToAction("index", "user");

                        //return Json(new { Result = "Success", msg = "Record successfully inserted!" });
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return RedirectToAction("index", "user");
                        //return Json(new { Result = "Success", msg = "Record successfully updated!" });
                    }
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    return RedirectToAction("index", "user");
                    //return Json(new { Result = "Success", msg = "There was some server error. Please try again later!" });
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "user");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int inID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "USER")))
            {
                try
                {
                    bool _result = false;

                    #region To insert record in database
                    _result = Convert.ToBoolean(_objUserMaster.deleteUserMaster(inID));

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
        public JsonResult updateStatus_IsActive(int inID, bool inCurrentStatus)
        {
            try
            {
                bool _result = false;

                _result = Convert.ToBoolean(_objUserMaster.updateStatus_IsActive(inID, !inCurrentStatus));

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

        [HttpPost]
        public JsonResult updateStatus_MobileAppAccess(int inID, bool inCurrentStatus)
        {
            try
            {
                bool _result = false;

                _result = Convert.ToBoolean(_objUserMaster.updateStatus_MobileAppAccess(inID, !inCurrentStatus));

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

            List<sp_UserMaster_SelectWhere_Result> _objList;

            string _strwhere = "";
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                _strwhere += " and (FullName like '%" + param.sSearch + "%' " +
                                  " or UserType like '%" + param.sSearch + "%' " +
                                  " or MobileNo like '%" + param.sSearch + "%' " +
                                  " or Contact1 like '%" + param.sSearch + "%' " +
                                  " or EmailID like '%" + param.sSearch + "%' " +
                                  " or UserName like '%" + param.sSearch + "%' " +
                                  " or UserRole like '%" + param.sSearch + "%' " +
                                ")";
            }

            #region Sorting Started

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            string Sortingname = " order by ";

            switch (sortColumnIndex)
            {
                case 1:
                    Sortingname += "FullName " + sortDirection;
                    break;
                case 2:
                    Sortingname += "UserType " + sortDirection;
                    break;
                case 3:
                    Sortingname += "MobileNo" + sortDirection;
                    break;
                case 4:
                    Sortingname += "EmailID" + sortDirection;
                    break;
                case 5:
                    Sortingname += "UserName" + sortDirection;
                    break;
                case 6:
                    Sortingname += "UserRole" + sortDirection;
                    break;
                default:
                    Sortingname += "FullName " + sortDirection;
                    break;
            }
            _strwhere += Sortingname;

            #endregion

            _objList = _objUserMaster.getUserMasterWhere(_strwhere).ToList();
            if (_objList != null && _objList.ToList().Count > 0)
            {
                var masterValue = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var result = from c in masterValue
                             select new[] { c.FullName, c.UserType, c.MobileNo, 
                                            c.EmailID, c.UserName, c.UserRole, Convert.ToString(c.AllowMobileRegistration), 
                                            Convert.ToString(c.IsActive), Convert.ToString(c.ID), Convert.ToString(c.RefRoleID) };


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
        //public UserManager<ApplicationUser> UserManager { get; private set; }
        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool _result = false;
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            model.UpdUser = -1;
        //            model.UpdTerminal = "test";

        //            _result = Convert.ToBoolean(_objUserMaster.SetUserRole(model.UserId, model.RefRoleId, model.UserName, model.UpdUser, model.UpdTerminal));
        //            if (_result)
        //            {
        //                TempData["Success"] = "User Successfully Registerd.";
        //                return RedirectToAction("Index", "User");
        //            }
        //            else
        //            {
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return RedirectToAction("Index", "User");
        //}

        #region Validation

        [HttpPost]
        public JsonResult isValueExists(int inID, string inValueName)
        {
            try
            {
                if (_objUserMaster.isUserExists(inID, inValueName))
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