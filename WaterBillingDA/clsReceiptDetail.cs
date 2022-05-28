using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsReceiptDetail
    {
        WaterBillingEntities _cnn;

        public clsReceiptDetail()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? SaveReceiptDetail(int pId,int pRefCollectionCenterId	,decimal pCounterNo	,int pReceiptNo	,DateTime pReceiptDate,int pRefConsumerId,
					decimal pRecAmt,int pRefPaymentTypeId,string pChequeNo,DateTime? pChequeDate,string pBankName,string pBankBranch,
                    string pIsChqStatus, int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? _retval = false;
            try
            {
                var _Obj = _cnn.sp_ReceiptDetail_Save(pId,pRefCollectionCenterId	,pCounterNo	,pReceiptNo	,pReceiptDate,pRefConsumerId,
					                                    pRecAmt,pRefPaymentTypeId,pChequeNo,pChequeDate,pBankName,pBankBranch,
                                                        pIsChqStatus, pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                _retval = true;
            }
            catch (Exception)
            {

                return _retval;
            }
            return _retval;
        }

        public bool? UpdateChequeStatus(int pId, string pIsChqStatus, string pChqBounceRefNo, DateTime? pChqBounceDate, decimal? pChqBounceCharge,
                                int pUpdUser, string pUpdTerminal)
        {
            bool? _retval = false;
            try
            {
                var _Obj = _cnn.sp_ReceiptDetail_SetChequeStatus(pId, pIsChqStatus, pChqBounceRefNo, pChqBounceDate, pChqBounceCharge,
                                pUpdUser, pUpdTerminal);
                _retval = true;
            }
            catch (Exception)
            {

                return _retval;
            }
            return _retval;
        }

        public bool? DeleteReceiptDetail(int pId)
        {
            bool? _retval = false;
            try
            {
                var _obj = _cnn.sp_ReceiptDetail_Delete(pId);
                _retval = true;
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_ReceiptDetail_Select_Result> GetReceiptDetail(int pId)
        {
            List<sp_ReceiptDetail_Select_Result> _retval = new List<sp_ReceiptDetail_Select_Result>();
            try
            {
                _retval = _cnn.sp_ReceiptDetail_Select(pId).ToList();
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_ReceiptDetail_SelectWhere_Result> GetReceiptDetailSelectWhere(string pConsditions)
        {
            List<sp_ReceiptDetail_SelectWhere_Result> _retval = new List<sp_ReceiptDetail_SelectWhere_Result>();
            try
            {
                _retval = _cnn.sp_ReceiptDetail_SelectWhere(pConsditions).ToList();
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }
    }
}

