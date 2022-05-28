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
    public class ChqBounceChargiesController : BaseController
    {
        clsChqBounceChargiesMaster _objChqBounceChargies = new clsChqBounceChargiesMaster();
        string _Message = string.Empty;
        //
        // GET: /ChqBounceChargies/


        public ActionResult Index()
        {
            // fill Bank Name
            //SelectList _objSupplyType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Bank);
            //ViewData["fillRefBankId"] = _objSupplyType;

            //fill effect date
            SelectList _objEffectDate = clsCommonUI.fillEffectDate_ChqBounceChargiesMaster();
            ViewData["fillEffectDate"] = _objEffectDate;

            return View();
        }

        [HttpGet]
        public PartialViewResult EffectDateListRetrive()
        {
            SelectList _objEffectDate = clsCommonUI.fillEffectDate_ChqBounceChargiesMaster();
            ViewData["fillEffectDate"] = _objEffectDate;

            return PartialView("DateSelectionPartial");
        }

        [HttpPost]
        public ActionResult SetupNewChargies(ChqBounceChargiesMasterModel _objParam)
        {
            List<ChqBounceChargiesMasterModel> _objModel = new List<ChqBounceChargiesMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CHQBOUNCECHARGIES", _objParam.EffectDate, _objParam.RefBankId)))
            {
                try
                {
                    if (_objParam.EffectDate != null && !string.IsNullOrEmpty(_objParam.EffectDate.ToString()))
                    {
                        _objParam.InsUser = clsCommonUI._User;
                        _objParam.InsTerminal = clsCommonUI._Terminal;
                        var _tempObj = _objChqBounceChargies.SetupNewChargiesforChqBounce(_objParam.EffectDate, _objParam.RefBankId, _objParam.InsUser, _objParam.InsTerminal).ToList();

                        if (_tempObj.Count > 0)
                        {
                            _objModel = LoadData(_tempObj);
                        }
                        else
                        {
                            TempData["Warning"] = "Reason Type Records Not available.";
                            return PartialView("LoadChqBounceChargiesPartial", _objModel);
                        }

                        if (_objModel == null)
                        {
                            _objModel = null;
                        }

                        SelectList _objEffectDate = clsCommonUI.fillEffectDate_ChqBounceChargiesMaster();
                        ViewData["fillEffectDate"] = _objEffectDate;
                        //TempData["Success"] = "Record Successfully Inserted!";
                        //Session["EffectDate"] = null;
                    }
                    else
                    {
                        return RedirectToAction("index", "ChqBounceChargies");
                    }

                }
                catch (Exception ex)
                {
                    //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
                }
                //return View(_objModel);

                return PartialView("LoadChqBounceChargiesPartial", _objModel);
            }
            else
            {
                TempData["Warning"] = "Insert or Update rights not given!";
                Index();
                return PartialView("LoadChqBounceChargiesPartial", _objModel);
            }

        }

        public List<ChqBounceChargiesMasterModel> LoadData(dynamic _ptempObj)
        {
            try
            {

                List<ChqBounceChargiesMasterModel> _objModel = new List<ChqBounceChargiesMasterModel>();

                foreach (var _obj in _ptempObj)
                {
                    _objModel.Add(new ChqBounceChargiesMasterModel
                    {

                        Id = _obj.Id,
                        EffectDate = _obj.EffectDate,
                        BankName = _obj.BankName,
                        RefBankId = _obj.RefBankId,
                        ReasonType = _obj.ReasonType,
                        RefReasonTypeID = _obj.RefReasonId,
                        Chargies = _obj.Chargies,
                        LastChargies = _obj.LastChargies,
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
        public ActionResult Save(List<ChqBounceChargiesMasterModel> _paramObj)
        {
            List<ChqBounceChargiesMasterModel> _objModel = new List<ChqBounceChargiesMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CHQBOUNCECHARGIES", _paramObj[0].EffectDate, _paramObj[0].RefBankId)))
            {
                try
                {

                    bool _result = false;
                    string _strResult = string.Empty;

                    #region To update rate in database

                    foreach (var _tempObj in _paramObj)
                    {
                        if (_tempObj.Chargies != 0)
                        {
                            _tempObj.UpdUser = clsCommonUI._User;
                            _tempObj.UpdTerminal = clsCommonUI._Terminal;

                            _result = Convert.ToBoolean(_objChqBounceChargies.saveChqBounceChargies(_tempObj.Id, _tempObj.Chargies, _tempObj.UpdUser, _tempObj.UpdTerminal));
                        }
                    }

                    if (_result)
                    {
                        //TempData["Success"] = "Record successfully updated!";
                        var _tempObj = _objChqBounceChargies.SelectChqBounceChargiesMaster(Convert.ToDateTime(_paramObj[0].EffectDate), _paramObj[0].RefBankId);
                        _objModel = LoadData(_tempObj);
                        TempData["Success"] = "Record Successfully Updated!";
                        return PartialView("LoadChqBounceChargiesPartial", _objModel);
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
                    return RedirectToAction("index", "ChqBounceChargies");
                    //return Json(new { Result = "Error", msg = ex.Message });
                }
            }
            else
            {
                TempData["Warning"] = "Update rights not given!";
                Index();
                return PartialView("LoadChqBounceChargiesPartial", _objModel);
            }
        }
    }
}