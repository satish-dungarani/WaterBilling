using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBillingDB;

namespace WaterBillingDA
{
    public class GenerateMeterFixingBillsNew
    {
        WaterBillingEntities _cnn;
        clsMasterValue _ObjMastervalue = new clsMasterValue();
        clsReceiptDetail _ObjReceiptDetail = new clsReceiptDetail();
        clsBoardRentMaster _ObjBoardRent = new clsBoardRentMaster();
        clsConsumerRateMaster _ObjConsumerRate = new clsConsumerRateMaster();
        clsMeterMinChargeMaster _ObjMeterMinCharge = new clsMeterMinChargeMaster();
        clsConsumeDetail _ObjConsumeDetail = new clsConsumeDetail();
        clsConsumerMaster _ObjConsumeMaster = new clsConsumerMaster();
        public DateTime? mEffDate1, mEffDate2;
        public double? X_DPC = 0, X_Dpc2 = 0;
        public int mPrevHLQty = 0;

        public GenerateMeterFixingBillsNew()
        {
            _cnn = new WaterBillingEntities();
        }

        public DataSet GenerateMeterFixingBills()
        {
            try
            {
                //get last effective date from effdate table

                if (_cnn.EffDate.Where(x => x.EffectiveDate <= GenerateBill.X_IssueDate).Count() > 0)
                {
                    mEffDate1 = _cnn.EffDate.Where(x => x.EffectiveDate <= GenerateBill.X_IssueDate).OrderByDescending(x => x.EffectiveDate).FirstOrDefault().EffectiveDate;
                    mEffDate2 = mEffDate1;

                }
                else
                    throw new Exception("No any value available in Effect Date table.");

                //get dpc percantage from other chargies table
                X_DPC = _cnn.OtherChargies.Where(x => x.EffectDate == mEffDate1).FirstOrDefault().DPCPercantage;

                #region "Decalre all Variable"
                int Id = GenerateBill._Id;
                string mCustNo;
                string mStatus;
                string mMType;
                string mStCode;
                string mSupCode;
                string mSizeCode;
                string mMTypeCode;
                string mBillType = null;
                string mBookNo;
                int? mNoOfFlats;
                int? mMTypeId;
                int? mSupTypeId;
                int? mSupCategoryId;
                int? mReaderId;
                int? mMSizeId;
                int mOthChg;
                int mAvg;
                int mAdj;
                int mBFABasic = 0;
                int mOpBal;
                int mOpDpc;
                int mPrevRead;
                int mRebate;
                int mReading;
                int mQty;
                int mQty2;
                int mQtyCons;
                int mRent1 = 0;
                int mRent2 = 0;
                int mRent = 0;
                int mCurrMthDpc = 0;
                decimal mDedAmt;
                decimal mDedMth;
                decimal mRate;
                decimal mRate2;
                decimal mDebitNoteAmount;
                decimal mDebitNoteDPCAmount;
                decimal mCreaditNoteAmount;
                decimal mCreaditNoteDPCAmount;
                DateTime? mLReceiptDate;
                DateTime? mWrkSince;
                DateTime? mLIDate;
                DateTime? mPReadDate;
                DateTime? mNReadDate;
                DateTime? mIDate;
                DateTime? mFromCalDt;
                DateTime? mUptoCalDt;
                DateTime? mFromCalDt1;
                DateTime? mUptoCalDt1;
                bool mIsRcptReceipved = false;
                #endregion

                var _Obj = _cnn.sp_BG_SelectConsumerMeterDetail(GenerateBill.X_CustNo).FirstOrDefault();

                #region "Assign value to Variable"
                mCustNo = _Obj.ConsumerNo;
                mStatus = GenerateBill.X_CurrStatus;
                mMType = _Obj.MeterType;// if not in used then delete
                mStCode = _Obj.MeterStatusCode;
                mSupCode = _Obj.SupplyTypeCode;
                mSizeCode = _Obj.SizeCode;
                mMTypeCode = _Obj.MeterTypeCode;
                mBookNo = _Obj.BookNo;
                mNoOfFlats = _Obj.NoOfFlats;
                mMTypeId = _Obj.RefMeterTypeId;
                mSupTypeId = _Obj.RefSupplyTypeId;
                mSupCategoryId = _Obj.RefSupplyCategoryId;
                mReaderId = _Obj.RefReaderId;
                mMSizeId = _Obj.RefMeterSizeId;
                mOthChg = 0;//remove later
                mDebitNoteAmount = Convert.ToDecimal(_Obj.DebitNoteAmount);
                mDebitNoteDPCAmount = Convert.ToDecimal(_Obj.DebitNoteDPCAmount);
                mCreaditNoteAmount = Convert.ToDecimal(_Obj.CreaditNoteAmount);
                mCreaditNoteDPCAmount = Convert.ToDecimal(_Obj.CreaditNoteDPCAmount);
                mAvg = Convert.ToInt32(_Obj.AverageConsumption);
                mAdj = Convert.ToInt32(_Obj.AdjUnit);
                //mBFABasic ,
                mOpBal = Convert.ToInt32(_Obj.Balance);
                mOpDpc = Convert.ToInt32(_Obj.OpDpc);
                mPrevRead = Convert.ToInt32(_Obj.InitalReading);
                mRebate = Convert.ToInt32(_Obj.Rebate);
                mReading = GenerateBill.X_Reading;
                mQtyCons = GenerateBill.X_Reading > mPrevRead ? GenerateBill.X_Reading - mPrevRead : 0;
                //mQty ,
                mQty2 = Convert.ToInt32(_Obj.NoOfConnections);
                mDedAmt = 0;// remove later
                //mDedMth,
                //mRate ,
                //mRate2,
                mWrkSince = _Obj.WrkSince;
                mLIDate = _Obj.PrevIssueDate;
                mLReceiptDate = _Obj.LastReceiptDate;
                mPReadDate = _Obj.PReadDate;
                mNReadDate = GenerateBill.X_IssueDate;
                mIDate = GenerateBill.X_IssueDate;
                mFromCalDt1 = _Obj.PrevIssueDate;
                mUptoCalDt1 = _Obj.WrkSince;
                mFromCalDt = _Obj.WrkSince;
                mUptoCalDt = GenerateBill.X_IssueDate;
                int mMaxReadingNo = Convert.ToInt32(_Obj.MaxReadingNo);
                DateTime mPrevIDate = Convert.ToDateTime(_Obj.PrevIssueDate);
                #endregion

                //if (GenerateBill.X_CurrStatus == "MOK")
                //{
                //    mFromCalDt = _Obj.PReadDate;
                //    mUptoCalDt = GenerateBill.X_ReadingDate;
                //}
                //else
                //{
                //    mFromCalDt = _Obj.PrevIssueDate;
                //    mUptoCalDt = GenerateBill.X_IssueDate;
                //}

                //if (GenerateBill.X_PrevStatus == "DIS" && GenerateBill.X_CurrStatus == "MOK")
                //{
                //    mFromCalDt = _cnn.ConsumerMaster.Where(x => x.ConsumerNo == GenerateBill.X_CustNo).SingleOrDefault().ReConnectedDate;
                //}

                if (mPrevRead > GenerateBill.X_Reading)
                {
                    mQtyCons = (mMaxReadingNo - mPrevRead) + GenerateBill.X_Reading;
                }

                string _WhereCondition = " and RefConsumerId = (Select Id from ConsumerMaster Where ConsumerNo = '" + GenerateBill.X_CustNo + "') and Cast(ReceiptDate as Date) >= Convert(Date,'" + mFromCalDt + "' ,103)  and ChqBounceDate Is Null";

                if (_ObjReceiptDetail.GetReceiptDetailSelectWhere(_WhereCondition).Count > 0)
                {
                    mIsRcptReceipved = true;
                }

                if (GenerateBill.X_ToCalDPC)// tally with normal bill
                {
                    if (!mIsRcptReceipved)
                    {
                        mCurrMthDpc = Convert.ToInt32(Math.Round(Convert.ToDouble(((mOpBal * (X_DPC / 100)) / 365) * 30), 0));
                    }
                }

                //Select top 1 @mEffDate1 = EffectiveDate From EffDate WHERE Cast(EffectiveDate as Date) <= Cast(@mUptoCalDt as Date) ORDER BY EffectiveDate Desc
                mEffDate1 = _cnn.EffDate.Where(x => x.EffectiveDate <= mUptoCalDt).OrderByDescending(x => x.EffectiveDate).FirstOrDefault().EffectiveDate;

                int i2 = 0;
                int iRCount = 0;
                List<DateTime> a4xEDt = new List<DateTime>();
                List<DateTime> a4xFDt = new List<DateTime>();
                List<int> a4Months = new List<int>();
                List<int> a4Days = new List<int>();
                int mCtr = 0;

                iRCount = _cnn.EffDate.Where(x => x.EffectiveDate <= mEffDate1).Count();

                //foreach (DateTime _DT in _cnn.EffDate.Where(x => x.EffectiveDate <= mEffDate1).Select(x => x.EffectiveDate).OrderByDescending(x => x.Value).FirstOrDefault())
                //{
                DateTime _DT = Convert.ToDateTime(_cnn.EffDate.Where(x => x.EffectiveDate <= mEffDate1).Select(x => x.EffectiveDate).OrderByDescending(x => x.Value).FirstOrDefault());
                a4xEDt.Add(_DT);
                a4xFDt.Add(Convert.ToDateTime(mFromCalDt));
                //}

                foreach (DateTime _DT1 in _cnn.EffDate.Where(x => x.EffectiveDate >= mFromCalDt && x.EffectiveDate <= mUptoCalDt).Select(x => x.EffectiveDate).OrderBy(x => x.Value).ToList())
                {
                    a4xEDt.Add(_DT1);
                    a4xFDt.Add(_DT1);
                }

                if (a4xEDt.Count - 1 > 0)
                {
                    for (int i = 0; i < a4xEDt.Count; i++)
                    {
                        if (i != a4xEDt.Count - 1)
                        {
                            a4Months.Add((a4xFDt[i + 1].Year - a4xFDt[i].Year) * 12 + (a4xFDt[i + 1].Month - a4xFDt[i].Month));
                        }
                        else
                        {
                            a4Months.Add((mUptoCalDt.Value.Year - a4xFDt[i].Year) * 12 + (mUptoCalDt.Value.Month - a4xFDt[i].Month));
                        }
                    }
                }

                mEffDate2 = _cnn.EffDate.Where(x => x.EffectiveDate <= mFromCalDt).OrderByDescending(x => x.EffectiveDate).FirstOrDefault().EffectiveDate;
                //SELECT top 1 @X_Dpc2 = DPCPercantage From OtherChargies WHERE Cast(EffectDate as Date) = Cast(@mEffDate2 as Date)
                X_Dpc2 = _cnn.OtherChargies.Where(x => x.EffectDate == mEffDate2).FirstOrDefault().DPCPercantage;

                int mMth1 = 0;
                int mMth2 = 0;
                int mDays1 = 0;
                int mDays2 = 0;

                int mDiff1 = ((mUptoCalDt1.Value.Year - mFromCalDt1.Value.Year) * 12 + (mUptoCalDt1.Value.Month - mFromCalDt1.Value.Month));
                int mDiff = ((mUptoCalDt.Value.Year - mFromCalDt.Value.Year) * 12 + (mUptoCalDt.Value.Month - mFromCalDt.Value.Month));
                int mCalMths = mDiff == 0 ? 1 : mDiff;

                mDays1 = (mUptoCalDt.Value - mFromCalDt.Value).Days;

                int mDaysBeforeFix = (mUptoCalDt1.Value - mFromCalDt1.Value).Days;
                int mDaysAfterFix = (mUptoCalDt.Value - mFromCalDt.Value).Days;

                if (a4xEDt.Count - 1 > 0)
                {
                    for (int i = 0; i < a4xEDt.Count; i++)
                    {
                        if (i != a4xEDt.Count - 1)
                        {
                            a4Months.Add((a4xFDt[i].Year - a4xFDt[i + 1].Year) * 12 + (a4xFDt[i].Month - a4xFDt[i + 1].Month));
                            a4Days.Add(Math.Abs((a4xFDt[i] - a4xFDt[i + 1]).Days));
                        }
                        else
                        {
                            a4Months.Add((mUptoCalDt.Value.Year - a4xFDt[i].Year) * 12 + (mUptoCalDt.Value.Month - a4xFDt[i].Month));
                            a4Days.Add(Math.Abs((mUptoCalDt.Value - a4xFDt[i]).Days));
                        }
                    }
                }
                else
                {
                    a4Months.Add(mCalMths);
                    a4Days.Add(mDays1);
                }


                mQtyCons = Convert.ToInt32(Math.Round(Convert.ToDouble((double)mAvg / (double)30), 0) * mDaysBeforeFix);
                mQtyCons = mQtyCons + (mReading - mPrevRead);

                mQtyCons = mQtyCons != 0 ? mQtyCons : Convert.ToInt32(Math.Round(Convert.ToDouble((double)mAvg / (double)30), 0) * mDays1);
                string mCMth = " Not to Include in Bill";// reove later
                int mTAvg = 0;
                mTAvg = Convert.ToInt32(Math.Round(Convert.ToDouble((double)mAvg / (double)30), 0) * mDays1);
                int mTQty = mReading < mPrevRead || mReading == 0 || mReading == mPrevRead ? mTAvg : mReading - mPrevRead;

                if (GenerateBill.X_CurrStatus == "MOK")
                {
                    mQty = mQtyCons + mAdj;
                }
                else
                {
                    mQty = mQtyCons > mTAvg ? mQtyCons : mTAvg;
                    mQty = mQty + mAdj;
                }

                if (GenerateBill.X_PrevStatus == "INV" && GenerateBill.X_CurrStatus == "MOK")
                {
                    mPrevHLQty = GetPrevHLQtyNew(mCustNo);
                }

                mQty = mQty - mPrevHLQty;

                int mTAvg2 = 0;
                mRate2 = 0;
                int mCalMths2 = 0;
                mQty2 = 0;
                double xRate1 = 0;
                double xRate2 = 0;
                int xAmtMinC = 0;
                int xx1 = 0;
                int xx2 = 0;
                int xmMth1 = 0;
                int xmMth2 = 0;
                int xmDays1 = 0;
                int xmDays2 = 0;

                //
                if (mEffDate1 != mEffDate2)
                {
                    if (mFromCalDt.Value.Day <= 20)
                    {
                        xmMth1 = mFromCalDt.Value.Month;
                    }
                    else if (mFromCalDt.Value.Day > 20)
                    {
                        if (mFromCalDt.Value.Month == 12)
                        {
                            xmMth1 = 1;
                        }
                        else if (mFromCalDt.Value.Month < 12)
                        {
                            xmMth1 = mFromCalDt.Value.Month - 1;
                        }
                    }

                    if (mEffDate1.Value.Day <= 20)
                    {
                        xmMth2 = mEffDate1.Value.Month;
                    }
                    else if (mEffDate1.Value.Day > 20)
                    {
                        if (mEffDate1.Value.Month == 1)
                        {
                            xmMth2 = 12;
                        }
                        else if (mEffDate1.Value.Month > 1)
                        {
                            xmMth2 = mEffDate1.Value.Month + 1;
                        }
                    }
                    mCalMths2 = (mEffDate1.Value.Year - mFromCalDt.Value.Year) * 12 + (mEffDate1.Value.Month - mFromCalDt.Value.Month);
                    mDays2 = Convert.ToInt32(Math.Abs((mEffDate1.Value - mFromCalDt.Value).Days));
                }

                mDays2 = mDays2 == mDays1 ? 0 : mDays2;
                //if (mDays2 < 0)
                //{
                //    mDays2 = 0;
                //}
                mCalMths2 = mCalMths2 == mCalMths ? 0 : mCalMths2;
                //if (mCalMths2 < 0)
                //{
                //    mCalMths2 = 0;
                //}

                mCalMths = mCalMths - mCalMths2;
                mDays1 = mDays1 - mDays2;
                if (mStatus == "MOK")
                {
                    mQty = mQtyCons + mAdj;
                    mBillType = "W";
                }
                else if (mStatus != "MOK")
                {
                    mBillType = "A";
                }

                if (mMTypeCode == "BRD")
                {
                    //SELECT * From BoardRent WHERE EffDateId = @mID AND MeterSizeId = @mMSId
                    foreach (var _Rate1 in _ObjBoardRent.getBoardRentMasterSelectWhere("and Cast(EffectDate as Date) = Convert(Date,'" + mEffDate1 + "' ,103) and RefMeterSizeId = " + mMSizeId).Select(x => x.Rate).ToList())
                    {
                        mRent1 = mEffDate1 == mEffDate2 ? Convert.ToInt32(Math.Round(_Rate1 / 30 * mDays1, 0)) : 0;
                        mRent2 = 0;
                        if (mEffDate1 != mEffDate2)
                        {
                            for (int i = 0; i < a4xEDt.Count; i++)
                            {
                                foreach (var _Rate2 in _ObjBoardRent.getBoardRentMasterSelectWhere("and Cast(EffectDate as Date) = Convert(Date,'" + a4xEDt[i] + "', 103) and RefMeterSizeId = " + mMSizeId).Select(x => x.Rate).ToList())
                                {
                                    mRent2 = mRent2 + Convert.ToInt32(Math.Round((_Rate2 / 30) * a4Days[i], 0));
                                }
                            }
                        }
                        mRent = mRent1 + mRent2;
                    }
                }

                int mDy = 0;
                int mM = 0;
                for (int i = 0; i < a4xEDt.Count; i++)
                {
                    mM = mM + a4Months[i];
                    mDy = mDy + a4Days[i];
                }
                int mQtyToCheck = mQty > 0 ? mQty : mAvg;
                if (mQty > 0)
                    mQtyToCheck = mQty;
                else
                    mQtyToCheck = Convert.ToInt32(Math.Round(Convert.ToDouble((mAvg / 30) * mDy), 0));

                bool mIsAsPerSlab = false;
                //if (GenerateBill.X_CurrStatus == "MOK")
                //{
                //    mIsAsPerSlab = true;
                //}

                if (mIsAsPerSlab)
                {
                    if (mSupCode == "DOM")
                        mIsAsPerSlab = false; //true; Check from Config. Master 
                    else
                        mIsAsPerSlab = false;
                }

                if (mIsAsPerSlab)
                {
                    xRate1 = 0;
                    xRate2 = 0;

                    double mQtyForRate = Math.Round(Convert.ToDouble(mQtyToCheck / mNoOfFlats), 2) * 1000;
                    double mMonthlyQtyForRate = Math.Round(Convert.ToDouble(mQtyToCheck / mDy) * 30, 2);

                    if (mEffDate1 == mEffDate2)
                    {
                        //xRate1 = xRate1 + objRateSlab.GetRateSlab1(mEffDate1Id, mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                    }
                    else
                    {
                        int xR = 0;
                        for (int i = 0; i < a4xEDt.Count; i++)
                        {
                            //xRate1 = xRate1 + objRateSlab.GetRateSlab1(a4xEDtId(ii), mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                        }
                    }

                    if (mMonthlyQtyForRate > 15000)
                    {
                        if (mEffDate1 == mEffDate2)
                        {
                            //xRate1 = xRate1 + objRateSlab.GetRateSlab2(mEffDate1Id, mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                        }
                        else
                        {
                            int xR = 0;
                            for (int i = 0; i < a4xEDt.Count; i++)
                            {
                                //xRate1 = xRate1 + objRateSlab.GetRateSlab2(a4xEDtId(ii), mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                            }

                        }
                    }

                    if (mMonthlyQtyForRate > 20000)
                    {
                        if (mEffDate1 == mEffDate2)
                        {//xRate1 = xRate1 + objRateSlab.GetRateSlab3(mEffDate1Id, mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);

                        }

                        else
                        {
                            int xR = 0;
                            for (int i = 0; i < a4xEDt.Count; i++)
                            {
                                //xRate1 = xRate1 + objRateSlab.GetRateSlab3(a4xEDtId(ii), mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                            }
                        }

                    }

                    if (mMonthlyQtyForRate > 25000)
                    {
                        if (mEffDate1 == mEffDate2)
                        {
                            //xRate1 = xRate1 + objRateSlab.GetRateSlab4(mEffDate1Id, mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                        }
                        else
                        {
                            int xR = 0;
                            for (int i = 0; i < a4xEDt.Count; i++)
                            {
                                //xRate1 = xRate1 + objRateSlab.GetRateSlab4(a4xEDtId(ii), mSupTypeId, mQtyToCheck, mNoOfFlats, mDy);
                            }
                        }

                    }

                }
                else
                {
                    foreach (var _ConsumeRate1 in _ObjConsumerRate.SelectConsumerRateSelectWhere(" and Cast(EffectDate as Date) = Convert(Date,'" + mEffDate1 + "' ,103) and RefSupplyCategoryId = " + mSupCategoryId + " and RefSupplyTypeId = " + mSupTypeId).Select(x => x.Rate).ToList())
                    {
                        if (mEffDate1 == mEffDate2)
                        {
                            xRate1 = Convert.ToDouble(_ConsumeRate1);
                        }
                        else
                        {
                            int xR = 0;
                            for (int i = 0; i < a4xEDt.Count; i++)
                            {
                                foreach (var _ConsumeRate2 in _ObjConsumerRate.SelectConsumerRateSelectWhere(" and Cast(EffectDate as Date) = Convert(Date,'" + a4xEDt[i] + "',103) and RefSupplyCategoryId = " + mSupCategoryId + " and RefSupplyTypeId = " + mSupTypeId).Select(x => x.Rate).ToList())
                                {
                                    xR = xR + Convert.ToInt32(Math.Round((_ConsumeRate2 / 30) * a4Days[i], 0));
                                }
                            }
                            mM = 0;
                            xRate1 = Convert.ToInt32(Math.Round(Convert.ToDouble(xR / mDy), 2));
                        }

                    }
                }

                int mWorkStatId = 0;
                int mNWorkStatID = 0;
                int mStatusId = 0;
                mWorkStatId = _ObjMastervalue.getMasterValueWhere((int)clsCommon.MasterType.MeterStatus, " and ShortName = 'MOK'").FirstOrDefault().ID;
                mNWorkStatID = _ObjMastervalue.getMasterValueWhere((int)clsCommon.MasterType.MeterStatus, " and ShortName = 'NWK'").FirstOrDefault().ID;
                double mMinChargesWrk = 0;
                double mMinChargesNWK = 0;

                mStatusId = mStatus == "MOK" ? mWorkStatId : mNWorkStatID;
                _WhereCondition = " and Cast(EffectDate as Date) = Convert(Date,'" + mEffDate1 + "' ,103) and RefSupplyTypeId = " + mSupTypeId + " and RefMeterStatusId = " + mStatusId + " and RefMeterSizeId = " + mMSizeId;
                foreach (var _MeterRate1 in _ObjMeterMinCharge.getMeterMinChargeSelectWhere(_WhereCondition).Select(x => x.Rate).ToList())
                {
                    if (mEffDate1 == mEffDate2)
                    {
                        mMinChargesWrk = Convert.ToDouble(_MeterRate1 * mCalMths);
                    }
                    else
                    {
                        mMinChargesWrk= 0;
                        for (int i = 0; i < a4xEDt.Count; i++)
                        {
                            _WhereCondition = " and Cast(EffectDate as Date) = Convert(Date ,'" + a4xEDt[i] + "', 103) and RefSupplTypeId = " + mSupTypeId + " and RefMeterStatusId = " + mStatusId + " and RefMeterSizeId = " + mMSizeId;
                            foreach (var _MeterRate2 in _ObjMeterMinCharge.getMeterMinChargeSelectWhere(_WhereCondition).Select(x => x.Rate).ToList())
                            {
                                mMinChargesWrk = mMinChargesWrk+ Convert.ToDouble(_MeterRate2 * a4Months[i]);
                            }
                        }
                    }
                }
                mStatusId = mStatus == "NWK" ? mWorkStatId : mNWorkStatID;
                _WhereCondition = " and Cast(EffectDate as Date) = Convert(Date,'" + mEffDate1 + "' ,103) and RefSupplyTypeId = " + mSupTypeId + " and RefMeterStatusId = " + mStatusId + " and RefMeterSizeId = " + mMSizeId;
                foreach (var _MeterRate1 in _ObjMeterMinCharge.getMeterMinChargeSelectWhere(_WhereCondition).Select(x => x.Rate).ToList())
                {
                    if (mEffDate1 == mEffDate2)
                    {
                        mMinChargesNWK = Convert.ToDouble(_MeterRate1 * mDiff1);
                    }
                    else
                    {
                        mMinChargesNWK = 0;
                        for (int i = 0; i < a4xEDt.Count; i++)
                        {
                            _WhereCondition = " and Cast(EffectDate as Date) = Convert(Date ,'" + a4xEDt[i] + "', 103) and RefSupplTypeId = " + mSupTypeId + " and RefMeterStatusId = " + mStatusId + " and RefMeterSizeId = " + mMSizeId;
                            foreach (var _MeterRate2 in _ObjMeterMinCharge.getMeterMinChargeSelectWhere(_WhereCondition).Select(x => x.Rate).ToList())
                            {
                                mMinChargesNWK = mMinChargesNWK + Convert.ToDouble(_MeterRate2 * a4Months[i]);
                            }
                        }
                    }
                }

                xRate2 = mMinChargesNWK + mMinChargesWrk;
                //if (mIsAsPerSlab)
                //{
                //    xx1 = Convert.ToInt32(xRate1);
                //}
                //else
                //{
                //    xx1 = Convert.ToInt32(Math.Round((mQty * xRate1), 0));
                //}

                //ask about this condition
                //string mSupCategoryCode = _ObjMastervalue.getMasterValue(8, Convert.ToInt32(mSupTypeId)).FirstOrDefault().ShortName;
                //if (mSupCategoryCode == "RET")
                //{
                //    xx1 = Convert.ToInt32(xRate1);
                //}
                //else
                //{
                //    xx1 = Convert.ToInt32(Math.Round((mQty * xRate1), 0));
                //}
                xx1 = Convert.ToInt32(Math.Round((mQty * xRate1), 0));
                xx1 = Convert.ToInt32(xRate1);
                xx2 = Convert.ToInt32(xRate2);
                mRate = Convert.ToDecimal(xRate1);

                if (mReading == 0)
                {
                    mBillType = "A";
                }

                bool mIsOld = false; // remove later
                //foreach (var _BillDate in _ObjConsumeDetail.get_Consumerdetail_SelectWhere(" and RefConsumerId = (Select Id From ConsumerMaster Where ConsumerNo = '" + mCustNo + "')").Select(x => x.BillDate).ToList())
                //{
                //    if (_BillDate == GenerateBill.X_IssueDate)
                //        mIsOld = true;
                //}

                int mAmt = 0;
                if (xx1 > xx2 || xx1 == 0 || xx2 == 0)
                {
                    int xAmt1 = 0;
                    int xAmt2 = 0;
                    //if (mIsAsPerSlab)
                    //{
                    xAmt1 = xx1;
                    //}
                    //else
                    //{
                    //    if (mRate != 0)
                    //        xAmt1 = Convert.ToInt32(Math.Round((mRate * mQty), 0));
                    //    if (mRate2 != 0)
                    //        xAmt2 = Convert.ToInt32(Math.Round((mRate2 * mQty2), 0));
                    //}
                    //mAmt = xAmt1 + xAmt2;
                }
                else
                {
                    mBillType = "M";
                    mAmt = Convert.ToInt32(Math.Round(Convert.ToDecimal(xx2), 0));
                }

                int mAmt4DPC = mOpBal;

                int mD = 0;

                //int mOstBal = mOpBal + mAmt + mRent + mCurrMthDpc + mDebitNoteAmount - mCreaditNoteAmount ;
                //mOstBal = Convert.ToInt32(mOstBal - mRebate );
                int mTillDateDPC = 0;
                int mDaysForDPC = 0;
                if (!mIsRcptReceipved)
                {
                    mDaysForDPC = (mUptoCalDt.Value - mFromCalDt1.Value).Days;
                }
                else
                {
                    mDaysForDPC = (mUptoCalDt.Value - mLReceiptDate.Value).Days;
                }
                mTillDateDPC = Convert.ToInt32(Math.Round(Convert.ToDouble((((mOpBal) * (X_DPC / 100)) / 365) * mDaysForDPC), 0));
                int mDpcBal = Convert.ToInt32(mOpDpc + mDebitNoteDPCAmount - mCreaditNoteDPCAmount + mTillDateDPC);
                int mBillBalance = Convert.ToInt32(mOpBal + mAmt + mRent - mRebate + mDebitNoteDPCAmount - mCreaditNoteAmount);
                //int mDueDateGap = (GenerateBill.X_IssueDate - GenerateBill.x_DueDate).Days;
                //mDueDateGap = mDueDateGap * mDueDateGap < 0 ? -1 : 1;

                //mD = Convert.ToInt32(Math.Round(Convert.ToDouble((((mBFABasic + mOpBal + mAmt + mRent) * (X_DPC / 100)) / 365) * 30), 0));

                //if (!GenerateBill.X_ToCalDPC)
                //    mD = 0;

                //// GenBillNoNew() function is create to pending.
                ////string mBNo = GenBillNoNew();

                //mD = mD < 0 ? 0 : mD;

                int ConsumerId = _cnn.sp_ConsumerMaster_SelectWhere(" and ConsumerNo = '" + mCustNo + "'").FirstOrDefault().Id;
                string BillNo = "";
                int InsUser = 0;
                string InsTerminal = "";
                int UpdUser = 0;
                string UpdTerminal = "";
                DateTime ReadingDate = System.DateTime.Now;
                decimal MeterReading = 0;



                var _result = Convert.ToBoolean(_ObjConsumeDetail.set_Consumerdetail_save(Id, ConsumerId, Convert.ToInt32(mReaderId),
                        mMSizeId, GenerateBill.X_CurrStatusID, Convert.ToDateTime(ReadingDate), MeterReading, mBillType, "NorBill",
                        mMTypeId, mSupTypeId, mSupCategoryId, mPReadDate, mPrevRead, 1, GenerateBill.X_IssueDate, Convert.ToDateTime(mLIDate), GenerateBill.x_DueDate,
                        mAmt, mOpBal, mBillBalance, mRent, mOpDpc, mDpcBal, mTillDateDPC,
                        mD, mRebate, mQty, mQty2, mAdj, mPrevHLQty, mRate, mRate2, mQtyCons, mAvg,
                        mQty + mQty2, mBookNo, mNoOfFlats, InsUser, InsTerminal, UpdUser, UpdTerminal).Value);

                DataSet _Ds = new DataSet();
                if (_result)
                {
                    #region SetData In Datatable
                    Database _ObjDb = new SqlDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                    using (DbCommand _ObjCmd = _ObjDb.GetStoredProcCommand("sp_BillDetail_Select"))
                    {
                        _ObjDb.AddInParameter(_ObjCmd, "@pId", DbType.Int32, Id);

                        _Ds = _ObjDb.ExecuteDataSet(_ObjCmd);
                    }
                    #endregion
                }
                return _Ds;

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int GetPrevHLQtyNew(string _ConsumerNo)
        {
            int mHLQty = 0;
            try
            {
                int _MeterStatusId = _cnn.sp_MasterValue_SelectWhere((int)clsCommon.MasterType.MeterStatus, " and ShortName = 'HLK'").FirstOrDefault().ID;
                foreach (sp_ConsumeDetail_SelectWhere_Result _ObjHlk in _cnn.sp_ConsumeDetail_SelectWhere(" and RefConsumerId = (Select Id from ConsumerMaster Where ConsumerNo = '" + _ConsumerNo + "') and Cast(BillDate as Date) < Cast('" + GenerateBill.X_IssueDate + "' as Date)").OrderByDescending(x => x.BillDate).ToList())
                {
                    if (_ObjHlk.RefMeterStatusId == _MeterStatusId)
                        mHLQty = mHLQty + Convert.ToInt32(_ObjHlk.Qty);
                    else
                        return mHLQty;
                }
                return mHLQty;

            }
            catch (Exception ex)
            {
                return mHLQty;
            }

        }

    }
}
