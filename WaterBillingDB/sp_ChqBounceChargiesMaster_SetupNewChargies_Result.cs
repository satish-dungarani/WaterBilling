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
    
    public partial class sp_ChqBounceChargiesMaster_SetupNewChargies_Result
    {
        public int Id { get; set; }
        public System.DateTime EffectDate { get; set; }
        public int RefBankId { get; set; }
        public string BankName { get; set; }
        public int RefReasonId { get; set; }
        public string ReasonType { get; set; }
        public decimal LastChargies { get; set; }
        public decimal Chargies { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}