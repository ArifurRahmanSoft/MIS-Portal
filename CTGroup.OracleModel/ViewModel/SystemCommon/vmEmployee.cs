using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.OracleModel.ViewModel.SystemCommon
{
    public class vmEmployee
    {
        public string STAFFID { get; set; }
        public string STAFFNAME { get; set; }
        public string USERID { get; set; }
        public string USERFULLNAME { get; set; }
        public string ENCRYPTPASSWORD { get; set; }
        public string AEMP_OMAIL { get; set; }
        public string NAME { get; set; }
        public decimal? COLLECTORID { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string TELEPHONE { get; set; }
        public int? ReturnValue { get; set; }
        public decimal? BuildingId { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LoggedUser { get; set; }
        public string EmployeeID { get; set; }
        public string ContractEmailID { get; set; }
        public Nullable<int> UserID { get; set; }

        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserFullName { get; set; }

        public Nullable<int> GenderID { get; set; }
        public string Gender { get; set; }

        public string LoginID { get; set; }
        public string LoginEmail { get; set; }
        public string LoginPhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string IsActive { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
}
