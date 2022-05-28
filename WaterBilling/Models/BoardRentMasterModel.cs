using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class BoardRentMasterModel
    {
        public int ID { get; set; }
        public DateTime EffectDate { get; set; }
        public int RefMeterTypeId { get; set; }

        public string MeterType { get; set; }

        public int RefMeterSizeId { get; set; }
        public string MeterSize { get; set; }

        public decimal oldRate { get; set; }

        public decimal Rate { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }

    }

    public class effectiveDates
    {
        public DateTime effectiveDate { get; set; }
    }
}