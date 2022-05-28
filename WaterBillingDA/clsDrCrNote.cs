using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;
using System.Configuration;

namespace WaterBillingDA
{
    public class clsDrCrNote
    {
        WaterBillingEntities _cnn;
        public string _ConnectionString { get; set; }
        public clsDrCrNote()
        {
            _cnn = new WaterBillingEntities();

        }

        public bool? SaveDrCrNote(int pId, string pNoteType, int pSerialNo, int pRefConsumerId, DateTime? pNoteDate, decimal pAmount,
                    string pNarration, int? pRefReasonId, string pOrderNo, DateTime? pOrderDate,string pFileName, int pInsUser, string pInsTerminal, int pUpdUser, string pUpdTerminal)
        {
            bool? _retval = false;
            try
            {
                var _Obj = _cnn.sp_DrCrNote_Save(pId, pNoteType, pSerialNo, pRefConsumerId, pNoteDate, pAmount,
                    pNarration, pRefReasonId, pOrderNo, pOrderDate, pFileName, pInsUser, pInsTerminal, pUpdUser, pUpdTerminal);
                _retval = true;
            }
            catch (Exception)
            {

                return _retval;
            }
            return _retval;
        }

        public bool? DeleteDrCrNote(int pId)
        {
            bool? _retval = false;
            try
            {
                var _obj = _cnn.sp_DrCrNote_Delete(pId);
                _retval = true;
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_DrCrNote_Select_Result> GetDrCrNoteDetail(int pId)
        {
            List<sp_DrCrNote_Select_Result> _retval = new List<sp_DrCrNote_Select_Result>();
            try
            {
                _retval = _cnn.sp_DrCrNote_Select(pId).ToList();
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_DrCrNote_SelectWhere_Result> GetDrCrNoteSelectWhere(string pConsditions)
        {
            List<sp_DrCrNote_SelectWhere_Result> _retval = new List<sp_DrCrNote_SelectWhere_Result>();
            try
            {
                _retval = _cnn.sp_DrCrNote_SelectWhere(pConsditions).ToList();
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public string GetUniqueId()
        {
            try
            {
                SqlCommand _Cmd = new SqlCommand();
                SqlConnection _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                _Cmd.Connection = _Con;
                _Cmd.CommandType = CommandType.Text;
                _Cmd.CommandText = "Select NEWID()";
                _Con.Open();
                string _Id = Convert.ToString(_Cmd.ExecuteScalar());
                _Con.Close();
                return _Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
