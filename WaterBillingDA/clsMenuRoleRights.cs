using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsMenuRoleRights
    {
        WaterBillingEntities _cnn;
        public clsMenuRoleRights()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? saveMenuRoleRights(int pRefRoleId, int pRefMenuId, bool? pCanInsert, bool? pCanUpdat, bool? pCanDelete, bool? pCanView, int pInsUser
            , string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? retval = false;
            try
            {
                _cnn.sp_MenuRoleRights_Save(pRefRoleId, pRefMenuId, pCanInsert, pCanUpdat, pCanDelete, pCanView, pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                retval = true;
            }
            catch (Exception)
            {

                return retval;
            }
            return retval;
        }

        public bool? deleteMenuRoleRights(int pRefRoleId)
        {
            bool? _retval = false;
            try
            {
                _cnn.sp_MenuRoleRights_Delete(pRefRoleId);
                _retval = true;
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_MenuRoleRights_Select_Result> getMenuRoleRights(int pRefRoleId)
        {
            List<sp_MenuRoleRights_Select_Result> retval = null;
            try
            {
                retval = _cnn.sp_MenuRoleRights_Select(pRefRoleId).ToList();
            }
            catch (Exception)
            {
                return retval;
            }
            return retval;
        }
    }
}
