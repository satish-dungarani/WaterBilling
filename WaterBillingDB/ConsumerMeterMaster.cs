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
    
    public partial class ConsumerMeterMaster
    {
        public int Id { get; set; }
        public int RefConsumerId { get; set; }
        public string MeterNo { get; set; }
        public string MeterMake { get; set; }
        public Nullable<decimal> InitialReading { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public Nullable<decimal> MaxReadingNo { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    
        public virtual ConsumerMaster ConsumerMaster { get; set; }
    }
}
