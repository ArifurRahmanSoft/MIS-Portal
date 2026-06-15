using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Models.ViewModel.SystemCommon
{
    public partial class vmCmnMenuPermission
    {
        // new code block
        public decimal MENUID { get; set; }
        public string MENUNAME { get; set; }
        public decimal MODULEID { get; set; }
        public string MODULENAME { get; set; }
        public string USERID { get; set; }
        public decimal MENUPERMISSIONID { get; set; }
        public string USERFULLNAME { get; set; }
        public decimal SEQUENCE { get; set; }
        public decimal ENABLEVIEW { get; set; }
        public decimal ENABLEINSERT { get; set; }
        public decimal ENABLEUPDATE { get; set; }
        public decimal ENABLEDELETE { get; set; }

        public bool ENABLE_VIEW { get; set; }
        public bool ENABLE_INSERT { get; set; }
        public bool ENABLE_UPDATE { get; set; }
        public bool ENABLE_DELETE { get; set; } // in future use it for handling check uncheck issue

        // new code block




        [Display(Name = "Menu ID")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MenuID { get; set; }

        [Display(Name = "Menu Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string MenuName { get; set; }

        [Display(Name = "Sequence")]
        public int? Sequence { get; set; }

        [Display(Name = "Module I D")]
        public int? ModuleID { get; set; }

        [Display(Name = "Module Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string ModuleName { get; set; }

        [Display(Name = "Menu Permission I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MenuPermissionID { get; set; }
        
        [Display(Name = "User I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int UserID { get; set; }

        [Display(Name = "User Full Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string UserFullName { get; set; }
        
        [Display(Name = "Enable View")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableView { get; set; }

        [Display(Name = "Enable Insert")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableInsert { get; set; }

        [Display(Name = "Enable Update")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableUpdate { get; set; }

        [Display(Name = "Enable Delete")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableDelete { get; set; }

        [Display(Name = "Company I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CompanyID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string CompanyName { get; set; }

        [Display(Name = "Status I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int StatusID { get; set; }

        [Display(Name = "Status Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string StatusName { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [Display(Name = "Create By")]
        public int? CreateBy { get; set; }

        [Display(Name = "Create On")]
        public DateTime? CreateOn { get; set; }

        [Display(Name = "Create Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CreatePc { get; set; }

        [Display(Name = "Update By")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Update On")]
        public DateTime? UpdateOn { get; set; }

        [Display(Name = "Update Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string UpdatePc { get; set; }

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "Delete By")]
        public int? DeleteBy { get; set; }

        [Display(Name = "Delete On")]
        public DateTime? DeleteOn { get; set; }

        [Display(Name = "Delete Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DeletePc { get; set; }
      
    }
}
