using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBillingDB;
using WaterBilling.Models;
using System.IO;

namespace WaterBilling
{
    public static class clsCommonUI
    {

        public static int _User = 0;
        public static string _Terminal = string.Empty;
        public static int? _UserRoleId = 0;
        public static bool _IsReader = false;

        public enum MasterType
        {
            Camp = 1,
            Ward = 2,
            Zone = 3,
            MeterSize = 4,
            MeterType = 5,
            MeterStatus = 6,
            SupplyType = 7,
            SupplyCategory = 8,
            DMA = 9,
            Bank = 10,
            CollectionCenter = 11,
            PaymentType = 12,
            ConstructionType = 13,
            ReasonType = 14,
            UserType = 15,
            UserRole = 16,
            CityId = 17,
            StateId = 18,
            CountryId = 19,
            //Reader Id in User Master fix with '69'
            ReaderId = 69

        }

        public static SelectList fillEffectiveDate_BoardRate()
        {
            clsBoardRentMaster _objBoardRent = new clsBoardRentMaster();

            List<SelectListItem> Selectlist = new List<SelectListItem>();

            foreach (DateTime objDt in _objBoardRent.get_Distinct_EffectiveDates())
            {
                Selectlist.Add(new SelectListItem { Selected = false, Text = objDt.ToString("dd/MM/yyyy"), Value = objDt.ToString("dd/MM/yyyy") });

            }
            return new SelectList(Selectlist, "Value", "Text");
        }

        public static SelectList fillEffectiveDate_ConsumerRate()
        {
            clsConsumerRateMaster _objConsumerRent = new clsConsumerRateMaster();

            List<SelectListItem> Selectlist = new List<SelectListItem>();

            foreach (DateTime objDt in _objConsumerRent.get_Distinct_EffectiveDates())
            {
                Selectlist.Add(new SelectListItem { Selected = false, Text = objDt.ToString("dd/MM/yyyy"), Value = objDt.ToString("dd/MM/yyyy") });

            }
            return new SelectList(Selectlist, "Value", "Text");
        }

        public static SelectList fillEffectiveDate_MeterMinCharge()
        {
            clsMeterMinChargeMaster _objMeterMinCharge = new clsMeterMinChargeMaster();

            List<SelectListItem> Selectlist = new List<SelectListItem>();

            foreach (DateTime objDt in _objMeterMinCharge.get_Distinct_EffectiveDates())
            {
                Selectlist.Add(new SelectListItem { Selected = false, Text = objDt.ToString("dd/MM/yyyy"), Value = objDt.ToString("dd/MM/yyyy") });
            }
            return new SelectList(Selectlist, "Value", "Text");
        }

        //public static SelectList fillMenuName_MenuMaster()
        //{
        //    clsCommon _objMenuRights = new clsCommon();

        //    List<SelectListItem> Selectlist = new List<SelectListItem>();

        //    foreach (MenuMaster  _obj in _objMenuRights.get_MenuList())
        //    {
        //        Selectlist.Add(new SelectListItem { Selected = false, Text = _obj.MenuName, Value = _obj.ID.ToString() });

        //    }
        //    return new SelectList(Selectlist, "Value", "Text");
        //}

        public static SelectList fillMasterValue(int pRefMasterId)
        {
            #region Valid values for pID are as follow.
            //===============================================
            //  Id	MasterName	                OrdNo
            //===============================================
            //  1	Camp Master	                1.00
            //  2	Ward Master	                2.00
            //  3	Zone Master	                3.00
            //  4	Meter Size Master	        4.00
            //  5	Meter Type Master	        5.00
            //  6	Meter Status Master	        6.00
            //  7	Supply Type Master	        7.00
            //  8	Supply Category Master	    8.00
            //  9	DMA Master	                9.00
            //  10	Bank Master	                10.00
            //  11	Collection Center Master	11.00
            //  12	Payment Type Master	        12.00
            //  13	Construction Type Master	13.00
            //  14	ReasonType	                14.00
            //  15  UserType                    15.00
            //  16  UserRole                    16.00

            #endregion

            clsMasterValue _objMasterValue = new clsMasterValue();

            List<SelectListItem> Selectlist = new List<SelectListItem>();

            foreach (var obj in _objMasterValue.getMasterValue(pRefMasterId, -1))
            {
                Selectlist.Add(new SelectListItem { Selected = false, Text = obj.ValueName, Value = obj.ID.ToString() });

            }
            return new SelectList(Selectlist, "Value", "Text");
        }

