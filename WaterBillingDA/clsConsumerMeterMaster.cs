using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsConsumerMeterMaster
    {
        WaterBillingEntities _cnn;

        public clsConsumerMeterMaster()
        {
            _cnn = new WaterBillingEntities();
        }
        public bool? saveConsumerMeterMaster(int pID, int pRefConsumerId, string pMeterNo, string pMeterMake, decimal pInitialReading,
            DateTime? pStartDate, DateTime? pInActiveDate, decimal pMaxReadingNo,
            int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_ConsumerMeterMaster_Save(pID, Convert.ToInt32(pRefConsumerId), pMeterNo, pMeterMake, pInitialReading,
            pStartDate, pInActiveDate, pMaxReadingNo, pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);

                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteConsumerMeterMaster(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_ConsumerMeterMaster_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public List<sp_ConsumerMeterMaster_Select_Result> getConsumerMeterMaster(int pID)
        {
            List<sp_ConsumerMeterMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_ConsumerMeterMaster_Select(pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_ConsumerMeterMaster_SelectWhere_Result> getConsumerMeterMasterWhere(string pInValue)
        {
            List<sp_ConsumerMeterMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_ConsumerMeterMaster_SelectWhere(pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool isConsumerMeterExists(int pID, string pValueName)
        {
            bool retVal = false;

            try
            {
                int _resp;
                if(pID == 0)
                {
                    _resp = _cnn.sp_ConsumerMeterMaster_SelectWhere(" and MeterNo =='" + pValueName.ToString() + "'").ToList().Count;
                }
                else
                {
                    _resp = _cnn.sp_ConsumerMeterMaster_SelectWhere(" and ID !=" + pID.ToString() + " and MeterNo='" + pValueName.Trim() + "'").ToList().Count;
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
