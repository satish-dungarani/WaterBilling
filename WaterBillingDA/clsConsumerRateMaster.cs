using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsConsumerRateMaster
    {
        WaterBillingEntities _cnn;

        public clsConsumerRateMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public List<sp_ConsumeRateMaster_SetupNewRate_Result> SetupNewRateConsumerRateMaster(DateTime pEffectDate, int pRefSupplyTypeID, int pInsUser, string pInsTerminal)
        {
            List<sp_ConsumeRateMaster_SetupNewRate_Result> retVal;
            try
            {
                retVal = _cnn.sp_ConsumeRateMaster_SetupNewRate(pEffectDate, pRefSupplyTypeID, pInsUser, pInsTerminal).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_ConsumerRateMaster_Select_Result> SelectConsumerRateData(DateTime pEffectDate, int pRefSupplyTypeID )
        {
            List<sp_ConsumerRateMaster_Select_Result> retVal;
            try
            {
                retVal = _cnn.sp_ConsumerRateMaster_Select(pEffectDate, pRefSupplyTypeID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_ConsumeRateMaster_SelectWhere_Result> SelectConsumerRateSelectWhere(string pCondition)
        {
            List<sp_ConsumeRateMaster_SelectWhere_Result> retVal;
            try
            {
                retVal = _cnn.sp_ConsumeRateMaster_SelectWhere(pCondition).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? saveConsumerRateMaster(int pID, decimal pRate, int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                _cnn.sp_ConsumerRateMaster_Save(pID, pRate, pUpdUser, pUpdTerminal);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public List<DateTime> get_Distinct_EffectiveDates()
        {
            try
            {

                return _cnn.ConsumeRateMaster.GroupBy(x => x.EffectDate).OrderByDescending(x => x.Key).Select(x => x.Key).ToList();
                //return _cnn.ConsumeRateMaster.OrderByDescending(x => x.EffectDate).Select(x => x.EffectDate).Distinct().ToList();

                //return (from date in _cnn.BoardRentMaster
                //                       group date by date.EffectDate into dategroup
                //                       select  dategroup.Key).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
