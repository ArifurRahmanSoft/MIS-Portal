using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public class vmLoginUser
    {
        public string EmpID { get; set; }
        public string UserPassw { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UpdateBy { get; set; }
    }
    public class vmAuthenticatedUser
    {
        public decimal ReturnValue { get; set; }
        public string UserID { get; set; }
        public decimal IsFirstLogin { get; set; }
        public decimal CompanyID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CustomCode { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserFullName { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public string UserTypeName { get; set; }
        public Nullable<int> UserGroupID { get; set; }
        public string GroupName { get; set; }
        public string TargetPath { get; set; }
        public bool IsTemp { get; set; }
    }
    public class vmRecoverUser
    {
        public Nullable<int> ReturnValue { get; set; }
        public string RecoverEmail { get; set; }
        public string UserID { get; set; }
        public string LoginID { get; set; }
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public string RequestedIP { get; set; }
        public string CompanyName { get; set; }

    }
}
