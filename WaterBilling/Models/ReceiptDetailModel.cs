using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class ReceiptDetailModel
    {
        public int Id { get; set; }
        public int RefCollectionCenterId { get; set; }
        public List<int> RefChqStatusCCId { get; set; }
        public decimal CounterNo { get; set; }
        public int ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string ChqStatusReceiptDate { get; set; }
        public int RefConsumerId { get; set; }
        public string ConsumerNo { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerAddress { get; set; }
        public decimal RecAmt { get; set; }
        public decimal BalanceAmt { get; set; }
        public int RefPaymentTypeId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        //public int? BankName { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string IsChqStatus { get; set; }
        public bool ChqStatus { get; set; }
        public string ChqBounceRefNo { get; set; }
        public DateTime? ChqBounceDate { get; set; }
        public decimal? ChqBounceCharge { get; set; }
        public decimal? DPCBal { get; set; }
        public decimal? DPCPaid { get; set; }
        public decimal? BasicBal { get; set; }
        public decimal? BasicPaid { get; set; }
        public decimal? BasicBalPending { get; set; }
        public decimal? DPCBalPending { get; set; }
        public DateTime? DueDate { get; set; }
        public string FrmType { get; set; }
        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}