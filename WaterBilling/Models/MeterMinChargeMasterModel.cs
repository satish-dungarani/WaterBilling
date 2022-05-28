using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class MeterMinChargeMasterModel
    {
        public int Id { get; set; }
        public DateTime EffectDate { get; set; }
        public int RefSupplyTypeId { get; set; }
        public string SupplyType { get; set; }
        public int RefMeterSizeId { get; set; }
        public string MeterSize { get; set; }
        public int RefMeterStatusId { get; set; }
        public string MeterStatus { get; set; }
        public decimal Rate { get; set; }
        public decimal LastRate { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}