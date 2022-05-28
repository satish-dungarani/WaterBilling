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
    public class ConsumerRateController : BaseController
    {

        clsConsumerRateMaster _objConsumerRate = new clsConsumerRateMaster();
        string _Message = string.Empty;
        //
        // GET: /ConsumerRate/
        public ActionResult Index()
        {
            // fill supply type
            SelectList _objSupplyType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.SupplyType);
            ViewData["fillRefSupplyTypeId"] = _objSupplyType;

            //fill effect date
            SelectList _objEffectDate = clsCommonUI.fillEffectiveDate_ConsumerRate();
            ViewData["fillEffectDate"] = _objEffectDate;

            return View();
        }

        [HttpGet]
        public PartialViewResult EffectDateListRetrive()
        {
            SelectList _objEffectDate = clsCommonUI.fillEffectiveDate_ConsumerRate();
            ViewData["fillEffectDate"] = _objEffectDate;

            return PartialView("DateSelectionPartial");
        }

        [HttpPost]
        public ActionResult SetupNewRate(ConsumerRateMasterModel _objParam)
        {
            //string _Date = _objParam.EffectDate.ToString("MM/dd/yyyy");
            //_objParam.EffectDate = DateTime.ParseExact(_Date, "MM/dd/yyyy", null);
            List<ConsumerRateMasterModel> _objModel = new List<ConsumerRateMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CONSUMERRATE", _objParam.EffectDate, _objParam.RefSupplyTypeId)))
            {
                try
                {
                    if (_objParam.EffectDate != null && !string.IsNullOrEmpty(_objParam.EffectDate.ToString()))
                    {
                        _objParam.InsUser = clsCommonUI._User;
                        _objParam.InsTerminal = clsCommonUI._Terminal;

                        

                        var _tempObj = _objConsumerRate.SetupNewRateConsumerRateMaster(_objParam.EffectDate, _objParam.RefSupplyTypeId, _objParam.InsUser, _objParam.InsTerminal).ToList();

                        if (_tempObj.Count > 0)
                        {
                            _objModel = LoadData(_tempObj);
                        }
                        else
                        {
                            TempData["Warning"] = "Supply Category Records Not available.";
                            return PartialView("LoadConsumerRatePartial", _objModel);
                        }

                        if (_objModel == null)
                        {
                            _objModel = null;
                        }

                        SelectList _objEffectDate = clsCommonUI.fillEffectiveDate_ConsumerRate();
                        ViewData["fillEffectDate"] = _objEffectDate;
                        //TempData["Success"] = "Record Successfully Inserted!";
                        //Session["EffectDate"] = null;
                    }
                    else
                    {
                        return RedirectToAction("index", "ConsumerRate");
                    }

                }
                catch (Exception ex)
                {
                    //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
                }
                //return View(_objModel);

                return PartialView("LoadConsumerRatePartial", _objModel);
            }
            else
            {
                TempData["Warning"] = "Insert or Update rights not given!";
                Index();
                return PartialView("LoadConsumerRatePartial", _objModel);
            }

        }

        public List<ConsumerRateMasterModel> LoadData(dynamic _ptempObj)
        {
            try
            {

                List<ConsumerRateMasterModel> _objModel = new List<ConsumerRateMasterModel>();

                foreach (var _obj in _ptempObj)
                {
                    _objModel.Add(new ConsumerRateMasterModel
                    {

                        Id = _obj.Id,
                        EffectDate = _obj.EffectDate,
                        SupplyType = _obj.SupplyType,
                        RefSupplyTypeId = _obj.RefSupplyTypeId,
                        SupplyCategory = _obj.SupplyCategory,
                        RefSupplyCategoryId = _obj.RefSupplyCategoryId,
                        Rate = _obj.Rate,
                        LastRate = _obj.LastRate,
                        InsUser = _obj.InsUser,
                        InsDate = _obj.InsDate,
                        InsTerminal = _obj.InsTerminal,

                    });
                }

                return _objModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Save(List<ConsumerRateMasterModel> _paramObj)
        {
            List<ConsumerRateMasterModel> _objModel = new List<ConsumerRateMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CONSUMERRATE", _paramObj[0].EffectDate, _paramObj[0].RefSupplyTypeId)))
            {
                try
                {

                    bool _result = false;
                    string _strResult = string.Empty;

                    #region To update rate in database

                    foreach (var _tempObj in _paramObj)
                    {
                        if (_tempObj.Rate != 0)
                        {
                            _tempObj.UpdUser = clsCommonUI._User;
                            _tempObj.UpdTerminal = clsCommonUI._Terminal;

                            _result = Convert.ToBoolean(_objConsumerRate.saveConsumerRateMaster(_tempObj.Id, _tempObj.Rate, _tempObj.UpdUser, _tempObj.UpdTerminal));
                        }
                    }

                    if (_result)
                    {
                        //TempData["Success"] = "Record successfully updated!";
                        var _tempObj = _objConsumerRate.SelectConsumerRateData(Convert.ToDateTime(_paramObj[0].EffectDate), _paramObj[0].RefSupplyTypeId);
                        _objModel = LoadData(_tempObj);
                        TempData["Success"] = "Record Successfully Updated!";
                        return PartialView("LoadConsumerRatePartial", _objModel);
                        //return RedirectToAction("index", "MeterMinCharge");
                    }
                    else
                    {
                        //TempData["Error"] = "There was some server error. Please try again later!";
                        return View();
                        //return Json(new { Result = "Success", msg = "There was some server error. Please try again later!" });
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("index", "ConsumerRate");
                    //return Json(new { Result = "Error", msg = ex.Message });
                }
            }
            else
            {
                TempData["Warning"] = "Update rights not given!";
                Index();
                return PartialView("LoadConsumerRatePartial", _objModel);
            }
        }
    }
}