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
    
    public partial class sp_UserMaster_SelectWhere_Result
    {
        public int ID { get; set; }
        public int RefuserTypeID { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> RefRoleID { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public bool AllowMobileRegistration { get; set; }
        public int InsUser { get; set; }
        public System.DateTime InsDate { get; set; }
        public string InsTerminal { get; set; }
        public Nullable<int> UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}
