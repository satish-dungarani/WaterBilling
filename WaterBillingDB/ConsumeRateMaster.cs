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
    
    public partial class ConsumeRateMaster
    {
        public int Id { get; set; }
        public System.DateTime EffectDate { get; set; }
        public int RefSupplyTypeId { get; set; }
        public int RefSupplyCategoryId { get; set; }
        public decimal Rate { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    
        public virtual MasterValue MasterValue { get; set; }
        public virtual MasterValue MasterValue1 { get; set; }
    }
}