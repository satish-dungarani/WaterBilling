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
    
    public partial class MasterValue
    {
        public MasterValue()
        {
            this.MeterMinChargeMaster = new HashSet<MeterMinChargeMaster>();
            this.MeterMinChargeMaster1 = new HashSet<MeterMinChargeMaster>();
            this.UserCollCentRights = new HashSet<UserCollCentRights>();
            this.ChqBounceChargiesMaster = new HashSet<ChqBounceChargiesMaster>();
            this.ChqBounceChargiesMaster1 = new HashSet<ChqBounceChargiesMaster>();
            this.ConsumeRateMaster = new HashSet<ConsumeRateMaster>();
            this.ConsumeRateMaster1 = new HashSet<ConsumeRateMaster>();
            this.ReceiptDetail = new HashSet<ReceiptDetail>();
            this.ReceiptDetail1 = new HashSet<ReceiptDetail>();
        }
    
        public int RefMasterId { get; set; }
        public int ID { get; set; }
        public string ValueName { get; set; }
        public string ShortName { get; set; }
        public string MarathiName { get; set; }
        public Nullable<decimal> OrdNo { get; set; }
        public bool IsActive { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    
        public virtual MastersList MastersList { get; set; }
        public virtual ICollection<MeterMinChargeMaster> MeterMinChargeMaster { get; set; }
        public virtual ICollection<MeterMinChargeMaster> MeterMinChargeMaster1 { get; set; }
        public virtual ICollection<UserCollCentRights> UserCollCentRights { get; set; }
        public virtual ICollection<ChqBounceChargiesMaster> ChqBounceChargiesMaster { get; set; }
        public virtual ICollection<ChqBounceChargiesMaster> ChqBounceChargiesMaster1 { get; set; }
        public virtual ICollection<ConsumeRateMaster> ConsumeRateMaster { get; set; }
        public virtual ICollection<ConsumeRateMaster> ConsumeRateMaster1 { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetail { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetail1 { get; set; }
    }
}
