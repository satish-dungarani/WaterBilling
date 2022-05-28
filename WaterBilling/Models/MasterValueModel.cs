using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class MasterValueModel
    {

        public int refMasterID { get; set; }
        public int ID { get; set; }

        [StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters.")]
        public string ValueName { get; set; }
        public string ShortName { get; set; }
        public string MarathiName { get; set; }
        public decimal OrdNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemGenerated { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}