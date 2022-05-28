using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public class DrCrNoteModel
    {
        public int Id { get; set; }
        public string NoteType { get; set; }
        public int SerialNo { get; set; }
        public int RefConsumerId { get; set; }
        public string ConsumerNo { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerAddress { get; set; }
        public DateTime? NoteDate { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public int? RefReasonId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public HttpPostedFileBase file { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}