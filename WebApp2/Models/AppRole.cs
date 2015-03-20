namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("app_role2")]
    public class AppRole
    {
        public AppRole()
        {
            this.SupportCompanyNo = 0;    // means PUBLIC(0) not PRIVATE(AppCompany.No) or TEMPLATE(-1)
            this.CreateTime = DateTime.Now;
            this.LastupdateTime = DateTime.Now;
            this.AppUserRole = new HashSet<AppUserRole>();
            this.AppFunction = new HashSet<AppFunction>();
        }

        [Column("role_no", Order=1), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("role_id", Order = 2)]
        [MaxLength(64)]
        [MinLength(2)]
        [Required]
        public string ID { get; set; }

        [Column("role_name", Order = 3)]
        [MaxLength(128)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; }

        [Column("company_no", Order = 4)]
        public int SupportCompanyNo { get; set; }

        [Column("create_time", Order = 5)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [Column("lastupdate_time", Order = 6)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? LastupdateTime { get; set; }

        public virtual ICollection<AppUserRole> AppUserRole { get; set; }

        public virtual ICollection<AppFunction> AppFunction { get; set; }

        public override string ToString()
        {            
            return (string.Format("AppRole(No={0},ID={1},Name={2},CompanyNo={3},CreateTime={4},LastUpdateTime={5})",
                                  this.No, this.ID, this.Name, this.SupportCompanyNo,
                                  this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                                  (this.LastupdateTime == null) ? "" : ((DateTime)this.LastupdateTime).ToString("yyyy-MM-dd HH:mm:ss")));
        }       

    }
}
