using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace WaterBillingDA
{
    public class clsConsumeDetail
    {
        WaterBillingEntities _cnn;
        public clsConsumeDetail()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? set_Consumerdetail_save(int? pId, int pRefConsumerId, int pRefReaderId, int? pRefMeterSizeId, int pRefMeterStatusId, DateTime pReadingDate
            , decimal pMeterReading, string pBillType, string pBillMode, int? pRefMeterTypeId, int? pRefSupplayTypeId, int? pRefSupplayCategoryId, DateTime? pPRevReadingDate, decimal? pLastReading,
            decimal? pBillNo, DateTime? pBillDate, DateTime? pPrevBillDate, DateTime? pDueDate, decimal? pAssessmentAmt, decimal? pPrevAmount, decimal? pDueAmount, decimal? pRent,
            decimal? pPrevDPC, decimal? pDueDPC, decimal? pTillDateDPC, decimal? pDpcToCharge, decimal? pRebate, int? pQty, int? pQty2, int? pQtyAdj, int? pHlkQtyAdj, decimal? pRate, decimal? pRate2,
            int? pQtyConsum, int? pConsumAverage, int? pQtyBilled, string pBookNo, int? pNoOfFlats,
            int pInsUser, string pInsTerminal, int? pUpdUser, string pUpdTerminal)
        {
            bool _retval = false;
            try
            {
                var _Obj = _cnn.sp_ConsumeDetail_Save(pId, pRefConsumerId, pRefMeterStatusId, pRefReaderId, pRefMeterSizeId, pReadingDate,
                    pMeterReading, pBillType, pBillMode, pRefMeterTypeId, pRefSupplayTypeId, pRefSupplayCategoryId, pPRevReadingDate, pLastReading,
            pBillNo, pBillDate, pPrevBillDate, pDueDate, pAssessmentAmt, pPrevAmount, pDueAmount, pRent,
            pPrevDPC, pDueDPC, pTillDateDPC, pDpcToCharge, pRebate, pQty, pQty2, pQtyAdj, pHlkQtyAdj, pRate, pRate2,
            pQtyConsum, pConsumAverage, pQtyBilled, pBookNo, pNoOfFlats,
            pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                _retval = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _retval;
        }

        public List<sp_ConsumeDetail_Select_Result> get_Consumerdetail_Select(int pId)
        {
            List<sp_ConsumeDetail_Select_Result> _Objselect;
            try
            {
                _Objselect = _cnn.sp_ConsumeDetail_Select(pId).ToList();
                return _Objselect;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<sp_ConsumeDetail_SelectWhere_Result> get_Consumerdetail_SelectWhere(string pWhereCon)
        {
            List<sp_ConsumeDetail_SelectWhere_Result> _Objselectwhere;
            try
            {
                _Objselectwhere = _cnn.sp_ConsumeDetail_SelectWhere(pWhereCon).ToList();
                //if (_Objselectwhere.Count == 0)
                //    throw new Exception("Generate bill for this Consumer No before receipt creation.");
                return _Objselectwhere;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_BG_GetConsumerDetailForGenerateBill_Result> get_BG_ConsumerdetailForBill(int pCampId, int pReaderId, string pOddEven, int pMeterStatusId)
        {
            List<sp_BG_GetConsumerDetailForGenerateBill_Result> _ObjBillDetail;
            try
            {
                _ObjBillDetail = _cnn.sp_BG_GetConsumerDetailForGenerateBill(pCampId, pReaderId, pOddEven, pMeterStatusId).ToList();
                return _ObjBillDetail;
            }
            catch (Exception)
            {
                return _ObjBillDetail = new List<sp_BG_GetConsumerDetailForGenerateBill_Result>();
            }
        }

        public bool? Consumerdetail_Delete(int pId)
        {
            bool _retval = false;
            try
            {
                _cnn.sp_ConsumeDetail_Delete(pId);
                _retval = true;
            }
            catch (Exception)
            {

                return false;
            }
            return _retval;
        }

        public DataSet GetBillDetail(int pRefCampId, int pRefReaderId, string pOddEven, int pRefMeterStatusId, DateTime pBillDate, DateTime pDueDate)
        {
            try
            {
                DataSet _Ds = new DataSet();
                #region SetData In Datatable
                
                Database _ObjDb = new SqlDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                using (DbCommand _objCmd = _ObjDb.GetStoredProcCommand("sp_BG_GetBillDetail"))
                {
                    _ObjDb.AddInParameter(_objCmd, "@pRefCampId", DbType.Int32,pRefCampId);
                    _ObjDb.AddInParameter(_objCmd, "@pRefReaderId", DbType.Int32,pRefReaderId);
                    _ObjDb.AddInParameter(_objCmd, "@pOddEven", DbType.String,pOddEven);
                    _ObjDb.AddInParameter(_objCmd, "@pMeterStatusId", DbType.Int32,pRefMeterStatusId);
                    _ObjDb.AddInParameter(_objCmd, "@pIssueDate", DbType.Date,pBillDate);
                    _ObjDb.AddInParameter(_objCmd, "@pDueDate", DbType.Date,pDueDate);

                    _Ds = _ObjDb.ExecuteDataSet(_objCmd);
                }
                if (_Ds.Tables[0].Rows.Count == 0)
                    throw new Exception("No any Bill available for Generate");
                #endregion

                return _Ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
