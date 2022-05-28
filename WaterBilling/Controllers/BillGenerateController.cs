using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBilling.Models;
using WaterBillingDB;
using System.Data;
using WaterBilling.DataSets;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System.IO;

namespace WaterBilling.Controllers
{
    public class BillGenerateController : BaseController
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
        //public ActionResult editSession(int? pID)
        //{
        //    if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "CONSUMEDETAIL")))
        //    {
        //        Session["ConsumeID"] = pID;
        //        return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        _Message = "Update Rights not given!";
        //        return Json(new { Saved = "No", _Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public PartialViewResult RetriveReaderList(int? pCampId)
        {
            try
            {
                if (pCampId != null)
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


        public ActionResult ManageBill()
        {
            bool result = true;
            result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "BILLGENERATE"));
            if (result)
            {
                SelectList _objCamp = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.Camp);
                ViewData["fillCamp"] = _objCamp;

                SelectList _objReaderName = clsCommonUI.fillReaderName_UserMaster();
                ViewData["fillReaderName"] = _objReaderName;

                SelectList _objMeterStatus = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.MeterStatus);
                ViewData["fillMeterStatus"] = _objMeterStatus;

                ConsumeDetailModel _obj = new ConsumeDetailModel();
                try
                {
                    if (_obj == null)
                    {
                        _obj = new ConsumeDetailModel();
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

        #endregion


        //temapary function create for testing Generate Bill Method
        //Start 
        #region TempGenerateBill
        [HttpPost]
        public ActionResult GentBill(ConsumeDetailModel _ObjConsumeDetail)
        {
            try
            {
                ds_BillDetails _Ds = new ds_BillDetails();
                clsConsumeDetail _ObjConsume = new clsConsumeDetail();
                int ictr = 0;
                GenerateBill _Objgb = new GenerateBill();
                foreach (sp_BG_GetConsumerDetailForGenerateBill_Result _Obj in _ObjConsume.get_BG_ConsumerdetailForBill(Convert.ToInt32(_ObjConsumeDetail.RefCampId), _ObjConsumeDetail.RefReaderId, _ObjConsumeDetail.OddEven, _ObjConsumeDetail.RefMeterStatusId).ToList())
                {
                    DataSet _Dst = new DataSet();
                    GenerateBill._Id = Convert.ToInt32(_Obj.Id);
                    GenerateBill.x_DueDate = Convert.ToDateTime(_ObjConsumeDetail.DueDate);
                    _Dst = _Objgb.GenBillNew(_Obj.ConsumerNo, Convert.ToDateTime(_ObjConsumeDetail.BillDate), Convert.ToInt32(_Obj.MeterReading), _ObjConsumeDetail.RefMeterStatusId, Convert.ToInt32(_Obj.PrevRefStatusId), Convert.ToDateTime(_Obj.ReadingDate));

                    DataRow _Dr = _Ds.dTableBill.NewRow();
                    _Ds.dTableBill.ImportRow(_Dr);
                    //for (int i = 0; i < _Dt.Columns.Count; i++)
                    //{

                    if (_Dst.Tables[0].Rows.Count > 0)
                    {
                        _Ds.dTableBill.ImportRow(_Dst.Tables[0].Rows[0]);


                    }

                    foreach (DataRow _ObjRow in _Dst.Tables[1].Rows)
                    {
                        DataRow _Drs = _Ds.dTableReceipt.NewRow();
                        _Ds.dTableReceipt.ImportRow(_Drs);
                        _Ds.dTableReceipt.ImportRow(_ObjRow);
                    }
                    //}

                    ictr++;
                }
                //BillPrint(_Ds);

                if (ictr == 0)
                {
                    return Json(new { _Message = " Not available Bill For Generate.", Result = "false" }, JsonRequestBehavior.AllowGet);
                }
                Session["BillList"] = _Ds;
                return Json(new { _Message = " " + ictr + " Bill Generate Successfully.", Result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { _Message = ex.Message, Result = "false" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BillPrint(int pRefCampId, int pRefReaderId, string pOddEven, int pRefMeterStatusId, DateTime pBillDate, DateTime pDueDate)
        {
            try
            {
                DataSet _Ds1 = new DataSet();
                //ds_BillDetails _Ds = new ds_BillDetails();
                ds_BillDetails.dTableBillDataTable _DtBill = new ds_BillDetails.dTableBillDataTable();
                _Ds1 = _objConsumeDetail.GetBillDetail(pRefCampId, pRefReaderId, pOddEven, pRefMeterStatusId, pBillDate, pDueDate);
                _Ds1.Tables[0].TableName = "dTableBill";
                _Ds1.Tables[1].TableName = "dTableReceipt";

                _Ds1.Relations.Add(_Ds1.Tables[0].Columns["ConsumerId"], _Ds1.Tables[1].Columns["ConsumerId"]);

                ReportDocument _report = new ReportDocument();
                _report.Load(Server.MapPath("/Reports/rptFinalBill.rpt"));
                //ds_BillDetails _Ds = (ds_BillDetails)Session["BillList"];
                if (_Ds1.Tables["dTableBill"].Rows.Count > 0)
                {
                    _report.SetDataSource(_Ds1);
                    //_report.SetParameterValue("ReportHeading", "List Report");
                    _report.SetParameterValue("Division", "Division");

                    CrystalReportViewer _Crv = new CrystalReportViewer();
                    _Crv.ReportSource = _report;
                    _Crv.Visible = true;
                    _Crv.DataBind();
                    _Crv.DisplayPage = true;
                    _Crv.DisplayToolbar = true;
                    _Crv.DisplayStatusbar = true;

                    //_report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,System.Web.HttpContext.Current.Response,false,"BillReport");

                    //Stream _Rpt = _report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //return new FileStreamResult(_Rpt, "application/pdf");
                }
                Stream stream = _report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult GetBillPrintDetail()
        {
            try
            {
                DataSet _Ds1 = new DataSet();
                //ds_BillDetails _Ds = new ds_BillDetails();
                ds_BillDetails.dTableBillDataTable _DtBill = new ds_BillDetails.dTableBillDataTable();
                _Ds1 = (DataSet)Session["BillList"];

                ReportDocument _report = new ReportDocument();
                _report.Load(Server.MapPath("/Reports/rptFinalBill.rpt"));
                if (_Ds1.Tables["dTableBill"].Rows.Count > 0)
                {
                    _report.SetDataSource(_Ds1);
                    //_report.SetParameterValue("ReportHeading", "List Report");
                    _report.SetParameterValue("Division", "Division");

                    CrystalReportViewer _Crv = new CrystalReportViewer();
                    _Crv.ReportSource = _report;
                    _Crv.Visible = true;
                    _Crv.DataBind();
                    _Crv.DisplayPage = true;
                    _Crv.DisplayToolbar = true;
                    _Crv.DisplayStatusbar = true;

                    //_report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,System.Web.HttpContext.Current.Response,false,"BillReport");

                    //Stream _Rpt = _report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //return new FileStreamResult(_Rpt, "application/pdf");
                }
                Stream stream = _report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        //End
    }
}