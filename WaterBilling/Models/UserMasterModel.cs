using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBilling.Models
{
    public partial class UserMasterModel
    {
        public int ID { get; set; }
        public int refUserTypeID { get; set; }
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
        public int refRoleID { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public string UserReg { get; set; }
        public bool AllowMobileRegistration { get; set; }

        public List<int> RefCollectionCenterId { get; set; }
        public Boolean CollCentIsActive { get; set; }

        public int InsUser { get; set; }
        public Nullable<System.DateTime> InsDate { get; set; }
        public string InsTerminal { get; set; }
        public int UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerminal { get; set; }
    }
}