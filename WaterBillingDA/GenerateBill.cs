using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;
using System.Data;

namespace WaterBillingDA
{

    public class GenerateBill
    {
        public DataSet _Ds = new DataSet();
        public static int _Id;
        public static string _Message = string.Empty;
        public static bool X_ToCalDPC = true;
        public static DateTime x_DueDate, X_ReadingDate;
        public static string X_PrevStatus, X_CurrStatus, X_CustNo;
        public static int X_PrevStatusID, X_CurrStatusID;
        public static DateTime X_IssueDate;
        public static int X_Reading;
        public static string[] a4Month = new string[12];
        public static bool X_CalAsPerDays = true;
        WaterBillingEntities _cnn;
        clsReceiptDetail _ObjReceiptDetail = new clsReceiptDetail();
        clsMasterValue _ObjMasterValue = new clsMasterValue();
        clsConsumerMaster _ObjConsumermaster = new clsConsumerMaster();
        GenerateNormalBillNew _ObjNormalBill = new GenerateNormalBillNew();
        GenerateMeterFixingBillsNew _ObjMEterFixingBill = new GenerateMeterFixingBillsNew();

        public GenerateBill()
        {
            _cnn = new WaterBillingEntities();
        }

        public DataSet  GenBillNew(string xCustNo, DateTime xIDt, int xReading, int xStatusID, int xPrevStatID, DateTime xReadingDate)
        {
            try
            {
                #region "Set Months In String Formate"
                //a4Month[0] = "Jan";
                //a4Month[1] = "Feb";
                //a4Month[2] = "Mar";
                //a4Month[3] = "Apr";
                //a4Month[4] = "May";
                //a4Month[5] = "Jun";
                //a4Month[6] = "Jul";
                //a4Month[7] = "Aug";
                //a4Month[8] = "Sep";
                //a4Month[9] = "Oct";
                //a4Month[10] = "Nov";
                //a4Month[11] = "Dec";
                #endregion


                xPrevStatID = Convert.ToInt32(_ObjConsumermaster.getConsumerMasterWhere(" and ConsumerNo = '" + xCustNo + "'").FirstOrDefault().PrevRefStatusId);

                X_CustNo = xCustNo;
                X_IssueDate = xIDt.Date;
                X_Reading = xReading;
                X_PrevStatusID = xPrevStatID;
                X_CurrStatusID = xStatusID;
                X_ReadingDate = xReadingDate;

                //x_DueDate = X_IssueDate.AddDays(15);// this is temp
                int mPCount = 0;

                mPCount = _ObjReceiptDetail.GetReceiptDetailSelectWhere(" and RefConsumerId = (select Id from ConsumerMaster where ConsumerNo = '" + X_CustNo + "')").Count();

                //-- set true / false base on mPCount 
                //if (mPCount > 0)
                //    X_ToCalDPC = true;
                //else
                //    X_ToCalDPC = false;

                //-- retrive short name of meterstatus from mastervalues base on PrevStatusId
                X_PrevStatus = _ObjMasterValue.getMasterValue(6, xPrevStatID).SingleOrDefault().ShortName;

                //-- retrive short name of meterstatus from mastervalues base on CurrentStatusId
                X_CurrStatus = _ObjMasterValue.getMasterValue(6, xStatusID).SingleOrDefault().ShortName;

                //--if condition for check PrevStatus = 'NWK' - 'Not-Working' and CurrentStatus = 'MOK' - 'Working'
                if (X_PrevStatus == "NWK" && X_CurrStatus == "MOK")
                {
                    if (!_cnn.sp_BG_CheckIfMeterFixedNew(X_CustNo).FirstOrDefault().Value)
                    {
                        _Message = "Meter Not Fixed. Cannot Generate";
                        throw new Exception(_Message);
                        //--Throw Message in Cache
                        //--***********Pending*************
                    }
                    
                }

                //--if condition for check PrevStatus = 'DIS' - 'Disconnected'
                if (X_PrevStatus == "DIS")
                {
                    //--statement for check Current Status is 'Wroking'
                    if (X_CurrStatus == "MOK")
                    {
                        //--Store function return value in result
                        //-- (here to retrieve value for Re-Connected but MeterFixedNew function's result gethering value 
                        //-- in same pattern then we used this)

                        if (!_cnn.sp_BG_CheckIfReConnected(X_CustNo).FirstOrDefault().Value)
                        {
                            _Message = "";
                            throw new Exception(_Message);
                            //--Throw Message in Cache
                            //--***********Pending*************
                        }
                        //--Call Procedure for Generate 'Normal' BillNew

                        return _ObjNormalBill.GenerateNormalBill();
                    }
                    else if (X_CurrStatus == "DIS")
                    {
                        X_CurrStatus = "DIS";
                    }
                }

                //--if condition for check Current Status = 'MOK' - 'Working'
                if (X_CurrStatus == "MOK")
                {
                    //--Store function return value in result

                    if (_cnn.sp_BG_CheckIfMeterFixedNew(X_CustNo).FirstOrDefault().Value)
                    {
                        //--Call Procedure for Generate 'MeterFixing' BillsNew
                        return _ObjMEterFixingBill.GenerateMeterFixingBills();
                    }
                    else
                    {
                        //-- check Prev Status is 'Working' or 'House Locked'
                        if (X_PrevStatus == "MOK" || X_PrevStatus == "HLK")
                        {
                            //--Call Procedure for Generate 'Normal' BillNew
                           return _ObjNormalBill.GenerateNormalBill();
                        }
                    }
                }
                else
                {
                    //-- check Current Status is 'Disconnected'
                    if (X_CurrStatus == "DIS")
                    {
                        //--Store function return value in result
                        //-- (here set CheckIfArrearsNew() Function to check condition and it's not create)
                        //if(@Result = 1)
                        //{
                        //    //--Call Procedure for Generate 'Arrears' BillsNew
                        //}
                        //else
                        //{
                        _Message = "";
                        throw new Exception(_Message);
                            //return;
                        //    --Throw Message in Cache
                        //    --***********Pending*************
                        //    }
                    }
                    else
                    {
                        //--Call Procedure for Generate 'Normal' BillNew
                       return  _ObjNormalBill .GenerateNormalBill();
                        
                    }
                }
                return _Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static DataTable BillPrint()
        //{
        //    DataTable _Dt = new DataTable();
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
        //    return _Dt;
        //}

        
    }

     
}
