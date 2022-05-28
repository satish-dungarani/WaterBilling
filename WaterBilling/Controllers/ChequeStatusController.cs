using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBilling.Models;
using WaterBillingDA;
using System.Globalization;

namespace WaterBilling.Controllers
{
    public class ChequeStatusController : BaseController
    {
        //
        // GET: /ChequeStatus/
        clsReceiptDetail _objReceiptDetail = new clsReceiptDetail();
        public ActionResult Index()
        {
            SelectList _ObjCollectionCenter = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CollectionCenter);
            ViewData["fillCollectionCenter"] = _ObjCollectionCenter;

            SelectList _ObjReader = clsCommonUI.fillReaderName_UserMaster();
            ViewData["fillReader"] = _ObjReader;

            return View();
        }

        [HttpPost]
        public JsonResult Save(int _ID, bool _isChecked, string _ChequeStatus, string _ChqBounceRefNo, DateTime? _ChqBounceDate,
                                               decimal? _ChqBounceCharge)
        {
            bool retval = false;
            try
            {
                string _IsChequeStatus = null;

                if (_isChecked)
                {
                    _IsChequeStatus = _ChequeStatus;
                }
                else
                {
                    _IsChequeStatus = "HOLD";
                }

                retval = Convert.ToBoolean(_objReceiptDetail.UpdateChequeStatus(_ID, _IsChequeStatus, _ChqBounceRefNo,
                    _ChqBounceDate, _ChqBounceCharge, clsCommonUI._User, clsCommonUI._Terminal));


                if (retval)
                {
                    //TempData["Success"] = "Rights successfully allocated!";
                    return Json(new { _ChqStatus = "true"}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //TempData["Error"] = "There was some server error. Please try again later!";
                    return Json(new { _ChqStatus = "false"}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { _ChqStatus = "false"}, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Select(ReceiptDetailModel _ObjParam)
        {
            try
            {
                List<ReceiptDetailModel> _objChequeStatus = new List<ReceiptDetailModel>();

                DateTime _StartDate = DateTime.ParseExact(_ObjParam.ChqStatusReceiptDate.Split('-')[0].Trim(), "MM/dd/yyyy", null);
                DateTime _LastDate = DateTime.ParseExact(_ObjParam.ChqStatusReceiptDate.Split('-')[1].Trim(), "MM/dd/yyyy", null);

                string _WhereCondtion = " and IsChqStatus in('Hold','" + _ObjParam.IsChqStatus + "') and ChequeNo is not null and ReceiptDate >= Cast('" + _StartDate.ToString("dd/MMM/yyyy") + "' as Date) and ReceiptDate <= Cast('" + _LastDate.ToString("dd/MMM/yyyy") + "' as Date)";
                string _CollectionCenterFilterList = string.Empty;
                if (_ObjParam.RefChqStatusCCId != null)
                {
                    if (_ObjParam.RefChqStatusCCId.Count > 0)
                    {
                        foreach (var CCID in _ObjParam.RefChqStatusCCId)
                        {
                            _CollectionCenterFilterList += CCID + ",";
                        }
                        _WhereCondtion += " and RefCollectionCenterId in (" + _CollectionCenterFilterList.Substring(0, _CollectionCenterFilterList.Length - 1) + ")";
                    }
                }
                if (!string.IsNullOrEmpty(_ObjParam.ConsumerNo))
                {
                    _WhereCondtion += " and ConsumerNo = '" + _ObjParam.ConsumerNo + "'";
                }
                foreach (var _obj in _objReceiptDetail.GetReceiptDetailSelectWhere(_WhereCondtion))
                {
                    _objChequeStatus.Add(new ReceiptDetailModel
                    {
                        Id = _obj.Id,
                        ConsumerNo = _obj.ConsumerNo,
                        ConsumerName = _obj.ConsumerName,
                        ChequeNo = _obj.ChequeNo,
                        ChequeDate = _obj.ChequeDate,
                        BankName = _obj.BankName,
                        BankBranch = _obj.BankBranch,
                        RecAmt = _obj.RecAmt,
                        ChqStatus = _obj.IsChqStatus.ToUpper() == "PASS" || _obj.IsChqStatus.ToUpper() == "BOUNCE" ? true : false,
                        //IsChqStatus = _obj.IsChqStatus,
                        InsUser = _obj.InsUser,
                        InsDate = _obj.InsDate,
                        InsTerminal = _obj.InsTerminal,
                        UpdUser = _obj.UpdUser.HasValue ? _obj.UpdUser.Value : 0,
                        UpdDate = _obj.UpdDate,
                        UpdTerminal = _obj.UpdTerminal
                    });
                }


                return PartialView("LoadCosumerChequeandDDdetailPartial", _objChequeStatus);

            }
            catch (Exception)
            {

                return PartialView("LoadCosumerChequeandDDdetailPartial");
            }
        }

        //public List<ReceiptDetailModel> FillChequeAndDDStatus(string _WhereCondtion)
        //{
        //    try
        //    {
        //        List<ReceiptDetailModel> _objChequeStatus = new List<ReceiptDetailModel>();

        //        foreach (var _obj in _objReceiptDetail.GetReceiptDetailSelectWhere(_WhereCondtion))
        //        {
        //            _objChequeStatus.Add(new ReceiptDetailModel
        //            {
        //                Id = _obj.Id,
        //                ConsumerNo = _obj.ConsumerNo,
        //                ConsumerName = _obj.ConsumerName,
        //                ChequeNo = _obj.ChequeNo,
        //                ChequeDate = _obj.ChequeDate,
        //                BankName = _obj.BankName,
        //                BankBranch = _obj.BankBranch,
        //                RecAmt = _obj.RecAmt,
        //                ChqStatus = _obj.IsChqStatus.ToUpper() == "PASS" || _obj.IsChqStatus.ToUpper() == "BOUNCE" ? true : false,
        //                //IsChqStatus = _obj.IsChqStatus,
        //                InsUser = _obj.InsUser,
        //                InsDate = _obj.InsDate,
        //                InsTerminal = _obj.InsTerminal,
        //                UpdUser = _obj.UpdUser.HasValue ? _obj.UpdUser.Value : 0,
        //                UpdDate = _obj.UpdDate,
        //                UpdTerminal = _obj.UpdTerminal
        //            });
        //        }
        //        return _objChequeStatus;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}