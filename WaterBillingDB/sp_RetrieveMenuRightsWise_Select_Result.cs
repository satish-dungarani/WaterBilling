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
    
    public partial class sp_RetrieveMenuRightsWise_Select_Result
    {
        public string MenuName { get; set; }
        public int ID { get; set; }
        public string MenuDes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ParentMenuID { get; set; }
        public Nullable<int> refMenuGroupId { get; set; }
        public int OrderNo { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string MenuPath { get; set; }
        public int RefRoleId { get; set; }
        public int RefMenuId { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}
