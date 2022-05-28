using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBilling.Models;
using WaterBillingDB;

namespace WaterBilling.Controllers
{
    public class ConsumeDetailController : BaseController
    {
        clsConsumeDetail _objConsumeDetail = new clsConsumeDetail();
        clsMasterValue _ObjMasterValue = new clsMasterValue();
        clsConsumerMeterMaster _ObjConsumerMeter = new clsConsumerMeterMaster();
        string _Message = string.Empty;
        //
        // GET: /Setup/
        public ActionResult Index()
        {
            return View();
        }

        #region Transactin Events

        public ActionResult editSession(int? pID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "CONSUMEDETAIL")))
            {
                Session["ConsumeID"] = pID;
                return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _Message = "Update Rights not given!";
                return Json(new { Saved = "No", _Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult RetriveReaderList(int pCampId)
        {
            try
            {
                if (pCampId != 0)
                {
                    SelectList _objReaderName = clsCommonUI.fillReaderName_UserMaster(pCampId);
                    ViewData["fillReaderName"] = _objReaderName;
                }
            }
            catch (Exception)
            {
                return PartialView("_ReaderListPartial");
            }
            return PartialView("_ReaderListPartial");
        }

        [HttpGet]
        public ActionResult RetriveConsumerNoPartial(int pReaderId)
        {
            try
            {
                if (pReaderId != 0)
                {
                    SelectList _objConsumerNo = clsCommonUI.fillConsumerNo_ConsumerMaster(pReaderId);
                    ViewData["fillConsumerNo"] = _objConsumerNo;
                }
            }
            catch (Exception)
            {
                return PartialView();
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult Manage()
        {
            bool result = true;
            if (Session["ConsumeID"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CONSUMEDETAIL"));
            }
            if (result)
            {
                SelectList _objCamp = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Camp);
                ViewData["fillCamp"] = _objCamp;

                SelectList _objReaderName = clsCommonUI.fillReaderName_UserMaster();
                ViewData["fillReaderName"] = _objReaderName;

                if (clsCommonUI._IsReader)
                    RetriveConsumerNoPartial(clsCommonUI._User);
                else
                    RetriveConsumerNoPartial(Convert.ToInt32(_objReaderName.FirstOrDefault().Value));

                SelectList _objMeterStatus = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.MeterStatus);
                ViewData["fillMeterStatus"] = _objMeterStatus;

                ConsumeDetailModel _obj = new ConsumeDetailModel();
                try
                {
                    int pID = 0;
                    if (Session["ConsumeID"] != null && !string.IsNullOrEmpty(Session["ConsumeID"].ToString()))
                        pID = Convert.ToInt32(Session["ConsumeID"].ToString());

                    _obj = _objConsumeDetail.get_Consumerdetail_Select(pID).Select(x => new ConsumeDetailModel
                    {
                        Id = x.Id,
                        RefReaderId = clsCommonUI._User,
                        RefConsumerId = x.RefConsumerId,
                        RefConsumerMeterId = x.RefConsumerMeterId,
                        RefMeterStatusId = x.RefMeterStatusId,
                        ReadingDate = x.ReadingDate,
                        MeterReading = x.MeterReading,
                        ConsumerName = x.ConsumerName,
                        ConsumerAddress = x.ConsumerAddress,
                        BillDate = x.BillDate,
                        BillNo = x.BillNo,
                        AssessmentAmt = x.AssessmentAmt,
                        Rent = x.Rent,
                        PrevAmount = x.PrevAmount,
                        DueAmount = x.DueAmount,
                        PrevDPC = x.PrevDPC,
                        DueDPC = x.DueDPC,
                        //DpToCharge = x.DpToCharge,
                        Rebate = x.Rebate,
                        //OtherChargies = x.OtherChargies,
                        //OtherChargiesRemark = x.OtherChargiesRemark,
                        DueDate = x.DueDate,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_obj == null)
                    {
                        _obj = new ConsumeDetailModel();
                    }

                    Session["ConsumeID"] = null;
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
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ConsumerDetailPartial(int pID)
        {
            ConsumerMasterModel _obj = new ConsumerMasterModel();
            try
            {
                clsConsumerMaster _ObjConsumerMaster = new clsConsumerMaster();

                _obj = _ObjConsumerMaster.getConsumerMaster(pID).Select(x => new ConsumerMasterModel
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Address = x.Address,
                        RefMeterSizeId = Convert.ToInt32(x.RefMeterSizeId),
                        RefMeterTypeId = Convert.ToInt32(x.RefMeterTypeId),
                        RefSupplyTypeId = Convert.ToInt32(x.RefSupplyTypeId),
                        RefSupplyCategoryId = Convert.ToInt32(x.RefSupplyCategoryId)
                    }).FirstOrDefault();
            }
            catch (Exception)
            {
                return PartialView(_obj);
            }
            return PartialView(_obj);
        }

        [HttpPost]
        public ActionResult Save(ConsumeDetailModel _paramObj)
        {

            try
            {

                bool _result = false;
                string _strResult = string.Empty;

                int _MeterCount = _ObjConsumerMeter.getConsumerMeterMasterWhere(" and RefConsumerId = " + _paramObj.RefConsumerId).Count();
                if (_MeterCount == 0)
                {
                    TempData["Warning"] = "Meter Detail is not available for this consumer! Please enter first meter detail for this consumerno.";
                    return PartialView("ConsumeDetailListPartial");
                }

                if (_paramObj.MeterReading < 0)
                {
                    TempData["Warning"] = "Meter Reading can not lessthan 0(Zero).";
                    return PartialView("ConsumeDetailListPartial");
                }
                else if (_paramObj.MeterReading == 0 && "WORKING" == _ObjMasterValue.getMasterValue((int)clsCommonUI.MasterType.MeterStatus, _paramObj.RefMeterStatusId).FirstOrDefault().ValueName.ToUpper())
                {
                    TempData["Warning"] = "Meter Reading can not equal to 0(Zero) when meter is working.";
                    return PartialView("ConsumeDetailListPartial");
                }

                #region To insert record in database

                List<sp_ConsumeDetail_SelectWhere_Result> _ObjCondet = new List<sp_ConsumeDetail_SelectWhere_Result> ();
                _ObjCondet = _objConsumeDetail.get_Consumerdetail_SelectWhere(" and RefConsumerId = " + _paramObj.RefConsumerId + " and BillDate is null");
                if(_ObjCondet .Count() > 0 )
                    _paramObj.Id = _ObjCondet.FirstOrDefault().Id;

                if (_paramObj.Id == 0)
                {

                    //_paramObj.InsUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.InsTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objConsumeDetail.set_Consumerdetail_save(_paramObj.Id, _paramObj.RefConsumerId, _paramObj.RefReaderId,
                        _paramObj.RefMeterSizeId, _paramObj.RefMeterStatusId, Convert.ToDateTime(_paramObj.ReadingDate), _paramObj.MeterReading, _paramObj.BillType,_paramObj.BillMode,
                        _paramObj.RefMeterTypeId, _paramObj.RefSupplyTypeId, _paramObj.RefSupplyCategoryId, _paramObj.PRevReadingDate, _paramObj.LastReading, _paramObj.BillNo, _paramObj.BillDate, _paramObj.PrevBillDate, _paramObj.DueDate,
                        _paramObj.AssessmentAmt, _paramObj.PrevAmount, _paramObj.DueAmount, _paramObj.Rent, _paramObj.PrevDPC, _paramObj.DueDPC,_paramObj.TillDateDPC,
                        _paramObj.DpToCharge, _paramObj.Rebate, _paramObj.Qty, _paramObj.Qty2, _paramObj.QtyAdj,_paramObj.HlkQtyAdj, _paramObj.Rate,_paramObj.Rate2,_paramObj.QtyConsum,_paramObj.ConsumAverage,
                        _paramObj.QtyBilled,_paramObj.BookNo,_paramObj.NoOfFlats,_paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objConsumeDetail.set_Consumerdetail_save(_paramObj.Id, _paramObj.RefConsumerId, _paramObj.RefReaderId,
                        _paramObj.RefMeterSizeId, _paramObj.RefMeterStatusId, Convert.ToDateTime(_paramObj.ReadingDate), _paramObj.MeterReading, _paramObj.BillType, _paramObj.BillMode,
                        _paramObj.RefMeterTypeId, _paramObj.RefSupplyTypeId, _paramObj.RefSupplyCategoryId, _paramObj.PRevReadingDate, _paramObj.LastReading, _paramObj.BillNo, _paramObj.BillDate, _paramObj.PrevBillDate, _paramObj.DueDate,
                        _paramObj.AssessmentAmt, _paramObj.PrevAmount, _paramObj.DueAmount, _paramObj.Rent, _paramObj.PrevDPC, _paramObj.DueDPC,_paramObj.TillDateDPC,
                        _paramObj.DpToCharge, _paramObj.Rebate, _paramObj.Qty, _paramObj.Qty2, _paramObj.QtyAdj, _paramObj.HlkQtyAdj, _paramObj.Rate, _paramObj.Rate2, _paramObj.QtyConsum, _paramObj.ConsumAverage,
                        _paramObj.QtyBilled, _paramObj.BookNo, _paramObj.NoOfFlats,_paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.Id == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        return PartialView("ConsumeDetailListPartial");

                        //return Json(new { Result = "Success", msg = "Record successfully inserted!" });
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return PartialView("ConsumeDetailListPartial");
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
                return RedirectToAction("Manage", "ConsumeDetail");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int inID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "CONSUMEDETAIL")))
            {
                try
                {
                    bool _result = false;

                    #region To delete record in database
                    _result = Convert.ToBoolean(_objConsumeDetail.Consumerdetail_Delete(inID));

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

        public ActionResult AjaxHandler(clsJQueryDataTableParamModel param, int? pReaderId, DateTime? pReadingDate)
        {

            List<sp_ConsumeDetail_SelectWhere_Result> _objList;

            string _strwhere = "";

            _strwhere = " and CAST(ReadingDate as DATE) = CONVERT(date,'" + Convert.ToDateTime(pReadingDate).ToShortDateString() + "',103) and RefConsumerId in (select Id from ConsumerMaster c where c.RefReaderId =" + pReaderId + ") ";

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                _strwhere += " and (ConsumerNo like '%" + param.sSearch + "%' " +
                                  " or MeterNo like '%" + param.sSearch + "%' " +
                                  " or MeterStatus like '%" + param.sSearch + "%' " +
                                  " or ReadingDate like '%" + param.sSearch + "%' " +
                                  ")";
            }

            #region Sorting Started

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            string Sortingname = " order by ";

            switch (sortColumnIndex)
            {
                case 1:
                    Sortingname += "ConsumerNo " + sortDirection;
                    break;
                case 2:
                    Sortingname += "MeterNo " + sortDirection;
                    break;
                case 3:
                    Sortingname += "MeterStatus " + sortDirection;
                    break;
                case 4:
                    Sortingname += "ReadingDate " + sortDirection;
                    break;
                default:
                    Sortingname += "RefConsumerId " + sortDirection;
                    break;
            }
            _strwhere += Sortingname;

            #endregion

            _objList = _objConsumeDetail.get_Consumerdetail_SelectWhere(_strwhere).ToList();
            if (_objList != null && _objList.ToList().Count > 0)
            {
                var ConsumeValue = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var result = from c in ConsumeValue
                             select new[] { c.ConsumerNo, c.MeterNo, c.MeterStatus, 
                                            Convert.ToString(c.ReadingDate.ToShortDateString()), Convert.ToString(c.MeterReading), Convert.ToString(c.Id),Convert.ToString(c.RefMeterStatusId.ToString())};


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

        //[HttpPost]
        //public JsonResult isValueExists(int inID, string inValueName)
        //{
        //    try
        //    {
        //        if (_objUserMaster.isUserExists(inID, inValueName))
        //        {
        //            //TempData["Warning"] = "Value with the same name already exists!";
        //            return Json(new { Result = "Warning", msg = "Value with the same name already exists!" });
        //        }
        //        return Json(new { Result = "Success", msg = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        //TempData["Error"] = ex.ToString();
        //        return Json(new { Result = "Error", msg = ex.Message });
        //    }

        //}
        #endregion


        //temapary function create for testing Generate Bill Method
        //Start 
        #region TempGenerateBill
        //[HttpPost]
        //public JsonResult GentBill(ConsumeDetailModel _ObjConsumeDetail)
        //{
        //    try
        //    {
        //        clsConsumeDetail _ObjConsume = new clsConsumeDetail();
        //        int ictr = 0;
        //        GenerateBill _Objgb = new GenerateBill();
        //        foreach (sp_BG_GetConsumerDetailForGenerateBill_Result _Obj in _ObjConsume.get_BG_ConsumerdetailForBill(Convert.ToInt32(_ObjConsumeDetail.RefCampId), _ObjConsumeDetail.RefReaderId, _ObjConsumeDetail.OddEven, _ObjConsumeDetail.RefMeterStatusId).ToList())
        //        {
        //            _Objgb.GenBillNew(_Obj.ConsumerNo, Convert.ToDateTime(_ObjConsumeDetail.BillDate), Convert.ToInt32(_Obj.MeterReading), _ObjConsumeDetail.RefMeterStatusId ,Convert.ToInt32(_Obj.PrevRefStatusId), Convert.ToDateTime(_Obj.ReadingDate));
        //            ictr++;
        //        }

        //        return Json(new { _Message = " " ictr + " Bill Generate Successfully." ,Result = "true" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { _Message = ex.Message, Result = "false" }, JsonRequestBehavior.AllowGet);
        //    }
            
        //}
        

        #endregion
        //End
    }
}