using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBilling.Models;
using WaterBillingDB;
using System.Net.Mail;
using CrystalDecisions.CrystalReports.Engine;
using WaterBilling.DataSets;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using System.IO;

namespace WaterBilling.Controllers
{
    public class ConsumerController : BaseController
    {
        clsConsumerMeterMaster _objConsumerMeterMaster = new clsConsumerMeterMaster();
        clsConsumerMaster _objConsumerMaster = new clsConsumerMaster();
        clsUserMaster _objUserMaster = new clsUserMaster();
        string _Message = string.Empty;

        //
        // GET: /Consumer/
        public ActionResult Index()
        {
            Session["ConsumerId"] = null;
            return View();
        }

        public ActionResult editSession(int? pId)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "CONSUMER")))
            {
                Session["ConsumerId"] = pId;
                return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _Message = "Update Rights not given!";
                return Json(new { Saved = "No", _Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult editSessionMeter(int? pId)
        {
            Session["ConsumerMeterId"] = pId;
            return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Manage()
        {
            bool result = true;
            if (Session["ConsumerId"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CONSUMER"));
            }
            if (result)
            {
                SelectList _objZoneType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Zone);
                ViewData["fillZoneId"] = _objZoneType;

                SelectList _objCampType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Camp);
                ViewData["fillCampId"] = _objCampType;

                SelectList _objConstructionType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.ConstructionType);
                ViewData["fillConstructionType"] = _objConstructionType;

                SelectList _objMeterSizeId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.MeterSize);
                ViewData["fillMeterSizeId"] = _objMeterSizeId;

                SelectList _objMeterTypeId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.MeterType);
                ViewData["fillMeterTypeId"] = _objMeterTypeId;

                SelectList _objSupplyTypeId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.SupplyType);
                ViewData["fillSupplyTypeId"] = _objSupplyTypeId;

                SelectList _objSupplyCategoryId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.SupplyCategory);
                ViewData["fillSupplyCategoryId"] = _objSupplyCategoryId;

                SelectList _objDMAId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.DMA);
                ViewData["fillDMAId"] = _objDMAId;

                SelectList _objCityId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CityId);
                ViewData["fillCityId"] = _objCityId;

                SelectList _objStateId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.StateId);
                ViewData["fillStateId"] = _objStateId;

                SelectList _objCountryId = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.CountryId);
                ViewData["fillCountryId"] = _objCountryId;

                SelectList _objPrevStatus = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.MeterStatus);
                ViewData["fillPrevStatus"] = _objPrevStatus;

                //List<SelectListItem> SelectList = new List<SelectListItem>();
                //foreach (var _objReader in _objUserMaster.getUserMasterWhere(" and RefUserTypeId = " + (int)clsCommonUI.MasterType.ReaderId))
                //{
                //    SelectList.Add(new SelectListItem { Selected = false, Text = _objReader.FullName, Value = _objReader.ID.ToString() });
                //}
                SelectList _objReaderId = clsCommonUI.fillReaderName_UserMaster();
                ViewData["fillReaderId"] = _objReaderId;

                ConsumerMasterModel _obj = new ConsumerMasterModel();
                try
                {
                    string _Condition = string.Empty;
                    int pID = 0;
                    if (Session["ConsumerId"] != null && !string.IsNullOrEmpty(Session["ConsumerId"].ToString()))
                        pID = Convert.ToInt32(Session["ConsumerId"].ToString());

                    if (pID == -1)
                    {
                        _Condition = " Order by Id desc";
                    }
                    else
                    {
                        _Condition = " and Id = " + pID;
                    }

                    _obj = _objConsumerMaster.getConsumerMasterWhere(_Condition).Select(x => new ConsumerMasterModel
                    {

                        Id = x.Id,
                        ConsumerNo = x.ConsumerNo,
                        OldConsumerNo = x.OldConsumerNo,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        FirstNameMarathi = x.FirstNameMarathi,
                        MiddleNameMarathi = x.MiddleNameMarathi,
                        LastNameMarathi = x.LastNameMarathi,
                        Address = x.Address,
                        RefCityId = Convert.ToInt32(x.RefCityId),
                        RefStateId = Convert.ToInt32(x.RefStateId),
                        RefCountryId = Convert.ToInt32(x.RefCountryID),
                        PinCode = x.PinCode,
                        MobileNo = x.MobileNo,
                        EmailID = x.EmailId,
                        Contact1 = x.Contact1,
                        Contact2 = x.Contact2,
                        RefZoneId = Convert.ToInt32(x.RefZoneId),
                        RefCampId = Convert.ToInt32(x.RefCampId),
                        RefReaderId = Convert.ToInt32(x.RefReaderId),
                        NoOfConnections = Convert.ToInt32(x.NoOfConnections),
                        FamilyMember = Convert.ToInt32(x.FamilyMember),
                        AverageConsumption = Convert.ToDecimal(x.AverageConsumption),
                        Deposite = Convert.ToDecimal(x.Deposite),
                        RefConstructionType = Convert.ToInt32(x.RefConstructionType),
                        RefMeterSizeId = Convert.ToInt32(x.RefMeterSizeId),
                        RefMeterTypeId = Convert.ToInt32(x.RefMeterTypeId),
                        RefSupplyTypeId = Convert.ToInt32(x.RefSupplyTypeId),
                        RefSupplyCategoryId = Convert.ToInt32(x.RefSupplyCategoryId),
                        BookNo = x.BookNo,
                        BeatNo = x.BeatNo,
                        FolioNo = Convert.ToDecimal(x.FolioNo),
                        OddEven = x.OddEven,
                        RefDMAId = Convert.ToInt32(x.RefDMAId),
                        ConnectionDate = x.ConnectionDate,
                        SancRef = x.SancRef,
                        LetterNo = x.LetterNo,
                        LetterDate = x.LetterDate,
                        TRNo = x.TRNo,
                        TRDate = x.TRDate,
                        PlotId = x.PlotId,
                        PlumberName = x.PlumberName,
                        PlumberLicNo = x.PlumberLicNo,
                        DisconnectedDate = x.DisconnectedDate,
                        DisconnectionOrderNo = x.DisconnectionOrderNo,
                        DisconnectionOrderDate = x.DisconnectionOrderDate,
                        PrevBillDate = x.PrevBillDate,
                        PrevReadingDate = x.PrevReadingDate,
                        PrevReading = Convert.ToDecimal(x.PrevReading),
                        PrevAssessmentAmt = Convert.ToDecimal(x.PrevAssessmentAmt),
                        PrevAmount = Convert.ToDecimal(x.PrevAmount),
                        PrevDPC = Convert.ToDecimal(x.PrevDPC),
                        PrevRent = Convert.ToDecimal(x.PrevRent),
                        PrevRefStatusId = Convert.ToInt32(x.PrevRefStatusId),
                        PrevStatus = x.PrevStatus,
                        ReConnectedDate = x.ReConnectedDate,
                        ReconnectedOrderNo = x.ReconnectedOrderNo,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_obj == null)
                    {
                        _obj = new ConsumerMasterModel();
                    }

                    ViewBag.ConsumerMeter = ManageMeter();
                    if (ViewBag.ConsumerMeter == null)
                    {
                        ViewBag.ConsumerMeter = new ConsumerMeterMasterModel();
                    }
                }
                catch (Exception ex)
                {
                    //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
                }
                //Session["ConsumerId"] = null;
                return View(_obj);
            }
            else
            {
                TempData["Warning"] = "Insert rights not given!";
                Index();
                return View("Index");
            }
        }

        [HttpGet]
        public ConsumerMeterMasterModel ManageMeter()
        {


            ConsumerMeterMasterModel _obj = new ConsumerMeterMasterModel();
            try
            {
                string _Condtion = string.Empty;
                int pID = 0;
                if (Session["ConsumerMeterId"] != null && !string.IsNullOrEmpty(Session["ConsumerMeterId"].ToString()))
                    pID = Convert.ToInt32(Session["ConsumerMeterId"].ToString());

                if (pID == 0)
                {
                    _Condtion = " and RefConsumerId = " + Convert.ToInt32(Session["ConsumerId"]);
                }
                else if (pID == -1 || pID == -2)
                {
                    _Condtion = " and 1 = 0";
                }
                else
                {
                    _Condtion = " and Id = " + pID;
                }

                _obj = _objConsumerMeterMaster.getConsumerMeterMasterWhere(_Condtion).Select(x => new ConsumerMeterMasterModel
                {

                    MeterId = x.Id,
                    RefConsumerId = x.RefConsumerId,
                    MeterNo = x.MeterNo,
                    MeterMake = x.MeterMake,
                    InitialReading = Convert.ToDecimal(x.InitialReading),
                    StartDate = x.StartDate,
                    InActiveDate = x.InActiveDate,
                    MaxReadingNo = Convert.ToDecimal(x.MaxReadingNo),
                    InsUser = x.InsUser,
                    InsDate = x.InsDate,
                    InsTerminal = x.InsTerminal,
                    UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                    UpdDate = x.UpdDate,
                    UpdTerminal = x.UpdTerminal

                }).LastOrDefault();

                if (_obj == null)
                {
                    _obj = new ConsumerMeterMasterModel();
                }

                Session["ConsumerMeterId"] = null;

            }
            catch (Exception ex)
            {
                //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
            }
            return _obj;
            //return View("MeterDetailPartial");
        }

        [HttpPost]
        public ActionResult Save(ConsumerMasterModel _paramObj)
        {

            try
            {

                int _result = 0;
                string _strResult = string.Empty;

                #region To insert record in database

                if (_paramObj.Id == 0)
                {

                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToInt32(_objConsumerMaster.saveConsumerMaster(_paramObj.Id, _paramObj.ConsumerNo, _paramObj.OldConsumerNo,
                                _paramObj.FirstName, _paramObj.MiddleName, _paramObj.LastName, _paramObj.FirstNameMarathi, _paramObj.MiddleNameMarathi,
                                _paramObj.LastNameMarathi, _paramObj.Address, _paramObj.RefCityId, _paramObj.RefStateId, _paramObj.RefCountryId,
                                _paramObj.PinCode, _paramObj.MobileNo, _paramObj.EmailID, _paramObj.Contact1, _paramObj.Contact2, _paramObj.RefZoneId, _paramObj.RefCampId,
                                _paramObj.RefReaderId, _paramObj.NoOfConnections, _paramObj.FamilyMember, _paramObj.AverageConsumption, _paramObj.Deposite,
                                _paramObj.RefConstructionType, _paramObj.RefMeterSizeId, _paramObj.RefMeterTypeId, _paramObj.RefSupplyTypeId,
                                _paramObj.RefSupplyCategoryId, _paramObj.BookNo, _paramObj.BeatNo, _paramObj.FolioNo, _paramObj.OddEven, _paramObj.RefDMAId,
                                _paramObj.ConnectionDate, _paramObj.SancRef, _paramObj.LetterNo, _paramObj.LetterDate, _paramObj.TRNo,
                                _paramObj.TRDate, _paramObj.PlotId, _paramObj.PlumberName, _paramObj.PlumberLicNo, _paramObj.DisconnectedDate,
                                _paramObj.DisconnectionOrderNo, _paramObj.DisconnectionOrderDate,
                                _paramObj.PrevBillDate, _paramObj.PrevReadingDate, _paramObj.PrevReading, _paramObj.PrevAssessmentAmt, _paramObj.PrevAmount, _paramObj.PrevDPC,
                                _paramObj.PrevRent, _paramObj.PrevRefStatusId, _paramObj.ReConnectedDate, _paramObj.ReconnectedOrderNo,
                                _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));

                    _paramObj.Id = _result;

                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();


                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToInt32(_objConsumerMaster.saveConsumerMaster(_paramObj.Id, _paramObj.ConsumerNo, _paramObj.OldConsumerNo, _paramObj.FirstName,
                                _paramObj.MiddleName, _paramObj.LastName, _paramObj.FirstNameMarathi, _paramObj.MiddleNameMarathi, _paramObj.LastNameMarathi, _paramObj.Address,
                                _paramObj.RefCityId, _paramObj.RefStateId, _paramObj.RefCountryId, _paramObj.PinCode, _paramObj.MobileNo,
                                _paramObj.EmailID, _paramObj.Contact1, _paramObj.Contact2, _paramObj.RefZoneId, _paramObj.RefCampId, _paramObj.RefReaderId,
                                _paramObj.NoOfConnections, _paramObj.FamilyMember, _paramObj.AverageConsumption, _paramObj.Deposite,
                                _paramObj.RefConstructionType, _paramObj.RefMeterSizeId, _paramObj.RefMeterTypeId, _paramObj.RefSupplyTypeId,
                                _paramObj.RefSupplyCategoryId, _paramObj.BookNo, _paramObj.BeatNo, _paramObj.FolioNo, _paramObj.OddEven, _paramObj.RefDMAId,
                                _paramObj.ConnectionDate, _paramObj.SancRef, _paramObj.LetterNo, _paramObj.LetterDate, _paramObj.TRNo,
                                _paramObj.TRDate, _paramObj.PlotId, _paramObj.PlumberName, _paramObj.PlumberLicNo, _paramObj.DisconnectedDate,
                                _paramObj.DisconnectionOrderNo, _paramObj.DisconnectionOrderDate,
                                _paramObj.PrevBillDate, _paramObj.PrevReadingDate, _paramObj.PrevReading, _paramObj.PrevAssessmentAmt, _paramObj.PrevAmount, _paramObj.PrevDPC,
                                _paramObj.PrevRent, _paramObj.PrevRefStatusId, _paramObj.ReConnectedDate, _paramObj.ReconnectedOrderNo,
                                _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result != 0)
                {
                    if (_paramObj.Id == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        //return View("Manage");
                        //return Json(new { _Result = _RefConsumerId },JsonRequestBehavior.AllowGet);
                        return Json(new { Result = "Success", msg = "Record successfully inserted!" });
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        //return View("Manage");
                        //return Json(new { _Result = _RefConsumerId }, JsonRequestBehavior.AllowGet);
                        return Json(new { Result = "Success", msg = "Record successfully updated!" });
                    }
                }
                else
                {
                    TempData["Warning"] = "There was some server error. Please try again later!";
                    //return View("Manage");
                    return Json(new { Result = "Fail", msg = "There was some server error. Please try again later!" });
                    //return RedirectToAction("index", "Consumer");
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Warning"] = ex.Message;
                return RedirectToAction("index", "Consumer");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveMeterDetail(ConsumerMeterMasterModel _paramObj)
        {

            try
            {
                bool _result = false;
                string _strResult = string.Empty;


                #region To insert record in database

                if (_paramObj.MeterId == 0)
                {

                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objConsumerMeterMaster.saveConsumerMeterMaster(_paramObj.MeterId, _paramObj.RefConsumerId,
                        _paramObj.MeterNo, _paramObj.MeterMake, _paramObj.InitialReading, _paramObj.StartDate, _paramObj.InActiveDate,
                        _paramObj.MaxReadingNo, _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();

                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objConsumerMeterMaster.saveConsumerMeterMaster(_paramObj.MeterId, _paramObj.RefConsumerId,
                        _paramObj.MeterNo, _paramObj.MeterMake, _paramObj.InitialReading, _paramObj.StartDate, _paramObj.InActiveDate,
                        _paramObj.MaxReadingNo, _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.MeterId == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";
                        //return RedirectToAction("index", "ConsumerMeter");
                        return Json(new { });
                        //return View("Manage");
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        //return View("Manage");
                        //return RedirectToAction("index", "ConsumerMeter");
                        return Json(new { });
                    }
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    //return View("Manage");
                    return Json(new { Result = "Success", msg = "There was some server error. Please try again later!" });
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                //return RedirectToAction("index", "ConsumerMeter");
                return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int delID)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "DELETE", "CONSUMER")))
            {
                try
                {
                    bool _result = false;

                    List<sp_ConsumerMeterMaster_SelectWhere_Result> _objExiest = new List<sp_ConsumerMeterMaster_SelectWhere_Result>();

                    #region To delete record from database
                    _objExiest = _objConsumerMeterMaster.getConsumerMeterMasterWhere(" and RefConsumerId = " + delID).ToList();
                    if (_objExiest.Count > 0)
                    {
                        _Message = "Please delete first Consumer meter records.";
                        return Json(new { _result = false, _Message }, JsonRequestBehavior.AllowGet);
                    }

                    _result = Convert.ToBoolean(_objConsumerMaster.deleteConsumerMaster(delID));

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
        public ActionResult DeleteMeterDetail(int delID)
        {
            try
            {
                bool _result = false;


                #region To delete record from database
                _result = Convert.ToBoolean(_objConsumerMeterMaster.deleteConsumerMeterMaster(delID));

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

        public ActionResult AjaxHandler(clsJQueryDataTableParamModel param)
        {
            try
            {
                List<sp_ConsumerMaster_SelectWhere_Result> _objList;

                string _strwhere = "";
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    _strwhere += " and (FullName like '%" + param.sSearch + "%' " +
                                      " or ConsumerNo like '%" + param.sSearch + "%' " +
                                      " or MobileNo like '%" + param.sSearch + "%' " +
                                      " or ZoneType like '%" + param.sSearch + "%' " +
                                      " or OddEven like '%" + param.sSearch + "%' " +
                                      " or BookNo like '%" + param.sSearch + "%' " +
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
                        Sortingname += "ConsumerNo " + sortDirection;
                        break;
                    case 3:
                        Sortingname += "MobileNo" + sortDirection;
                        break;
                    case 4:
                        Sortingname += "ZoneType" + sortDirection;
                        break;
                    case 5:
                        Sortingname += "OddEven" + sortDirection;
                        break;
                    case 6:
                        Sortingname += "BookNo" + sortDirection;
                        break;
                    default:
                        Sortingname += "FullName " + sortDirection;
                        break;
                }
                _strwhere += Sortingname;

                #endregion

                _objList = _objConsumerMaster.getConsumerMasterWhere(_strwhere).ToList();
                if (_objList != null && _objList.ToList().Count > 0)
                {
                    var ConsumerMaster = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                    var result = from c in ConsumerMaster
                                 select new[] { c.ConsumerNo,  c.FullName, c.MobileNo, 
                                            c.ZoneType, c.OddEven, c.BookNo,  
                                            Convert.ToString(c.Id) };
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(new { _result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region DisplayGrid
        public ActionResult AjaxHandlerMeter(clsJQueryDataTableParamModel param, int _Consumerid)
        {
            try
            {

                List<sp_ConsumerMeterMaster_SelectWhere_Result> _objList;

                string _strwhere = " and RefConsumerId = " + _Consumerid;
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    _strwhere += " and (MeterNo like '%" + param.sSearch + "%' " +
                                      " or MeterMake like '%" + param.sSearch + "%' " +
                                      " or InitialReading like '%" + param.sSearch + "%' " +
                                      " or StartDate like '%" + param.sSearch + "%' " +
                                      " or InActiveDate like '%" + param.sSearch + "%' " +
                                      " or MaxReadingNo like '%" + param.sSearch + "%' " +
                                      ")";
                }

                #region Sorting Started

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                string Sortingname = " order by ";

                switch (sortColumnIndex)
                {
                    case 1:
                        Sortingname += "MeterNo " + sortDirection;
                        break;
                    case 2:
                        Sortingname += "MeterMake " + sortDirection;
                        break;
                    case 3:
                        Sortingname += "InitialDate " + sortDirection;
                        break;
                    case 4:
                        Sortingname += "StartDate " + sortDirection;
                        break;
                    case 5:
                        Sortingname += "InActiveDate " + sortDirection;
                        break;
                    case 6:
                        Sortingname += "MaxReadingNo " + sortDirection;
                        break;
                    default:
                        Sortingname += "MeterNo " + sortDirection;
                        break;
                }
                _strwhere += Sortingname;

                #endregion

                _objList = _objConsumerMeterMaster.getConsumerMeterMasterWhere(_strwhere).ToList();
                if (_objList != null && _objList.ToList().Count > 0)
                {
                    var ConsumerMeterMaster = _objList.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                    var result = from c in ConsumerMeterMaster
                                 where c.RefConsumerId == _Consumerid
                                 select new[] { c.MeterNo,  c.MeterMake, Convert.ToString(c.InitialReading), 
                                            Convert.ToString(c.StartDate), Convert.ToString(c.InActiveDate), Convert.ToString(c.MaxReadingNo),  
                                            Convert.ToString(c.Id), Convert.ToString(c.RefConsumerId) };
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(new { _result = false }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Validation

        [HttpPost]
        public JsonResult isValueExists(int inID, string inValueName)
        {
            try
            {
                if (_objConsumerMaster.isConsumerExists(inID, inValueName))
                {
                    //TempData["Warning"] = "Value with the same name already exists!";
                    return Json(new { Result = "Warning", msg = "Value with the same Consumer Number already exists!" });
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

        #region Validation

        [HttpPost]
        public JsonResult isValueExistsMeter(int inID, string inValueName)
        {
            try
            {
                if (_objConsumerMeterMaster.isConsumerMeterExists(inID, inValueName))
                {
                    //TempData["Warning"] = "Value with the same name already exists!";
                    return Json(new { Result = "Warning", msg = "Value with the same Meter Number already exists!" });
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

        public ActionResult ConsumerInformationPartial(string ConsumerNo = "")
        {
            try
            {
                ConsumerMasterModel _obj = new ConsumerMasterModel();
                _obj = _objConsumerMaster.getConsumerMasterWhere(" and ConsumerNo ='" + ConsumerNo + "'").Select(x => new ConsumerMasterModel
                    {

                        Id = x.Id,
                        ConsumerNo = x.ConsumerNo,
                        OldConsumerNo = x.OldConsumerNo,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        FirstNameMarathi = x.FirstNameMarathi,
                        MiddleNameMarathi = x.MiddleNameMarathi,
                        LastNameMarathi = x.LastNameMarathi,
                        Address = x.Address,
                        RefCityId = Convert.ToInt32(x.RefCityId),
                        RefStateId = Convert.ToInt32(x.RefStateId),
                        RefCountryId = Convert.ToInt32(x.RefCountryID),
                        PinCode = x.PinCode,
                        MobileNo = x.MobileNo,
                        EmailID = x.EmailId,
                        Contact1 = x.Contact1,
                        Contact2 = x.Contact2,
                        RefZoneId = Convert.ToInt32(x.RefZoneId),
                        RefReaderId = Convert.ToInt32(x.RefReaderId),
                        NoOfConnections = Convert.ToInt32(x.NoOfConnections),
                        FamilyMember = Convert.ToInt32(x.FamilyMember),
                        AverageConsumption = Convert.ToDecimal(x.AverageConsumption),
                        Deposite = Convert.ToDecimal(x.Deposite),
                        RefConstructionType = Convert.ToInt32(x.RefConstructionType),
                        RefMeterSizeId = Convert.ToInt32(x.RefMeterSizeId),
                        RefMeterTypeId = Convert.ToInt32(x.RefMeterTypeId),
                        RefSupplyTypeId = Convert.ToInt32(x.RefSupplyTypeId),
                        RefSupplyCategoryId = Convert.ToInt32(x.RefSupplyCategoryId),
                        BookNo = x.BookNo,
                        BeatNo = x.BeatNo,
                        FolioNo = Convert.ToDecimal(x.FolioNo),
                        OddEven = x.OddEven,
                        RefDMAId = Convert.ToInt32(x.RefDMAId),
                        ConnectionDate = x.ConnectionDate,
                        SancRef = x.SancRef,
                        LetterNo = x.LetterNo,
                        LetterDate = x.LetterDate,
                        TRNo = x.TRNo,
                        TRDate = x.TRDate,
                        PlotId = x.PlotId,
                        PlumberName = x.PlumberName,
                        PlumberLicNo = x.PlumberLicNo,
                        DisconnectedDate = x.DisconnectedDate,
                        DisconnectionOrderNo = x.DisconnectionOrderNo,
                        DisconnectionOrderDate = x.DisconnectionOrderDate,
                        PrevBillDate = x.PrevBillDate,
                        PrevReadingDate = x.PrevReadingDate,
                        PrevReading = Convert.ToDecimal(x.PrevReading),
                        PrevAssessmentAmt = Convert.ToDecimal(x.PrevAssessmentAmt),
                        PrevAmount = Convert.ToDecimal(x.PrevAmount),
                        PrevDPC = Convert.ToDecimal(x.PrevDPC),
                        PrevRent = Convert.ToDecimal(x.PrevRent),
                        PrevRefStatusId = Convert.ToInt32(x.PrevRefStatusId),
                        PrevStatus = x.PrevStatus,
                        ReConnectedDate = x.ReConnectedDate,
                        ReconnectedOrderNo = x.ReconnectedOrderNo,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                return PartialView(_obj);
            }
            catch (Exception)
            {

                return PartialView();
            }
        }

        #region Report
        public void ReportView()
        {
            try
            {

                ReportDocument _report = new ReportDocument();
                _report.Load(Server.MapPath("/Reports/rptList.rpt"));
                ds_List _Ds = new ds_List();
                //_Ds.DataSetName = "dTableList";
                
                //Retrieve data from table and fill in _Ds
                SqlDataAdapter _Da = new SqlDataAdapter(clsCommon.ReportData());
                _Da.Fill(_Ds, "dTableList");
                //_Ds = (ds_List)clsCommon.ReportData();
                _report.SetDataSource(_Ds);
                _report.SetParameterValue("ReportHeading", "List Report");
                _report.SetParameterValue("DivisionName", "Division");

                CrystalReportViewer _Crv = new CrystalReportViewer();
                _Crv.ReportSource = _report;
                _Crv.Visible = true;
                _Crv.DataBind();
                _Crv.DisplayPage = true;
                _Crv.DisplayToolbar = true;
                _Crv.DisplayStatusbar = true;
                //Response.Buffer = false;
                //Response.ClearContent();
                //Response.ClearHeaders();

                //Stream stream = _report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "List.pdf");

                _report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}