using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsMeterMinChargeMaster
    {
        WaterBillingEntities _cnn;

        public clsMeterMinChargeMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public List<sp_MeterMinChargeMaster_SetupNewRate_Result> SetupNewRateMeterMinChargeMaster(DateTime pEffectDate, int pRefSupplyTypeID, int pInsUser, string pInsTerminal)
        {
            List<sp_MeterMinChargeMaster_SetupNewRate_Result> retVal;
            try
            {
                retVal = _cnn.sp_MeterMinChargeMaster_SetupNewRate(pEffectDate, pRefSupplyTypeID, pInsUser, pInsTerminal).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_MeterMinChargeMaster_Select_Result> SelectMeterMinChargeData(DateTime pEffectDate, int pRefSupplyTypeID )
        {
            List<sp_MeterMinChargeMaster_Select_Result> retVal;
            try
            {
                retVal = _cnn.sp_MeterMinChargeMaster_Select(pEffectDate, pRefSupplyTypeID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_MeterMinChargeMaster_SelectWhere_Result> getMeterMinChargeSelectWhere(string pCondition)
        {
            List<sp_MeterMinChargeMaster_SelectWhere_Result> retVal;
            try
            {
                retVal = _cnn.sp_MeterMinChargeMaster_SelectWhere(pCondition).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? saveMeterMinChargeMaster(int pID, decimal pRate, int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                _cnn.sp_MeterMinChargeMaster_Save(pID, pRate, pUpdUser, pUpdTerminal);
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

                return _cnn.MeterMinChargeMaster.GroupBy(x => x.EffectDate).OrderByDescending(x => x.Key).Select(x => x.Key).ToList();
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

