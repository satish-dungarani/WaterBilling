using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class ConsumerMasterModel
    {
        public int Id { get; set; }
        public string ConsumerNo { get; set; }
        public string OldConsumerNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstNameMarathi { get; set; }
        public string MiddleNameMarathi { get; set; }
        public string LastNameMarathi { get; set; }
        public string Address { get; set; }
        public int RefCityId { get; set; }
        public int RefStateId { get; set; }
        public int RefCountryId { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public int RefZoneId { get; set; }
        public int RefCampId { get; set; }
        public int RefReaderId { get; set; }
        public int NoOfConnections { get; set; }
        public int FamilyMember { get; set; }
        public decimal AverageConsumption { get; set; }
        public decimal Deposite { get; set; }
        public int RefConstructionType { get; set; }
        public int RefMeterSizeId { get; set; }
        public int RefMeterTypeId { get; set; }
        public int RefSupplyTypeId { get; set; }
        public int RefSupplyCategoryId { get; set; }
        public string BookNo { get; set; }
        public string BeatNo { get; set; }
        public decimal FolioNo { get; set; }
        public string OddEven { get; set; }
        public int RefDMAId { get; set; }
        public Nullable<System.DateTime> ConnectionDate { get; set; }
        public string SancRef { get; set; }
        public string LetterNo { get; set; }
        public Nullable<System.DateTime> LetterDate { get; set; }
        public string TRNo { get; set; }
        public Nullable<System.DateTime> TRDate { get; set; }
        public string PlotId { get; set; }
        public string PlumberName { get; set; }
        public string PlumberLicNo { get; set; }
        public Nullable<System.DateTime> DisconnectedDate { get; set; }
        public string DisconnectionOrderNo { get; set; }
        public Nullable<System.DateTime> DisconnectionOrderDate { get; set; }
        public Nullable<System.DateTime> PrevBillDate { get; set; }
        public Nullable<System.DateTime> PrevReadingDate { get; set; }
        public decimal PrevReading { get; set; }
        public decimal PrevAssessmentAmt { get; set; }
        public decimal PrevAmount { get; set; }
        public decimal PrevDPC { get; set; }
        public decimal PrevRent { get; set; }
        public int PrevRefStatusId { get; set; }
        public string PrevStatus { get; set; }
        public Nullable<System.DateTime> ReConnectedDate { get; set; }
        public string ReconnectedOrderNo { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }



    }

    public partial class ConsumerMeterMasterModel
    {
        public int MeterId { get; set; }
        public int RefConsumerId { get; set; }
        public string MeterNo { get; set; }
        public string MeterMake { get; set; }
        public decimal InitialReading { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? InActiveDate { get; set; }
        public decimal MaxReadingNo { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }

    public partial class ConsumeDetailModel
    {
        public int Id { get; set; }
        public int RefConsumerId { get; set; }
        public int RefConsumerMeterId { get; set; }
        public string OddEven { get; set; }
        public int RefReaderId { get; set; }
        public int? RefMeterSizeId { get; set; }
        public string BillType { get; set; }
        public string BillMode { get; set; }
        public int? RefCampId { get; set; }
        public int? RefMeterTypeId { get; set; }
        public int? RefSupplyTypeId { get; set; }
        public int? RefSupplyCategoryId { get; set; }
        public DateTime? PRevReadingDate { get; set; }
        public decimal? LastReading { get; set; }
        public DateTime? PrevBillDate { get; set; }
        public int? Qty { get; set; }
        public int? Qty2 { get; set; }
        public int? QtyAdj { get; set; }
        public int? HlkQtyAdj { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Rate2 { get; set; }
        public int? QtyConsum { get; set; }
        public int? ConsumAverage { get; set; }
        public int? QtyBilled { get; set; }
        public string BookNo { get; set; }
        public int? NoOfFlats { get; set; }
        public int RefMeterStatusId { get; set; }
        public DateTime? ReadingDate { get; set; }
        public decimal MeterReading { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerAddress { get; set; }
        public DateTime? BillDate { get; set; }
        public Nullable<decimal> BillNo { get; set; }
        public Nullable<decimal> AssessmentAmt { get; set; }
        public Nullable<decimal> Rent { get; set; }
        public Nullable<decimal> PrevAmount { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> PrevDPC { get; set; }
        public Nullable<decimal> DueDPC { get; set; }
        public Nullable<decimal> TillDateDPC { get; set; }
        public Nullable<decimal> DpToCharge { get; set; }
        public Nullable<decimal> Rebate { get; set; }

        public Nullable<System.DateTime> DueDate { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }

    }

}