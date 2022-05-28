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
    public class BoardRentController : BaseController
    {
        clsBoardRentMaster _objBoardRent = new clsBoardRentMaster();
        string _Message = string.Empty;

        public ActionResult Index()
        {

            SelectList _obj = clsCommonUI.fillEffectiveDate_BoardRate();
            ViewData["fillEffectDate"] = _obj;

            if (_obj.Count() > 0)
            {
                Session["EffectDate"] = _obj.Select(x => x.Value).FirstOrDefault().ToString();
            }
            return View();
        }

        #region Transactin Events

        [HttpGet]
        public PartialViewResult EffectDateListRetrive()
        {
            SelectList _obj = clsCommonUI.fillEffectiveDate_BoardRate();
            ViewData["fillEffectDate"] = _obj;

            return PartialView("DateSelectionPartial");
        }
        public ActionResult editSession(DateTime pEffectiveDate)
        {
            //if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "CONSUMERRATE", _objParam.EffectDate)))
            //{
                Session["EffectDate"] = pEffectiveDate;
                return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    _Message = "Update Rights not given!";
            //    return Json(new { Saved = "No", _Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult generateNewDate(BoardRentMasterModel _objParam)
        {
            List<BoardRentMasterModel> _objModel = new List<BoardRentMasterModel>();
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "BOARDRENT", _objParam.EffectDate)))
            {
                Session["EffectDate"] = _objParam.EffectDate;
                var tempFlag = _objBoardRent.generateNewEffectiveDate(_objParam.EffectDate, -1, "web");
                 _objModel = loadDataPartial();
                 return PartialView("loadDataPartial", _objModel);
                //return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Warning"] = "Insert or Update rights not given!";
                Index();
                return PartialView("loadDataPartial", _objModel);
            }
        }

        public List<BoardRentMasterModel> loadDataPartial()
        {
            List<BoardRentMasterModel> _objModel = new List<BoardRentMasterModel>();
            try
            {
                if (Session["EffectDate"] != null && !string.IsNullOrEmpty(Session["EffectDate"].ToString()))
                {
                    DateTime pEffectDate = Convert.ToDateTime(Session["EffectDate"].ToString());

                    var _tempObj = _objBoardRent.getBoardRentMaster(pEffectDate).ToList();

                    foreach (var _obj in _tempObj)
                    {
                        _objModel.Add(new BoardRentMasterModel
                        {

                            ID = _obj.ID,
                            EffectDate = _obj.EffectDate,
                            RefMeterTypeId = _obj.refMeterTypeID,
                            MeterType = _obj.MeterType,
                            RefMeterSizeId = _obj.refMeterSizeID,
                            MeterSize = _obj.MeterSize,
                            oldRate = _obj.oldRate != 0 ? Convert.ToDecimal(_obj.oldRate) : 0,
                            Rate = _obj.Rate,
                            InsUser = _obj.InsUser,
                            InsDate = _obj.InsDate,
                            InsTerminal = _obj.InsTerminal,
                            UpdUser = _obj.UpdUser.HasValue ? _obj.UpdUser.Value : 0,
                            UpdDate = _obj.UpdDate,
                            UpdTerminal = _obj.UpdTerminal

                        });
                    }

                    if (_objModel == null)
                    {
                        _objModel = null;
                    }

                    //Session["EffectDate"] = null;
                }
                else
                {
                    return _objModel;
                }

            }
            catch (Exception ex)
            {
                //TempData[clsCommon.MessageType.Error.ToString()] = ex.Message;
            }
            //return View(_objModel);
            return _objModel;
        }

        [HttpPost]
        public ActionResult Save(List<BoardRentMasterModel> _paramObj)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "BOARDRENT", _paramObj[0].EffectDate)))
            {
                try
                {
                    bool _result = false;
                    string _strResult = string.Empty;

                    #region To insert record in database



                    //_paramObj.InsUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                    //_paramObj.InsTerminal = Session["loggedInSystem"].ToString();

                    foreach (var _tempObj in _paramObj)
                    {
                        if (_tempObj.ID == 0)
                        {
                            _tempObj.InsUser = clsCommonUI._User;
                            _tempObj.InsTerminal = clsCommonUI._Terminal;

                            _result = Convert.ToBoolean(_objBoardRent.saveBoardRentMaster(_tempObj.ID, _tempObj.EffectDate, _tempObj.RefMeterTypeId, _tempObj.RefMeterSizeId,
                                         _tempObj.Rate, _tempObj.InsUser, _tempObj.InsTerminal, _tempObj.UpdUser, _tempObj.UpdTerminal));
                        }
                        else
                        {
                            //_paramObj.UpdUser = Convert.ToInt32(Session["loggedInUserID"].ToString());
                            //_paramObj.UpdTerminal = Session["loggedInSystem"].ToString();
                            _tempObj.UpdUser = clsCommonUI._User;
                            _tempObj.UpdTerminal = clsCommonUI._Terminal;

                            _result = Convert.ToBoolean(_objBoardRent.saveBoardRentMaster(_tempObj.ID, _tempObj.EffectDate, _tempObj.RefMeterTypeId, _tempObj.RefMeterSizeId,
                                         _tempObj.Rate, _tempObj.InsUser, _tempObj.InsTerminal, _tempObj.UpdUser, _tempObj.UpdTerminal));
                        }
                    }


                    if (_result)
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return PartialView("loadDataPartial", loadDataPartial());
                    }
                    else
                    {
                        TempData["Error"] = "There was some server error. Please try again later!";
                        return PartialView("loadDataPartial", loadDataPartial());
                        //return Json(new { Result = "Success", msg = "There was some server error. Please try again later!" });
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("index", "home");
                    //return Json(new { Result = "Error", msg = ex.Message });
                }
            }
            else
            {
                TempData["Warning"] = "Update rights not given!";
                Index();
                return PartialView("loadDataPartial", loadDataPartial());
            }
        }

        [HttpPost]
        public ActionResult Delete(DateTime inEffectiveDate)
        {

            try
            {
                bool _result = false;

                #region To insert record in database
                _result = Convert.ToBoolean(_objBoardRent.deleteBoardRentMaster(inEffectiveDate));

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

        #endregion
    }
}