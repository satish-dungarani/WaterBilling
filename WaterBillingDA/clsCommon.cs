using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity;

namespace WaterBillingDA
{
    public class clsCommon
    {
        WaterBillingEntities _cnn;

        public clsCommon()
        {
            _cnn = new WaterBillingEntities();
        }

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

        //public List<sp_MasterList_Select_Result> getMasterList()
        //{
        //    List<sp_MasterList_Select_Result> retVal = null;
        //    try
        //    {
        //        retVal = _cnn.sp_MasterList_Select().ToList();
        //    }
        //    catch (Exception)
        //    {
        //        retVal = null;
        //    }

        //    return retVal;
        //}

        public static bool Commit(WaterBillingEntities _cnn)
        {
            bool retVal = false;

            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                _cnn.SaveChanges();
                retVal = true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                retVal = false;
                throw;
            }
            return retVal;
        }

        public bool get_MasterList(int? pId)
        {
            bool _retVal = true;
            try
            {
                _retVal = _cnn.sp_MasterList_Select(pId).FirstOrDefault().IsSystemGenerated;
                return _retVal;
            }
            catch (Exception)
            {
                return _retVal;
            }
        }

        public List<sp_RetrieveMenuRightsWise_Select_Result> get_MenuList(string pUserName)
        {
            List<sp_RetrieveMenuRightsWise_Select_Result> _retVal = null;
            try
            {
                _retVal = _cnn.sp_RetrieveMenuRightsWise_Select(pUserName).ToList();
                return _retVal;
            }
            catch (Exception)
            {

                return _retVal;
            }
        }

        public static SqlCommand ReportData()
        {
            try
            {
                SqlConnection _Cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                //string _QueryStr =  ;
                SqlCommand _Cmd = new SqlCommand();
                _Cmd.Connection = _Cnn;
                _Cmd.CommandType = CommandType.StoredProcedure;
                _Cmd.CommandText = "sp_ConsumerList_Select";

                //DataSet _Ds = new DataSet();
                //SqlDataAdapter _Da = new SqlDataAdapter(_Cmd);
                //_Da.Fill(_Ds,"dRableList");
                return _Cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
