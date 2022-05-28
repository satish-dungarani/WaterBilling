using WaterBillingDB;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsChqBounceChargiesMaster
    {
         WaterBillingEntities _cnn;

        public clsChqBounceChargiesMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public List<sp_ChqBounceChargiesMaster_SetupNewChargies_Result> SetupNewChargiesforChqBounce(DateTime pEffectDate, int pRefBankId, int pInsUser, string pInsTerminal)
        {
            List<sp_ChqBounceChargiesMaster_SetupNewChargies_Result> retVal;
            try
            {
                retVal = _cnn.sp_ChqBounceChargiesMaster_SetupNewChargies(pEffectDate, pRefBankId, pInsUser, pInsTerminal).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_ChqBounceChargiesMaster_Select_Result> SelectChqBounceChargiesMaster(DateTime pEffectDate, int pRefBankId )
        {
            List<sp_ChqBounceChargiesMaster_Select_Result> retVal;
            try
            {
                retVal = _cnn.sp_ChqBounceChargiesMaster_Select(pEffectDate, pRefBankId).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? saveChqBounceChargies(int pID, decimal pChargies , int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                _cnn.sp_ChqBounceChargiesMaster_Save(pID, pChargies, pUpdUser, pUpdTerminal);
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

                return _cnn.ChqBounceChargiesMaster.GroupBy(x => x.EffectDate).OrderByDescending(x => x.Key).Select(x => x.Key).ToList();
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
