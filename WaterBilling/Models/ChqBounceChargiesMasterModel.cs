using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class ChqBounceChargiesMasterModel
    {
        public int Id { get; set; }
        public DateTime EffectDate { get; set; }
        public string BankName { get; set; }
        public int RefBankId { get; set; }
        public string ReasonType { get; set; }
        public int RefReasonTypeID { get; set; }
        public decimal Chargies { get; set; }
        public decimal LastChargies { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}