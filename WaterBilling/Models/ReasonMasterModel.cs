using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WaterBilling.Models
{
    public partial class ReasonMasterModel
    {
        public int ID { get; set; }

        public int refReasonTypeID { get; set; }

        public string ReasonType { get; set; }

        [StringLength(500, ErrorMessage = "Reson cannot be longer than 500 characters.")]
        public string ReasonName { get; set; }

        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}