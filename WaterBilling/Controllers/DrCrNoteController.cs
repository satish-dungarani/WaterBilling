using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBillingDB;
using WaterBilling.Models;

namespace WaterBilling.Controllers
{
    public class DrCrNoteController : Controller
    {
        clsDrCrNote _objDrCrNote = new clsDrCrNote();
        string _Message = string.Empty;
        static string FileName = null;
        //
        // GET: /DrCrNote/
        //public ActionResult Index()
        //{
        //    return View();
        //}


        #region Transactin Events


        public JsonResult editSession(string pType)
        {
            if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "DRCRNOTE")))
            {
                Session["DrCr"] = pType;
                return Json(new { _Result = "true", _Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _Message = "Insert Rights not given!";
                return Json(new { _Result = "false", _Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Manage()
        {
            bool result = false;
            if (Session["DrCr"] != null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividualInMapPath((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "ManageDrCrNote('" + Session["DrCr"].ToString() + "')"));
            }
            if (result)
            {
                SelectList _objReason = clsCommonUI.fillReasonName_ReasonMaster(Session["DrCr"].ToString());
                ViewData["fillReason"] = _objReason;

                if (Session["DrCr"].ToString().ToUpper() == "DN")
                    ViewBag.Header = "Debit Note";
                else if (Session["DrCr"].ToString().ToUpper() == "CN")
                    ViewBag.Header = "Credit Note";

                DrCrNoteModel _obj = new DrCrNoteModel();
                try
                {
                    _obj = _objDrCrNote.GetDrCrNoteDetail(0).Select(x => new DrCrNoteModel
                    {
                        Id = x.Id,
                        NoteType = x.NoteType,
                        SerialNo = x.SerialNo,
                        RefConsumerId = Convert.ToInt32(x.RefConsumerId),
                        NoteDate = x.NoteDate,
                        Amount = Convert.ToDecimal(x.Amount),
                        Narration = x.Narration,
                        RefReasonId = x.RefReasonId,
                        OrderNo = x.OrderNo,
                        OrderDate = x.OrderDate,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_obj == null)
                    {
                        _obj = new DrCrNoteModel();
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
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Save(DrCrNoteModel _paramObj)
        {
            try
            {

                bool _result = false;
                string _strResult = string.Empty;

                #region To insert record in database

                if (_paramObj.Id == 0)
                {
                    _paramObj.NoteType = Session["DrCr"].ToString().ToUpper();
                    _paramObj.InsUser = clsCommonUI._User;
                    _paramObj.InsTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objDrCrNote.SaveDrCrNote(_paramObj.Id, _paramObj.NoteType, _paramObj.SerialNo, _paramObj.RefConsumerId,
                                _paramObj.NoteDate, _paramObj.Amount, _paramObj.Narration, _paramObj.RefReasonId, _paramObj.OrderNo,
                                _paramObj.OrderDate, FileName, _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }
                else
                {
                    _paramObj.NoteType = Session["DrCr"].ToString().ToUpper();
                    _paramObj.UpdUser = clsCommonUI._User;
                    _paramObj.UpdTerminal = clsCommonUI._Terminal;

                    _result = Convert.ToBoolean(_objDrCrNote.SaveDrCrNote(_paramObj.Id, _paramObj.NoteType, _paramObj.SerialNo, _paramObj.RefConsumerId,
                                _paramObj.NoteDate, _paramObj.Amount, _paramObj.Narration, _paramObj.RefReasonId, _paramObj.OrderNo,
                                _paramObj.OrderDate, FileName, _paramObj.InsUser, _paramObj.InsTerminal, _paramObj.UpdUser, _paramObj.UpdTerminal));
                }

                if (_result)
                {
                    if (_paramObj.Id == 0)
                    {
                        TempData["Success"] = "Record successfully inserted!";

                        //return RedirectToAction("Manage", "ReceiptDetail");
                        return PartialView("LoadLastTwoDrCrNotePartial", GetLastTwoDrCrNotePartial());
                    }
                    else
                    {
                        TempData["Success"] = "Record successfully updated!";
                        return PartialView("LoadLastTwoDrCrNotePartial", GetLastTwoDrCrNotePartial());
                    }
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    return PartialView("LoadLastTwoDrCrNotePartial", GetLastTwoDrCrNotePartial());
                }

                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Manage", "DrCrNote");
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpLoadFile(HttpPostedFileBase attfile)
        {

            try
            {
                FileName = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase _File = null;
                    _File = Request.Files[0];
                    if (_File.ContentType != "application/pdf")
                        throw new Exception();

                    if (_File != null && _File.ContentLength > 0)
                    {
                        //string FileName = Path.GetFileName(_File.FileName);
                        FileName = _objDrCrNote.GetUniqueId() + ".pdf";
                        string PathForSave = Server.MapPath("~/Files");
                        if (!Directory.Exists(PathForSave))
                        {
                            Directory.CreateDirectory(PathForSave);
                        }

                        _File.SaveAs(Path.Combine(PathForSave, FileName));
                    }
                }
                return Json(new { Result = "Success", Data = FileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(new { Result = "Fail", Data = FileName, Message = "File Uploading Fail, Wrong file Attach." }, JsonRequestBehavior.AllowGet);
                //return Json(new { Result = "Error", msg = ex.Message });
            }
        }
        #endregion

        public List<sp_DrCrNote_SelectWhere_Result> GetLastTwoDrCrNotePartial()
        {
            List<sp_DrCrNote_SelectWhere_Result> _Obj = new List<sp_DrCrNote_SelectWhere_Result>();
            try
            {
                string _Condition = " and Upper(NoteType) = '" + Session["DrCr"].ToString().ToUpper() + "' and Cast(InsDate as Date) = Cast(GETDATE() as Date) and InsUser = " + clsCommonUI._User + " order by Id Desc";
                var _ObjTemp = _objDrCrNote.GetDrCrNoteSelectWhere(_Condition);
                for (int i = 0; i < 2; i++)
                {
                    _Obj.Add(_ObjTemp[i]);
                }
                return _Obj;
            }
            catch (Exception)
            {
                return _Obj;
            }
        }

        [HttpGet]
        public PartialViewResult DisplayConsumerDetailPartial(string pConsumerNo)
        {
            clsConsumerMaster _ObjConsumerMaster = new clsConsumerMaster();
            try
            {
                var _Obj = _ObjConsumerMaster.getConsumerMasterWhere(" and ConsumerNo = '" + pConsumerNo + "'").Select(x => new DrCrNoteModel
                {
                    RefConsumerId = x.Id,
                    ConsumerName = x.FullName,
                    ConsumerAddress = x.Address
                }).FirstOrDefault();

                return PartialView(_Obj);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
    }
}