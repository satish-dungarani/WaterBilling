using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsBoardRentMaster
    {
        WaterBillingEntities _cnn;

        public clsBoardRentMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? saveBoardRentMaster(int pID, DateTime pEffectDate, int pRefMeterTypeID, int pRefMeterSizeID,
                            decimal pRate, int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_BoardRentMaster_Save(pID, pEffectDate, pRefMeterTypeID, pRefMeterSizeID,
                                                 pRate, pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteBoardRentMaster(DateTime pEffectDate)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_BoardRentMaster_Delete(pEffectDate);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public List<sp_BoardRentMaster_Select_Result> getBoardRentMaster(DateTime pEffectDate)
        {
            List<sp_BoardRentMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_BoardRentMaster_Select(pEffectDate).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_BoardRentMaster_SelectWhere_Result> getBoardRentMasterSelectWhere(string pCondition)
        {
            List<sp_BoardRentMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_BoardRentMaster_SelectWhere(pCondition).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? generateNewEffectiveDate(DateTime pEffectDate, int pUserID, string pTerminal)
        {
            bool? retVal = false;

            try
            {
                int x = _cnn.sp_BoardRentMaster_SetupNewDate(pEffectDate, pUserID, pTerminal);
                retVal = true;
            }
            catch (Exception)
            {
            }

            return retVal;
        }

        public List<DateTime> get_Distinct_EffectiveDates()
        {
            try
            {

                return _cnn.BoardRentMaster.GroupBy(x => x.EffectDate).OrderByDescending(x => x.Key).Select(x => x.Key).ToList();

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