        public static bool? checkAccessAlloworNot(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam)
        {
            bool _retval = false;
            try
            {
                string _RowUrlString = HttpContext.Current.Request.RawUrl.ToString();
                string _ControllerName = _RowUrlString.Substring(1, _RowUrlString.IndexOf('/', 1) - 1);
                string _ActionName = _RowUrlString.Substring(_RowUrlString.LastIndexOf('/') + 1, _RowUrlString.Length - _RowUrlString.LastIndexOf('/') - 1);
                if (_ControllerName.ToUpper() == "HOME")
                {
                    _retval = true;
                    return _retval;
                }
                foreach (sp_RetrieveMenuRightsWise_Select_Result _obj in _objParam)
                {
                    if (_obj.ControllerName != null)
                    {
                        if (_ControllerName.ToUpper() == _obj.ControllerName.ToUpper())
                        {
                            _retval = true;
                            break;
                        }
                    }
                }
                if (_retval == true)
                {
                    if (_ActionName.ToUpper() == "MANAGE")
                    {
                        foreach (sp_RetrieveMenuRightsWise_Select_Result _obj in _objParam)
                        {
                            if ((_obj.CanInsert == true || _obj.CanUpdate == true) && _ControllerName.ToUpper() == _obj.ControllerName.ToUpper())
                            {
                                return _retval;
                            }
                        }
                        _retval = false;
                    }
                }
            }
            catch (Exception)
            {

                return _retval;
            }
            return _retval;
        }

        public static bool? checkAccessIndividual(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam, string pOperation, string pControllerName)
        {
            bool _retval = false;

            try
            {
                if (pOperation.ToUpper() == "INSERT")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanInsert == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "UPDATE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanUpdate == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "DELETE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanDelete == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return false;

            }
            return _retval;
        }

        public static bool? checkAccessIndividualInMapPath(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam, string pOperation, string pMenuPath)
        {
            bool _retval = false;

            try
            {
                if (pOperation.ToUpper() == "INSERT")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanInsert == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "UPDATE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanUpdate == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "DELETE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanDelete == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return false;

            }
            return _retval;
        }

        public static bool? checkAccessIndividual(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam, string pOperation, string pControllerName, DateTime pEffectDate, int pSupplyType = 0)
        {
            bool _retval = false;
            int _Count = 0;
            clsConsumerRateMaster _ObjConsumeRate = new clsConsumerRateMaster();
            clsMeterMinChargeMaster _ObjMeterMinCharge = new clsMeterMinChargeMaster();
            clsBoardRentMaster _ObjBoardRent = new clsBoardRentMaster();
            clsChqBounceChargiesMaster _ObjChqBounceChargies = new clsChqBounceChargiesMaster();
            try
            {
                if (pControllerName.ToUpper() == "CONSUMERRATE")
                {
                    _Count = _ObjConsumeRate.SelectConsumerRateData(pEffectDate, pSupplyType).Count();
                }
                else if (pControllerName.ToUpper() == "METERMINCHARGE")
                {
                    _Count = _ObjMeterMinCharge.SelectMeterMinChargeData(pEffectDate, pSupplyType).Count();
                }
                else if (pControllerName.ToUpper() == "BOARDRENT")
                {
                    _Count = _ObjBoardRent.getBoardRentMaster(pEffectDate).Count();
                }
                else if (pControllerName.ToUpper() == "CHQBOUNCECHARGIES")
                {
                    _Count = _ObjChqBounceChargies.SelectChqBounceChargiesMaster(pEffectDate, pSupplyType).Count();
                }

                if (_Count == 0)
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanInsert == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (_Count > 0)
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanUpdate == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return false;

            }
            return _retval;
        }

