using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBilling.Models;
using WaterBillingDA;
using WaterBillingDB;
using System.IO;
using System.Net.Mail;

namespace WaterBilling.Controllers
{
    public class MeterMinChargeController : BaseController
    {
        clsMeterMinChargeMaster _objMeterMinCharge = new clsMeterMinChargeMaster();
        string _Message = string.Empty;
        //
        // GET: /MeterMinCharge/
        public ActionResult Index()
        {
            //fill supply type
            SelectList _objSupplyType = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.SupplyType);
            ViewData["fillRefSupplyTypeId"] = _objSupplyType;

            //fill effect date
            SelectList _objEffectDate = clsCommonUI.fillEffectiveDate_MeterMinCharge();
            ViewData["fillEffectDate"] = _objEffectDate;

            return View();
        }

        [HttpGet]
        public PartialViewResult EffectDateListRetrive()
        {
            SelectList _objEffectDate = clsCommonUI.fillEffectiveDate_MeterMinCharge();
            ViewData["fillEffectDate"] = _objEffectDate;

            return PartialView("DateSelectionPartial");
        }
        //public ActionResult SetupNewRate(string _effectDate, int _refSupplyTypeId)

        [HttpPost]
        public ActionResult SetupNewRate(MeterMinChargeMasterModel _objParam)
        
        {

            //SendMail("Outside Try");
            //SendMail(_objParam.EffectDate.ToString());
            //string _EffectDate = _effectDate;

            //string _EffectDate = Convert.ToDateTime(_effectDate + " 00:00:00").ToString("dd/MMM/yyyy");
            //DateTime _TempDate = DateTime.ParseExact(_Date, "MM/dd/yyyy", null);
            //DateTime _EffectDate = DateTime.Parse(_Date);

            List<MeterMinChargeMasterModel> _objModel = new List<MeterMinChargeMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "METERMINCHARGE", _objParam.EffectDate, _objParam.RefSupplyTypeId)))
            {
                try
                {
                    //SendMail("Inside Try");
                    //SendMail(_EffectDate);

                    if (_objParam.EffectDate != null && !string.IsNullOrEmpty(_objParam.EffectDate.ToString()))
                    {
                        //_objParam.InsUser = clsCommonUI._User;
                        //_objParam.InsTerminal = clsCommonUI._Terminal;

                        //var file = Path.Combine(Server.MapPath("~\\file\\"),"test.txt");
                        //var file = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(Server.MapPath("~\\file\\")), "test.txt");
                        //clsCommonUI.WriteToFile(file, _objParam.EffectDate.ToString());

                        //string _Date = _objParam.EffectDate.ToString("dd/MMM/yyyy");
                        //DateTime _EffectDate = DateTime.Parse(_Date);

                       

                        //var _tempObj = _objMeterMinCharge.SetupNewRateMeterMinChargeMaster(_objParam.EffectDate, _objParam.RefSupplyTypeId, _objParam.InsUser, _objParam.InsTerminal).ToList();
                        var _tempObj = _objMeterMinCharge.SetupNewRateMeterMinChargeMaster(_objParam.EffectDate, _objParam.RefSupplyTypeId, clsCommonUI._User, clsCommonUI._Terminal).ToList();

                        if (_tempObj.Count > 0)
                        {
                            _objModel = LoadData(_tempObj);
                        }
                        else
                        {
                            TempData["Warning"] = "Meter Size or Meter Status Records Not available.";
                            return PartialView("LoadMeterMinChargePartial", _objModel);
                        }


                        if (_objModel == null)
                        {
                            _objModel = null;
                        }

                        //TempData["Success"] = "Record Successfully Inserted!";
                        //Session["EffectDate"] = null;
                    }
                    else
                    {
                        return RedirectToAction("index", "MeterMinCharge");
                    }

                }
                catch (Exception ex)
                {
                    //SendMail(ex.ToString());
                    //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
                }
                //return View(_objModel);
                return PartialView("LoadMeterMinChargePartial", _objModel);
            }
            else
            {
                TempData["Warning"] = "Insert or Update rights not given!";
                Index();
                return PartialView("LoadMeterMinChargePartial", _objModel);
            }
        }

        //public void SendMail(string _Ed)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add("hpatel4u@gmail.com");
        //        mail.From = new MailAddress("demomail@jayamsoft.net");
        //        mail.Subject = "MeterMinCharge EffectDate " + _Ed;
        //        string Body = _Ed;
        //        mail.Body = Body;
        //        mail.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "jayamsoft.net";
        //        smtp.Port = 587;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential
        //        ("demomail@jayamsoft.net", "demo!@#123");// Enter seders User name and password
        //        smtp.EnableSsl = false;
        //        smtp.Send(mail);
        //    }
        //}

        public List<MeterMinChargeMasterModel> LoadData(dynamic _ptempObj)
        {
            try
            {

                List<MeterMinChargeMasterModel> _objModel = new List<MeterMinChargeMasterModel>();

                foreach (var _obj in _ptempObj)
                {
                    _objModel.Add(new MeterMinChargeMasterModel
                    {

                        Id = _obj.ID,
                        EffectDate = _obj.EffectDate,
                        SupplyType = _obj.SupplyType,
                        RefSupplyTypeId = _obj.RefSupplyTypeId,
                        MeterSize = _obj.MeterSize,
                        RefMeterSizeId = _obj.RefMeterSizeID,
                        MeterStatus = _obj.MeterStatus,
                        RefMeterStatusId = _obj.RefMeterStatusId,
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
        public ActionResult Save(List<MeterMinChargeMasterModel> _paramObj)
        {
            List<MeterMinChargeMasterModel> _objModel = new List<MeterMinChargeMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "METERMINCHARGE", Convert.ToDateTime(_paramObj[0].EffectDate), _paramObj[0].RefSupplyTypeId)))
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

                            _result = Convert.ToBoolean(_objMeterMinCharge.saveMeterMinChargeMaster(_tempObj.Id, _tempObj.Rate, _tempObj.UpdUser, _tempObj.UpdTerminal));
                        }
                    }

                    if (_result)
                    {
                        //TempData["Success"] = "Record successfully updated!";
                        var _tempObj = _objMeterMinCharge.SelectMeterMinChargeData(Convert.ToDateTime(_paramObj[0].EffectDate), _paramObj[0].RefSupplyTypeId);
                        _objModel = LoadData(_tempObj);
                        TempData["Success"] = "Record Successfully Updated!";
                        return PartialView("LoadMeterMinChargePartial", _objModel);
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
                    return RedirectToAction("index", "MeterMinCharge");
                    //return Json(new { Result = "Error", msg = ex.Message });
                }
            }
            else
            {
                TempData["Warning"] = "Update rights not given!";
                Index();
                return PartialView("LoadMeterMinChargePartial", _objModel);
            }
        }
    }
}

