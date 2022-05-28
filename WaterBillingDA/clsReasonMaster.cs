using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsReasonMaster
    {
        WaterBillingEntities _cnn;

        public clsReasonMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? saveReasonMaster(int pID, int refReasonTypeID, string ReasonName,
                            int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_ReasonMaster_Save(pID, refReasonTypeID, ReasonName, 
                                                     pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteReasonMaster(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_ReasonMaster_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public List<sp_ReasonMaster_Select_Result> getReasonMaster(int pID)
        {
            List<sp_ReasonMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_ReasonMaster_Select(pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }


        public List<sp_ReasonMaster_SelectWhere_Result> getReasonMasterWhere(string pInValue)
        {
            List<sp_ReasonMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_ReasonMaster_SelectWhere(pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }
    }
}
