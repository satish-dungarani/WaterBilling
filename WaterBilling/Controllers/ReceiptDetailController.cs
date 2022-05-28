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
    public class ReceiptDetailController : BaseController
    {

        clsReceiptDetail _objReceiptDetail = new clsReceiptDetail();
        string _Message = string.Empty;
        //
        // GET: /ReceiptDetail/
        //public ActionResult Index()
        //{
        //   return View("Manage");
        //}

        #region Transactin Events

        public ActionResult editSession(int? pID, string pFrmType)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "RECEIPTDETAIL")))
            {
                Session["ReceiptId"] = pID;
                Session["FrmType"] = pFrmType;
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
            if (Session["ReceiptId"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "RECEIPTDETAIL"));
            }
            if (result)
            {
                SelectList _objCollectionCenter = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CollectionCenter);
                ViewData["fillCollectionCenter"] = _objCollectionCenter;

                SelectList _objPaymentType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.PaymentType);
                ViewData["fillPaymentType"] = _objPaymentType;

                SelectList _objBank = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Bank);
                ViewData["fillBank"] = _objBank;

                ReceiptDetailModel _obj = new ReceiptDetailModel();
                try
                {
                    int pID = 0;
                    if (Session["ReceiptId"] != null && !string.IsNullOrEmpty(Session["ReceiptId"].ToString()))
                        pID = Convert.ToInt32(Session["ReceiptId"].ToString());

                    _obj = _objReceiptDetail.GetReceiptDetail(pID).Select(x => new ReceiptDetailModel
                    {

                        Id = x.Id,
                        RefCollectionCenterId = x.RefCollectionCenterId,
                        CounterNo = x.CounterNo,
                        ReceiptNo = x.ReceiptNo,
                        ReceiptDate = x.ReceiptDate,
                        RefConsumerId = x.RefConsumerId,
                        RecAmt = x.RecAmt,
                        RefPaymentTypeId = x.RefPaymentTypeId,
                        ChequeNo = x.ChequeNo,
                        ChequeDate = x.ChequeDate,
                        BankName = x.BankName,
                        BankBranch = x.BankBranch,
                        IsChqStatus = x.IsChqStatus,
                        ChqBounceRefNo = x.ChqBounceRefNo,
                        ChqBounceDate = x.ChqBounceDate,
                        ChqBounceCharge = x.ChqBounceCharge,
                        DPCBal = x.DPCBal,
                        DPCPaid = x.DPCPaid,
                        BasicBal = x.BasicBal,
                        BasicPaid = x.BasicPaid,
                        BasicBalPending = x.BasicBalPending,
                        DPCBalPending = x.DPCBalPending,
                        //FrmType =  Session["FrmType"] != null? Session["FrmType"].ToString(): "",
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();



                    if (_obj == null)
                    {
                        _obj = new ReceiptDetailModel();
                        _obj.FrmType = Session["FrmType"].ToString() != "" ? Session["FrmType"].ToString() : "";
                    }

                    Session["ReceiptId"] = null;
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
                return View("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Save(ReceiptDetailModel _paramObj)
        {
            try
            {
                bool _result = false;
                string _strResult = string.Empty;

                #region To insert record in database

                if (!string.IsNullOrEmpty(_paramObj.ChequeNo))
                    _paramObj.IsChqStatus = "Hold";
                else
                    _paramObj.IsChqStatus = "Pass";


                if (_paramObj.Id == 0)
                {
                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objReceiptDetail.SaveReceiptDetail(_paramObj.Id, _paramObj.RefCollectionCenterId, _paramObj.CounterNo, _paramObj.ReceiptNo, _paramObj.ReceiptDate,
                                _paramObj.RefConsumerId, _paramObj.RecAmt, _paramObj.RefPaymentTypeId, _paramObj.ChequeNo, _paramObj.ChequeDate,
                                _paramObj.BankName, _paramObj.BankBranch, _paramObj.IsChqStatus,
                                _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objReceiptDetail.SaveReceiptDetail(_paramObj.Id, _paramObj.RefCollectionCenterId, _paramObj.CounterNo, _paramObj.ReceiptNo, _paramObj.ReceiptDate,
                                _paramObj.RefConsumerId, _paramObj.RecAmt, _paramObj.RefPaymentTypeId, _paramObj.ChequeNo, _paramObj.ChequeDate,
                                _paramObj.BankName, _paramObj.BankBranch, _paramObj.IsChqStatus,
                                _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.Id == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";

                        //return RedirectToAction("Manage", "ReceiptDetail");
                        return PartialView("LoadLastTwoReceiptDetailPartial", GetLastTwoReceiptDetailPartial());
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return PartialView("LoadLastTwoReceiptDetailPartial", GetLastTwoReceiptDetailPartial());
                    }
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    return PartialView("LoadLastTwoReceiptDetailPartial", GetLastTwoReceiptDetailPartial());
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage", "ReceiptDetail");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        #endregion

        public sp_ReceiptDetail_SelectWhere_Result GetLastTwoReceiptDetailPartial()
        {
            sp_ReceiptDetail_SelectWhere_Result _Obj = new sp_ReceiptDetail_SelectWhere_Result();
            try
            {
                //string _Condition = " and Cast(ReceiptDate as Date) = Cast(GETDATE() as Date) and InsUser = " + clsCommonUI._User + " order by Id Desc";
                string _Condition = " and InsUser = " + clsCommonUI._User + " order by Id Desc";
                _Obj = _objReceiptDetail.GetReceiptDetailSelectWhere(_Condition).FirstOrDefault();
                return _Obj;
            }
            catch (Exception)
            {
                return _Obj;
            }
        }

        [HttpGet]
        public ActionResult DisplayConsumerDetailPartial(string pConsumerNo)
        {
            clsConsumerMaster _ObjConsumerMaster = new clsConsumerMaster();
            clsConsumeDetail _ObjConsumDetail = new clsConsumeDetail();
            try
            {
                var _Obj = _ObjConsumerMaster.getConsumerMasterWhere(" and ConsumerNo = '" + pConsumerNo + "'").Select(x => new ReceiptDetailModel
                {
                    RefConsumerId = x.Id,
                    ConsumerName = x.FullName,
                    ConsumerAddress = x.Address,
                    DPCBal = x.PrevDPC,
                    BasicBal = x.PrevAmount,
                    DueDate = Convert.ToDateTime(_ObjConsumDetail.get_Consumerdetail_SelectWhere(" and RefConsumerId = " + x.Id + " order by ReadingDate Desc").FirstOrDefault().DueDate)

                }).FirstOrDefault();

                return PartialView(_Obj);
            }
            catch (Exception ex)
            {
                //throw new EXception(ex.Message);
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        public JsonResult GetBankNameList()
        {
            clsMasterValue _ObjMaster = new clsMasterValue();
            try
            {
                var _BankList = _ObjMaster.getMasterValueWhere((int)clsCommonUI.MasterType.Bank, "").Select(x => x.ValueName).ToArray();
                return Json(_BankList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        //[HttpPost]
        //public ActionResult Delete(int inID)
        //{
        //    if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "USER")))
        //    {
        //        try
        //        {
        //            bool _result = false;

        //            #region To insert record in database
        //            _result = Convert.ToBoolean(_objReceiptDetail.DeleteReceiptDetail(inID));

        //            if (_result)
        //            {
        //                _Message = "Record sucessfully deleted!";
        //            }
        //            else
        //            {
        //                _Message = "There was some server error. Please try again later!";
        //            }
        //            #endregion
        //            return Json(new { _result, _Message }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            _Message = ex.Message;
        //            return Json(new { _result = false, _Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        _Message = "Delete Rights not given!";
        //        return Json(new { _result = false, _Message }, JsonRequestBehavior.AllowGet);

        //    }
        //}

        //public ActionResult AjaxHandler(clsJQueryDataTableParamModel param)
        //{

        //    List<sp_UserMaster_SelectWhere_Result> _objList;

        //    string _strwhere = "";
        //    if (!string.IsNullOrEmpty(param.sSearch))
        //    {
        //        _strwhere += " and (FullName like '%" + param.sSearch + "%' " +
        //                          " or UserType like '%" + param.sSearch + "%' " +
        //                          " or MobileNo like '%" + param.sSearch + "%' " +
        //                          " or Contact1 like '%" + param.sSearch + "%' " +
        //                          " or EmailID like '%" + param.sSearch + "%' " +
        //                          " or UserName like '%" + param.sSearch + "%' " +
        //                          " or UserRole like '%" + param.sSearch + "%' " +
        //                        ")";
        //    }

        //    #region Sorting Started

        //    var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //    var sortDirection = Request["sSortDir_0"]; // asc or desc
        //    string Sortingname = " order by ";

        //    switch (sortColumnIndex)
        //    {
        //        case 1:
        //            Sortingname += "FullName " + sortDirection;
        //            break;
        //        case 2:
        //            Sortingname += "UserType " + sortDirection;
        //            break;
        //        case 3:
        //            Sortingname += "MobileNo" + sortDirection;
        //            break;
        //        case 4:
        //            Sortingname += "EmailID" + sortDirection;
        //            break;
        //        case 5:
        //            Sortingname += "UserName" + sortDirection;
        //            break;
        //        case 6:
        //            Sortingname += "UserRole" + sortDirection;
        //            break;
        //        default:
        //            Sortingname += "FullName " + sortDirection;
        //            break;
        //    }
        //    _strwhere += Sortingname;

        //    #endregion

        //    _objList = _objUserMaster.getUserMasterWhere(_strwhere).ToList();
        //    if (_objList != null && _objList.ToList().Count > 0)
        //    {
        //        var masterValue = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
        //        var result = from c in masterValue
        //                     select new[] { c.FullName, c.UserType, c.MobileNo, 
        //                                    c.EmailID, c.UserName, c.UserRole, Convert.ToString(c.AllowMobileRegistration), 
        //                                    Convert.ToString(c.IsActive), Convert.ToString(c.ID), Convert.ToString(c.RefRoleID) };


        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = _objList.Count(),
        //            iTotalDisplayRecords = _objList.Count(),
        //            aaData = result
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = 0,
        //            iTotalDisplayRecords = _objList.Count(),
        //            aaData = new List<string[]>()
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //#endregion
    }
}