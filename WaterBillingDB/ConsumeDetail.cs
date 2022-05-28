//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WaterBillingDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConsumeDetail
    {
        public int Id { get; set; }
        public int RefConsumerId { get; set; }
        public int RefConsumerMeterId { get; set; }
        public int RefReaderId { get; set; }
        public int RefMeterStatusId { get; set; }
        public Nullable<int> RefMeterSizeId { get; set; }
        public string BillType { get; set; }
        public string BillMode { get; set; }
        public Nullable<int> RefMeterTypeId { get; set; }
        public Nullable<int> RefSupplyTypeId { get; set; }
        public System.DateTime ReadingDate { get; set; }
        public Nullable<System.DateTime> PrevReadingDate { get; set; }
        public decimal MeterReading { get; set; }
        public Nullable<decimal> LastReading { get; set; }
        public Nullable<decimal> BillNo { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<System.DateTime> PrevBillDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> AssessmentAmt { get; set; }
        public Nullable<decimal> PrevAmount { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> Rent { get; set; }
        public Nullable<decimal> PrevDPC { get; set; }
        public Nullable<decimal> DueDPC { get; set; }
        public Nullable<decimal> DpcToCharge { get; set; }
        public Nullable<decimal> Rebate { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<int> Qty2 { get; set; }
        public Nullable<int> QtyAdj { get; set; }
        public Nullable<int> HlkQtyAdj { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Rate2 { get; set; }
        public Nullable<int> QtyConsum { get; set; }
        public Nullable<int> ConsumAverage { get; set; }
        public Nullable<int> QtyBilled { get; set; }
        public string BookNo { get; set; }
        public Nullable<int> NoOfFlats { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
        public Nullable<int> RefSupplyCategoryId { get; set; }
        public Nullable<decimal> TillDateDPC { get; set; }
    
        public virtual ConsumerMaster ConsumerMaster { get; set; }
    }
}
