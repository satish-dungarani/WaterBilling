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
    
    public partial class OtherChargies
    {
        public int Id { get; set; }
        public System.DateTime EffectDate { get; set; }
        public Nullable<double> DPCPercantage { get; set; }
        public Nullable<double> Rebate { get; set; }
        public Nullable<double> RebateNext { get; set; }
        public Nullable<int> RebateDays { get; set; }
    }
}