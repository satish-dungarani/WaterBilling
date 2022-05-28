using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsConsumerMaster
    {
        WaterBillingEntities _cnn;

        public clsConsumerMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public int? saveConsumerMaster(int pID, string pConsumerNo, string pOldConsumerNo, string pFirstName, string pMiddleName, string pLastName,
            string pFirstNameMarathi, string pMiddleNameMarathi, string pLastNameMarathi, string pAddress,
            int pRefCityId, int pRefStateId, int pRefCountryId, string pPinCode, string pMobileNo,
            string pEmailID, string pContact1, string pContact2, int pRefZoneId, int pRefCampId, int pRefReaderId,
            int pNoOfConnections, int pFamilyMember, decimal pAverageConsumption, decimal pDeposite,
            int pRefConstructionType, int pRefMeterSizeId, int pRefMeterTypeId, int pRefSupplyTypeId,
            int pRefSupplyCategoryId, string pBookNo, string pBeatNo, decimal pFolioNo, string pOddEven, int pRefDMAId,
            DateTime? pConnectionDate, string pSancRef, string pLetterNo, DateTime? pLetterDate, string pTRNo,
            DateTime? pTRDate, string pPlotId, string pPlumberName, string pPlumberLicNo, DateTime? pDisconnectedDate,
            string pDisconnectionOrderNo, DateTime? pDisconnectionOrderDate,
            DateTime? pPrevBillDate, DateTime? pPrevReadingDate, decimal pPrevReading, decimal pPrevAssessmentAmt, decimal pPrevAmount,
            decimal pPrevDPC, decimal pPrevRent, int? pPrevRefStatusId, DateTime? pReConnectedDate, string pReconnectedOrderNo,
            int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            Nullable<int> retVal = 0;
            try
            {

                retVal = Convert.ToInt32(_cnn.sp_ConsumerMaster_Save(pID, pConsumerNo, pOldConsumerNo, pFirstName, pMiddleName, pLastName,
            pFirstNameMarathi, pMiddleNameMarathi, pLastNameMarathi, pAddress,
            pRefCityId, pRefStateId, pRefCountryId, pPinCode, pMobileNo,
            pEmailID, pContact1, pContact2, pRefZoneId, pRefCampId, pRefReaderId,
            pNoOfConnections, pFamilyMember, pAverageConsumption, pDeposite,
            pRefConstructionType, pRefMeterSizeId, pRefMeterTypeId, pRefSupplyTypeId,
            pRefSupplyCategoryId, pBookNo, pBeatNo, pFolioNo, pOddEven, pRefDMAId,
            pConnectionDate, pSancRef, pLetterNo, pLetterDate, pTRNo,
            pTRDate, pPlotId, pPlumberName, pPlumberLicNo, pDisconnectedDate,
            pDisconnectionOrderNo, pDisconnectionOrderDate,
            pPrevBillDate, pPrevReadingDate, pPrevReading, pPrevAssessmentAmt, pPrevAmount,
            pPrevDPC, pPrevRent, pPrevRefStatusId, pReConnectedDate, pReconnectedOrderNo,
            pInsUser, pInsTerminal, pUpdUser, pUpdTerminal).FirstOrDefault().Value.ToString());

            }
            catch (Exception)
            {
                retVal = 0;
            }

            return retVal;
        }

        public bool? deleteConsumerMaster(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_ConsumerMaster_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public List<sp_ConsumerMaster_Select_Result> getConsumerMaster(int pID)
        {
            List<sp_ConsumerMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_ConsumerMaster_Select(pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_ConsumerMaster_SelectWhere_Result> getConsumerMasterWhere(string pInValue)
        {
            List<sp_ConsumerMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_ConsumerMaster_SelectWhere(pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool isConsumerExists(int pID, string pValueName)
        {
            bool retVal = false;

            try
            {
                int _resp;
                if (pID == 0)
                {
                    _resp = _cnn.sp_ConsumerMaster_SelectWhere(" and ConsumerNo ='" + pValueName.Trim() + "'").ToList().Count;
                }
                else
                {
                    _resp = _cnn.sp_ConsumerMaster_SelectWhere(" and ID !=" + pID.ToString() + " and ConsumerNo='" + pValueName.Trim() + "'").ToList().Count;
                }

                if (_resp > 0)
                {
                    retVal = true;
                }
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

    }
}
