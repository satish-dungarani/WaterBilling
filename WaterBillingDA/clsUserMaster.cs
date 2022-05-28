using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class clsUserMaster
    {
        WaterBillingEntities _cnn;

        public clsUserMaster()
        {
            _cnn = new WaterBillingEntities();
        }

        public bool? saveUserMaster(int pID, int pRefUserTypeID, string pFirstName, string pLastName, string pPhotoPath, string pGender,
                            DateTime pDOB, string pMobileNo, string pContactNo1, string pContactNo2, string pEmailID, bool pIsActive,
                            int pRefRoleID, string pUserName, bool pAllowMobileRegistration,
                            int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal, List<int> pRefCollectionCenterId, bool CollCentIsActive)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_UserMaster_Save(pID, pRefUserTypeID, pFirstName, pLastName, pPhotoPath,
                                                    pGender, pDOB, pMobileNo, pContactNo1, pContactNo2, pEmailID,
                                                    pIsActive, pRefRoleID, pUserName, pAllowMobileRegistration,
                                                      pInsUser, pInsTerminal, pUpdUser, pUpdTerminal).FirstOrDefault().Result;
                if (pID == 0)
                {
                    foreach (var _RefCollectionCenterId in pRefCollectionCenterId)
                    {
                        SetUserCollectionCenterRights((int)_obj, _RefCollectionCenterId, CollCentIsActive, pInsUser, pInsTerminal);
                    }
                }
                else
                {
                    foreach (var _RefCollectionCenterId in pRefCollectionCenterId)
                    {
                        SetUserCollectionCenterRights((int)_obj, _RefCollectionCenterId, CollCentIsActive, pUpdUser, pUpdTerminal);
                    }
                }

                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteUserMaster(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_UserMaster_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? SetUserRole(int ID, int RefRoleId, string UserName, int UpdUser, string UpdTerminal)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_UserMaster_SetUserRole(ID, RefRoleId, UserName, UpdUser, UpdTerminal);

            }
            catch (Exception)
            {

                return false;
            }
            return retVal;
        }

        public List<sp_UserMaster_Select_Result> getUserMaster(int pID)
        {
            List<sp_UserMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_UserMaster_Select(pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }


        public List<sp_UserMaster_SelectWhere_Result> getUserMasterWhere(string pInValue)
        {
            List<sp_UserMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_UserMaster_SelectWhere(pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? updateStatus_IsActive(int pID, bool pFlag)
        {
            bool? retVal = false;
            try
            {
                var _obj = (from x in _cnn.UserMaster
                            where x.ID == pID
                            select x).SingleOrDefault();

                _obj.IsActive = pFlag;
                clsCommon.Commit(_cnn);

                retVal = true;

            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;

        }

        public bool isUserExists(int pID, string pValueName)
        {
            bool retVal = false;

            try
            {
                int _resp;
                if (pID == 0)
                {
                    _resp = _cnn.sp_UserMaster_SelectWhere(" and UserName='" + pValueName.Trim() + "'").ToList().Count;
                }
                else
                {
                    _resp = _cnn.sp_UserMaster_SelectWhere(" and ID !=" + pID.ToString() + " and UserName='" + pValueName.Trim() + "'").ToList().Count;
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


        public bool? updateStatus_MobileAppAccess(int pID, bool pFlag)
        {
            bool? retVal = false;
            try
            {
                var _obj = (from x in _cnn.UserMaster
                            where x.ID == pID
                            select x).SingleOrDefault();

                _obj.AllowMobileRegistration = pFlag;
                clsCommon.Commit(_cnn);

                retVal = true;

            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;

        }

        //public bool? DeleteUserCollectionCenterRights(int pRefUserId)
        //{
        //    bool? _retval = false;
        //    try
        //    {
        //        _cnn.sp_UserCollCentRights_Delete(pRefUserId);
        //        _retval = true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //    return _retval;
        //}

        public bool? SetUserCollectionCenterRights(int pRefUserId, int pRefCollectionCenterId, bool IsActive, int pInsUser, string pInsTerminal)
        {
            bool? _retval = false;
            try
            {
                _cnn.sp_UserCollCentRights_SetCollCentRights(pRefUserId, pRefCollectionCenterId, IsActive, pInsUser, pInsTerminal);
                _retval = true;
            }
            catch (Exception)
            {

                return false;
            }
            return _retval;
        }

        public Tuple<List<int>,bool> GetListofCollectionCenter(int pUserId)
        {
            List<int> _Retval = null;
            bool _IsActive = false;
            try
            {
                _Retval = _cnn.UserCollCentRights.Where(x => x.RefUserId == pUserId).Select(x => x.RefCollectionCenterId).ToList();
                _IsActive = _cnn.UserCollCentRights.Where(x => x.RefUserId == pUserId).Select(x => x.IsActive).FirstOrDefault();
                return new Tuple<List<int>, bool>(_Retval, _IsActive);
            }
            catch (Exception)
            {
                return new Tuple<List<int>, bool>(_Retval, _IsActive);
            }
        }


    }
}