        public static SelectList fillConsumerNo_ConsumerMaster(int? pReaderId)
        {
            if (pReaderId != null || pReaderId != 0)
            {
                clsConsumerMaster _ObjConsumerMaster = new clsConsumerMaster();

                List<SelectListItem> _SelectList = new List<SelectListItem>();
                try
                {

                    foreach (var _Obj in _ObjConsumerMaster.getConsumerMasterWhere(" and RefReaderId = " + pReaderId))
                    {
                        _SelectList.Add(new SelectListItem { Selected = false, Value = _Obj.Id.ToString(), Text = _Obj.ConsumerNo.ToString() });
                    }

                    return new SelectList(_SelectList, "Value", "Text");

                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public static SelectList fillReaderName_UserMaster()
        {
            clsUserMaster _ObjUserMaster = new clsUserMaster();
            clsMasterValue _ObjMasterValue = new clsMasterValue();

            List<SelectListItem> _SelectList = new List<SelectListItem>();
            try
            {
                int _ReaderRoleId = _ObjMasterValue.getMasterValueWhere((int)clsCommonUI.MasterType.UserRole, " and ValueName = 'Reader'").FirstOrDefault().ID;

                int _AdminRoleId = _ObjMasterValue.getMasterValueWhere((int)clsCommonUI.MasterType.UserRole, " and ValueName = 'Admin'").FirstOrDefault().ID;

                if (_ReaderRoleId == clsCommonUI._UserRoleId)
                {
                    foreach (var _Obj in _ObjUserMaster.getUserMasterWhere(" and Id = " + clsCommonUI._User))
                    {
                        _SelectList.Add(new SelectListItem { Selected = false, Value = _Obj.ID.ToString(), Text = _Obj.FullName.ToString() });
                    }
                    _IsReader = true;
                }
                else if (_AdminRoleId == clsCommonUI._UserRoleId)
                {
                    foreach (var _Obj in _ObjUserMaster.getUserMasterWhere(" and RefRoleId = " + _ReaderRoleId))
                    {
                        _SelectList.Add(new SelectListItem { Selected = false, Value = _Obj.ID.ToString(), Text = _Obj.FullName.ToString() });
                    }
                    _IsReader = false;
                }

                return new SelectList(_SelectList, "Value", "Text");
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static SelectList fillReaderName_UserMaster(int? pCampId)
        {
            clsUserMaster _ObjUserMaster = new clsUserMaster();
            clsConsumerMaster _ObjConsumerMaster= new clsConsumerMaster();

            List<SelectListItem> _SelectList = new List<SelectListItem>();
            try
            {
                var _ObjReaderId = _ObjConsumerMaster.getConsumerMasterWhere("").Where(x => x.RefCampId == pCampId).Select(x => x.RefReaderId).GroupBy(x => x.Value).ToList();
                foreach (var _ReaderId in _ObjReaderId)
                {
                    foreach (var _Obj in _ObjUserMaster.getUserMaster(Convert.ToInt32(_ReaderId.Key)))
                    {
                        _SelectList.Add(new SelectListItem { Selected = false, Value = _Obj.ID.ToString(), Text = _Obj.FullName.ToString() });
                    }    
                }
                _IsReader = false;

                return new SelectList(_SelectList, "Value", "Text");
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static SelectList fillReasonName_ReasonMaster(string _MasterValueShortName)
        {
            List<SelectListItem> _SelectItem = new List<SelectListItem>();
            try
            {
                clsMasterValue _objMasterValue = new clsMasterValue();
                clsReasonMaster _objReasonMaster = new clsReasonMaster();
                string _Consdition = " and Upper(ShortName) = '" + _MasterValueShortName.ToUpper() + "'";
                int _RefMasterId = _objMasterValue.getMasterValueWhere((int)clsCommonUI.MasterType.ReasonType, _Consdition).FirstOrDefault().ID;
                foreach (var _Obj in _objReasonMaster.getReasonMasterWhere(" and RefReasonTypeId = " + _RefMasterId))
                {
                    _SelectItem.Add(new SelectListItem { Selected = false, Value = _Obj.ID.ToString(), Text = _Obj.ReasonName });
                }
                return new SelectList(_SelectItem, "Value", "Text");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static SelectList fillEffectDate_ChqBounceChargiesMaster()
        {
            try
            {
                clsChqBounceChargiesMaster _ObjChqBounceChargies = new clsChqBounceChargiesMaster();

                List<SelectListItem> _SelectList = new List<SelectListItem>();

                foreach (DateTime _Obj in _ObjChqBounceChargies.get_Distinct_EffectiveDates())
                {
                    _SelectList.Add(new SelectListItem { Selected = false, Value = _Obj.ToString("dd/MM/yyyy"), Text = _Obj.ToString("dd/MM/yyyy") });
                }
                return new SelectList(_SelectList, "Value", "Text");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void WriteToFile(string pPath, string pText)
        {
            try
            {
                File.AppendAllText(pPath, pText);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}