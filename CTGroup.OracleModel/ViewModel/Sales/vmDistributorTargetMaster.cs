using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.Sales
{
    public class vmDistributorTargetMaster
    {
        public string FILE_NAME { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DOCUMENTID { get; set; }
        public string DOCUMENTNAME { get; set; }
        public string AREA_NATIONAL { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string VIRTUALPATH { get; set; }
        public decimal DIST_TARGET_ID { get; set; }
        public string DistributorTargetNo { get; set; }
        public DateTime DistributorTargetDate { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int? TransactionTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? Created_By { get; set; }
        public string CustomCode { get; set; }
        public long DISTRIBUTORTARGETID { get; set; }
        public string DISTRIBUTOR_ID { get; set; }
        public string DISTRIBUTOR_NAME { get; set; }
        public string SALES_TYPE { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string Created_IP { get; set; }
        public Nullable<int> Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_On { get; set; }
        public string Updated_Ip { get; set; }
        public bool Is_Deleted { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Deleted_By { get; set; }
        public Nullable<System.DateTime> Deleted_On { get; set; }
        public string Deleted_Ip { get; set; }
    }
}
